using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace HSEnvPredict
{
    static class Program
    {
        private static ApiServer _apiServer;

        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            //MessageBox.Show("Initializing predictor...");
            hsEnvPredictor predictor = new hsEnvPredictor();
            predictor.InitialPred("E:\\KK\\計畫\\SP7_1\\For SP1\\HSEnvPredict - 1.0.0.2\\model_VGG_2D.onnx", 5, new hsHSITransfer());
            predictor.InitailRegion("E:\\\\KK\\\\計畫\\\\SP7_1\\\\For SP1\\\\HSEnvPredict - 1.0.0.2\\\\model_VGG_3D_reg.onnx", 10);

            //MessageBox.Show("Starting API server...");
            _apiServer = new ApiServer();
            _apiServer.Start();

            //MessageBox.Show("Starting Windows Forms application...");
            Application.Run(new Form1());

            //MessageBox.Show("Stopping API server...");
            _apiServer.Stop();
        }
    }
}
