using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace HSEnvPredict
{

    public class hsEnvRegionInfo
    {
        public Bitmap Image;
        public Rectangle Roi;
        public Int32 ResultClassIndex;
        public Single ResultReg;

        public hsEnvRegionInfo(Bitmap img, Rectangle roi)
        {
            Image = img;
            Roi = roi;
        }
    }

    public class hsEnvPredictor
    {
        private Int32 m_pred_class_num = -1;
        private InferenceSession m_pred_session = null;
        private String m_pred_input_name = null;

        private Int32 m_region_class_num = -1;
        private InferenceSession m_region_session = null;
        private String m_region_input_name = null;

        private hsHSITransfer m_hsi_transfer = null;

        private Double m_now_progress = 0.0;

        public Int32 PredClassNum
        {
            get { return m_pred_class_num; }
        }

        public Int32 RegionClassNum
        {
            get { return m_region_class_num; }
        }

        public Double NowProgress
        {
            get { return m_now_progress; }
        }

        public event EventHandler ProgressChanged;

        public void InitailRegion(String onnx_file, Int32 class_num)
        {
            m_region_session = new InferenceSession(onnx_file);
            m_region_input_name = m_region_session.InputMetadata.Keys.Single();

            m_region_class_num = class_num;
        }

        public void InitialPred(String onnx_file, Int32 class_num, hsHSITransfer hsi_trans)
        {
            m_pred_session = new InferenceSession(onnx_file);
            m_pred_input_name = m_pred_session.InputMetadata.Keys.Single();

            m_pred_class_num = class_num;

            m_hsi_transfer = hsi_trans;
        }

        public Int32 PredictRegionUnitClass(Bitmap img24)
        {
            if (img24 == null)
                return -1;

            float[] img_data = PreprocessTestImage(img24);
            //float[] img_data = m_hsi_transfer.Transfer(img24);
            //var imageFlattened = (img_data.SelectMany(x => x).ToArray()).SelectMany(x => x).ToArray();

            int[] dimensions = { 1, 64, 64, 3 };
            var inputTensor = new DenseTensor<float>(img_data, dimensions);
            var modelInput = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(m_region_input_name, inputTensor)
            };

            var result = m_region_session.Run(modelInput);
            var probabilities = ((DenseTensor<float>)result.Single().Value).ToArray();
            int prediction = probabilities.ToList().IndexOf(probabilities.Max());

            return prediction;
        }

        public Single PredictPredUnitReg(Bitmap img24)
        {
            if (img24 == null)
                return -1;

            Single prediction = -1.0f;

            try
            {
                //float[] img_data = PreprocessTestImage(img24);
                float[] img_data = m_hsi_transfer.Transfer(img24);
                //var imageFlattened = (img_data.SelectMany(x => x).ToArray()).SelectMany(x => x).ToArray();

                int[] dimensions = { 1, 64, 64, 11, 1 };
                var inputTensor = new DenseTensor<float>(img_data, dimensions);
                var modelInput = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(m_pred_input_name, inputTensor)
            };

                var result = m_pred_session.Run(modelInput);
                //var probabilities = ((DenseTensor<float>)result.Single().Value).ToArray();
                //int prediction = probabilities.ToList().IndexOf(probabilities.Max());

                var prediction_ary = ((DenseTensor<float>)result.Single().Value).ToArray();

                prediction = prediction_ary[0];

                if (prediction < 0.0f)
                    prediction = 0.0f;

                if (prediction > 1.0f)
                    prediction = 1.0f;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



            return prediction;
        }

        //public Int32 PredictUnitClass(String file)
        //{
        //    return PredictUnitClass(LoadImage(file));
        //}

        public hsEnvRegionInfo[] Predict(Bitmap img24)
        {
            if (img24 == null)
                return new hsEnvRegionInfo[0];

            m_now_progress = 0.0;

            hsEnvRegionInfo[] reg_ary = SplitImage(img24, 64, 0).ToArray();

            for (Int32 i = 0; i < reg_ary.Length; i++)
            {
                reg_ary[i].ResultClassIndex = PredictRegionUnitClass(reg_ary[i].Image);
                reg_ary[i].ResultReg = PredictPredUnitReg(reg_ary[i].Image);

                m_now_progress = (Double)(i + 1) / (Double)reg_ary.Length;
                //ProgressChanged(this, new EventArgs());
            }

            return reg_ary;
        }

        public hsEnvRegionInfo[] Predict(String file)
        {
            Bitmap img24 = LoadImage(file);

            return Predict(img24);
        }

        private float[] PreprocessTestImage(Bitmap img24)
        {
            BitmapData bmp_data = img24.LockBits(new Rectangle(0, 0, img24.Width, img24.Height), ImageLockMode.ReadOnly, img24.PixelFormat);
            Byte[] img_data = new Byte[bmp_data.Stride * bmp_data.Height];
            Marshal.Copy(bmp_data.Scan0, img_data, 0, img_data.Length);

            var result = new float[img24.Height * img24.Width * 3];

            for (int y = 0; y < img24.Height; y++)
            {
                for (int x = 0; x < img24.Width; x++)
                {
                    Int32 src_index = y * bmp_data.Stride + x * 3;
                    Int32 tar_index = y * bmp_data.Width * 3 + x * 3;

                    var b = img_data[src_index];
                    var g = img_data[src_index + 1];
                    var r = img_data[src_index + 2];

                    //result[tar_index] = (Single)b / 255.0f;
                    //result[tar_index + 1] = (Single)g / 255.0f;
                    //result[tar_index + 2] = (Single)r / 255.0f;

                    result[tar_index] = (Single)r;
                    result[tar_index + 1] = (Single)g;
                    result[tar_index + 2] = (Single)b;
                }
            }

            img24.UnlockBits(bmp_data);

            return result;
        }

        private List<hsEnvRegionInfo> SplitImage(Bitmap src_img, Int32 unit_size, Int32 overlap)
        {
            Int32 pitch = unit_size - overlap;

            List<hsEnvRegionInfo> sp_img_list = new List<hsEnvRegionInfo>();

            Rectangle img_roi = new Rectangle(0, 0, src_img.Width, src_img.Height);

            Int32 x_num = 0;
            Int32 y_num = 0;

            x_num = (img_roi.Width - unit_size) / pitch + 1;

            if (((img_roi.Width - unit_size) % pitch) != 0)
                x_num++;

            y_num = (img_roi.Height - unit_size) / pitch + 1;

            if (((img_roi.Height - unit_size) % pitch) != 0)
                y_num++;

            for (Int32 x = 0; x < x_num; x++)
            {
                for (Int32 y = 0; y < y_num; y++)
                {
                    Rectangle sp_roi = new Rectangle(0, 0, unit_size, unit_size);

                    sp_roi.X = x * pitch;
                    sp_roi.Y = y * pitch;

                    if (x == x_num - 1 && x_num > 1)
                        sp_roi.X = img_roi.Width - unit_size;

                    if (y == y_num - 1 && y_num > 1)
                        sp_roi.Y = img_roi.Height - unit_size;

                    if (sp_roi.X < img_roi.X)
                        sp_roi.X = img_roi.X;

                    if (sp_roi.Y < img_roi.Y)
                        sp_roi.Y = img_roi.Y;

                    if (sp_roi.Right > img_roi.Right)
                        sp_roi.X = img_roi.Right - sp_roi.Width;

                    if (sp_roi.Bottom > img_roi.Bottom)
                        sp_roi.Y = img_roi.Bottom - sp_roi.Height;

                    Bitmap tar_img = CopyImage(src_img, sp_roi);

                    sp_img_list.Add(new hsEnvRegionInfo(tar_img, sp_roi));
                }
            }

            return sp_img_list;
        }

        public static Bitmap LoadImage(String file)
        {
            if (!System.IO.File.Exists(file))
                return null;

            Bitmap img = null;
            System.IO.FileStream fs = null;

            try
            {
                fs = System.IO.File.Open(file, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

                img = new Bitmap(fs);
            }
            catch
            {


            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

            return img;
        }

        public static Bitmap CopyImage(Bitmap src_img, Rectangle tar_roi)
        {
            Bitmap tar_img = new Bitmap(tar_roi.Width, tar_roi.Height, src_img.PixelFormat);

            BitmapData src_img_data = src_img.LockBits(new Rectangle(0, 0, src_img.Width, src_img.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, src_img.PixelFormat);
            BitmapData tar_img_data = tar_img.LockBits(new Rectangle(0, 0, tar_roi.Width, tar_roi.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, tar_img.PixelFormat);

            for (Int32 y = 0; y < tar_roi.Height; y++)
            {
                Byte[] img_data = new Byte[tar_img_data.Stride];

                IntPtr src_img_data_pos = IntPtr.Add(src_img_data.Scan0, (y + tar_roi.Y) * src_img_data.Stride + tar_roi.X * 3);
                IntPtr tar_img_data_pos = IntPtr.Add(tar_img_data.Scan0, y * tar_img_data.Stride);

                Marshal.Copy(src_img_data_pos, img_data, 0, img_data.Length);
                Marshal.Copy(img_data, 0, tar_img_data_pos, img_data.Length);
            }

            src_img.UnlockBits(src_img_data);
            tar_img.UnlockBits(tar_img_data);

            return tar_img;
        }


    }
}
