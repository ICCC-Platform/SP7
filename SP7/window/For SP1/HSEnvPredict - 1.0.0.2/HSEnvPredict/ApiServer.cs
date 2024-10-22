using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace HSEnvPredict
{
    public class ApiServer
    {
        private HttpListener _listener;
        private string _uploadFolder;
        private hsEnvPredictor m_predictor;
        private hsHSITransfer m_hsi_transfer;
        private List<hsEnvRegionInfo> m_reg_info = new List<hsEnvRegionInfo>();

        public ApiServer()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://+:5000/api/");
            _uploadFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads");
            Directory.CreateDirectory(_uploadFolder);

            InitializePredictor();
        }

        private void InitializePredictor()
        {
            m_predictor = new hsEnvPredictor();
            m_hsi_transfer = new hsHSITransfer();

            string pred_region_md = "E:\\KK\\計畫\\SP7_1\\For SP1\\HSEnvPredict - 1.0.0.2\\model_VGG_2D.onnx";
            int pred_region_num = 4;
            string pred_info_md = "E:\\KK\\計畫\\SP7_1\\For SP1\\HSEnvPredict - 1.0.0.2\\model_VGG_3D_reg.onnx";

            m_hsi_transfer.Initial(AppDomain.CurrentDomain.BaseDirectory);
            m_predictor.InitialPred(pred_info_md, -1, m_hsi_transfer);
            m_predictor.InitailRegion(pred_region_md, pred_region_num);
        }

        public void Start()
        {
            //MessageBox.Show("Starting API server...");
            _listener.Start();
            Task.Run(() => Listen());
            //MessageBox.Show("API server started.");
        }

        private async Task Listen()
        {
            while (_listener.IsListening)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    Task.Run(() => HandleRequest(context));
                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"Error in listener: {ex.Message}");
                }
            }
        }

        private void HandleRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            response.AddHeader("Access-Control-Allow-Origin", "*");
            response.AddHeader("Access-Control-Allow-Methods", "POST, OPTIONS");
            response.AddHeader("Access-Control-Allow-Headers", "Content-Type");

            if (request.HttpMethod == "OPTIONS")
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Close();
                return;
            }

            if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/api/analyze")
            {
                HandleFileUpload(request, response);
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Close();
            }
        }

        private void HandleFileUpload(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    string json = reader.ReadToEnd();
                    dynamic data = JsonConvert.DeserializeObject(json);
                    if (data.file == null)
                    {
                        SendErrorResponse(response, "No file found in the request", HttpStatusCode.BadRequest);
                        return;
                    }

                    string base64File = data.file;
                    byte[] fileBytes = Convert.FromBase64String(base64File);
                    string fileName = Guid.NewGuid().ToString() + ".jpg";
                    string filePath = Path.Combine(_uploadFolder, fileName);

                    // 檢查文件頭信息
                    byte[] headerBuffer = new byte[8];
                    Array.Copy(fileBytes, headerBuffer, 8);
                    string headerHex = BitConverter.ToString(headerBuffer).Replace("-", " ");
                    //MessageBox.Show($"File header before saving: {headerHex}");

                    // 保存文件
                    File.WriteAllBytes(filePath, fileBytes);

                    //MessageBox.Show($"File saved to {filePath}");

                    if (ValidateImage(filePath))
                    {
                        // 進行圖像分析
                        var reg_info = m_predictor.Predict(filePath);

                        // 保存有顏色的圖像
                        string coloredImagePath = SaveColoredImage(reg_info, filePath);

                        // 將有顏色的圖像返回給 Linux
                        string base64ColoredImage = Convert.ToBase64String(File.ReadAllBytes(coloredImagePath));
                        var result = new { FileName = fileName, ColoredImage = base64ColoredImage };
                        SendJsonResponse(response, result);
                    }
                    else
                    {
                        File.Delete(filePath);
                        SendErrorResponse(response, "Invalid image file.", HttpStatusCode.BadRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error during file upload: {ex.Message}");
                SendErrorResponse(response, "Internal server error.", HttpStatusCode.InternalServerError);
            }
        }


        private string SendToLinux(object data)
        {
            try
            {
                using (var handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                    using (var client = new HttpClient(handler))
                    {
                        client.Timeout = TimeSpan.FromMinutes(5);
                        var json = JsonConvert.SerializeObject(data);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        // 記錄請求資料
                        File.WriteAllText("request_log.txt", $"Request URL: https://140.123.105.199:5000/api/upload\nRequest Headers:\n{content.Headers}\nRequest Content:\n{json}");

                        var response = client.PostAsync("https://140.123.105.199:5000/api/upload", content).Result;
                        var responseContent = response.Content.ReadAsStringAsync().Result;

                        // 記錄響應資料
                        File.WriteAllText("response_log.txt", $"Response Status Code: {response.StatusCode}\nResponse Headers:\n{response.Headers}\nResponse Content:\n{responseContent}");

                        return responseContent;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error sending to Linux: {ex.Message}");
                return null;
            }
        }




        private bool ValidateImage(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Length > 100 * 1024 * 1024) // 100 MB
                {
                    //MessageBox.Show($"File is too large: {fileInfo.Length / (1024 * 1024)} MB");
                    return false;
                }

                byte[] buffer = new byte[8];
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (fileStream.Read(buffer, 0, 8) != 8)
                    {
                       // MessageBox.Show("File is too small to be a valid image.");
                        return false;
                    }
                }

                string headerHex = BitConverter.ToString(buffer).Replace("-", " ");
                //MessageBox.Show($"File header after saving: {headerHex}");

                if (buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47)
                {
                    //MessageBox.Show("Valid PNG file detected.");
                    return true;
                }

                if (buffer[0] == 0xFF && buffer[1] == 0xD8)
                {
                    //MessageBox.Show("Valid JPEG file detected.");
                    return true;
                }

                //MessageBox.Show("Unknown image format.");
                return false;
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error validating image: {ex.Message}");
                return false;
            }
        }

        private void SendJsonResponse(HttpListenerResponse response, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var buffer = Encoding.UTF8.GetBytes(json);
            response.ContentType = "application/json";
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.Close();
        }

        private void SendErrorResponse(HttpListenerResponse response, string message, HttpStatusCode statusCode)
        {
            response.StatusCode = (int)statusCode;
            var buffer = Encoding.UTF8.GetBytes(message);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.Close();
        }

        private string SaveColoredImage(hsEnvRegionInfo[] reg_info, string originalImagePath)
        {
            // 根據分析結果生成有顏色的新圖像
            Bitmap bitmap = new Bitmap(originalImagePath);
            Graphics graphics = Graphics.FromImage(bitmap);

            foreach (var reg in reg_info)
            {
                float pred_value = (float)reg.ResultClassIndex / (float)m_predictor.RegionClassNum;
                Color color = Color.FromArgb(128, Rainbow(pred_value));
                Rectangle roi = reg.Roi;
                graphics.FillRectangle(new SolidBrush(color), roi);
            }

            string coloredImagePath = Path.Combine(Path.GetDirectoryName(originalImagePath), "colored_" + Path.GetFileName(originalImagePath));
            bitmap.Save(coloredImagePath);
            bitmap.Dispose();

            return coloredImagePath;
        }

        private Color Rainbow(float progress)
        {
            progress = progress * 0.7f + 0.1f;
            float div = (Math.Abs(progress % 1) * 6);
            int ascending = (int)((div % 1) * 255);
            int descending = 255 - ascending;

            switch ((int)div)
            {
                case 0:
                    return Color.FromArgb(255, 255, ascending, 0);
                case 1:
                    return Color.FromArgb(255, descending, 255, 0);
                case 2:
                    return Color.FromArgb(255, 0, 255, ascending);
                case 3:
                    return Color.FromArgb(255, 0, descending, 255);
                case 4:
                    return Color.FromArgb(255, ascending, 0, 255);
                default:
                    return Color.FromArgb(255, 255, 0, descending);
            }
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
            //MessageBox.Show("API server stopped.");
        }
    }
}
