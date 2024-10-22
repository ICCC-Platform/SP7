using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Numpy;
using Python.Runtime;

namespace HSEnvPredict
{
    public class hsHSITransfer
    {
        private NDarray M = null;
        private NDarray pca_eigenvectors = null;
        private NDarray pca_mean = null;
        private NDarray bound = null;
        private NDarray md = null;
        private NDarray TransM = null;

        private Double TransMin = 0.0;
        private Double TransMax = 1.0;

        public hsHSITransfer()
        {

        }

        public Boolean Initial(String start_up_path)
        {
            M = np.load(String.Format("{0}\\spec_a.npy", start_up_path));
            pca_eigenvectors = np.load(String.Format("{0}\\spec_b.npy", start_up_path));
            pca_mean = np.load(String.Format("{0}\\spec_c.npy", start_up_path));
            bound = np.load(String.Format("{0}\\spec_d.npy", start_up_path));
            md = np.load(String.Format("{0}\\spec_e.npy", start_up_path));

            TransM = np.dot(M.T, pca_eigenvectors);

            TransM = TransM[":,::40"];
            pca_mean = pca_mean["::40"];

            return true;
        }

        public void TestNBI(Bitmap img24)
        {
            Single[] img_data = PreprocessTestImage(img24);


      

            NDarray src_data = null;
            NDarray tar_sepc = null;
            Double[] spec_data_d = null;

            src_data = np.array(img_data).reshape(img_data.Length / 3, 3);

            System.DateTime start_time = System.DateTime.Now;
            tar_sepc = Transfer1D(src_data).reshape(-1);
            Double sec = (System.DateTime.Now - start_time).TotalSeconds;


            spec_data_d = tar_sepc.GetData<Double>();


            Console.WriteLine("cost time {0} sec", sec);
        }

        public NDarray Transfer1D(NDarray src_data)
        {
            var ext = get_extend_color(src_data).T;

            var tar_sepc = np.dot(ext, TransM) + pca_mean;

            //tar_sepc = (tar_sepc - TransMin) / (TransMax - TransMin);

            return tar_sepc;
            //return tar_sepc[":,::40"];
        }

        public Single[] Transfer(Bitmap img24)
        {
            Single[] img_data = PreprocessTestImage(img24);

            NDarray src_data = null;
            NDarray tar_sepc = null;
            Double[] spec_data_d = null;

            IntPtr thd_ptr = PythonEngine.BeginAllowThreads();

            Task.Run(() =>
            {
                // when running on different threads you must lock!
                using (Py.GIL())
                {
                    src_data = np.array(img_data).reshape(img_data.Length / 3, 3);

                    tar_sepc = Transfer1D(src_data).reshape(-1);

                    spec_data_d = tar_sepc.GetData<Double>();
                }
            }).Wait();

            PythonEngine.EndAllowThreads(thd_ptr);

            Single[] spec_data_f = new Single[spec_data_d.Length];

//            for (int i = 0; i < spec_data_f.Length; i++)
//            {
//                spec_data_f[i] = (Single)spec_data_d[i];
//            }

            Parallel.For(0, spec_data_f.Length, i =>
            {
                spec_data_f[i] = (Single)spec_data_d[i];
            });

            return spec_data_f;
        }

        private Single[] PreprocessTestImage(Bitmap img24)
        {
            BitmapData bmp_data = img24.LockBits(new Rectangle(0, 0, img24.Width, img24.Height), ImageLockMode.ReadOnly, img24.PixelFormat);
            Byte[] img_data = new Byte[bmp_data.Stride * bmp_data.Height];
            Marshal.Copy(bmp_data.Scan0, img_data, 0, img_data.Length);

            var result = new Single[img24.Height * img24.Width * 3];

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

                    result[tar_index] = (Single)b;
                    result[tar_index + 1] = (Single)g;
                    result[tar_index + 2] = (Single)r;
                }
            }

            img24.UnlockBits(bmp_data);

            return result;
        }



        private NDarray get_extend_color(NDarray src_rgb)
        {
            NDarray extend = null;

            extend = np.vstack(src_rgb[":, 0"], src_rgb[":, 1"], src_rgb[":, 2"], src_rgb[":, 0"] * src_rgb[":, 0"], src_rgb[":, 1"] * src_rgb[":, 1"], src_rgb[":, 2"] * src_rgb[":, 2"]);

            return extend;
        }





        //def get_extend_color(self, src_rgb):


        //extend = None

        //if self.extend_mode == 0:
        //    extend = np.array([src_rgb[:, 0], src_rgb[:, 1], src_rgb[:, 2], np.power(src_rgb[:, 0], 2), np.power(src_rgb[:, 1], 2), np.power(src_rgb[:, 2], 2)])
        //elif self.extend_mode == 1:
        //    extend = np.array([src_rgb[:, 0], src_rgb[:, 1], src_rgb[:, 2], np.power(src_rgb[:, 0], 2), np.power(src_rgb[:, 1], 2), np.power(src_rgb[:, 2], 2), src_rgb[:, 0] * src_rgb[:, 1], src_rgb[:, 0] * src_rgb[:, 2], src_rgb[:, 1] * src_rgb[:, 2]])
        //elif self.extend_mode == 2:
        //    extend = np.array([src_rgb[:, 0], src_rgb[:, 1], src_rgb[:, 2], np.power(src_rgb[:, 0], 2), np.power(src_rgb[:, 1], 2), np.power(src_rgb[:, 2], 2), src_rgb[:, 0] * src_rgb[:, 1], src_rgb[:, 0] * src_rgb[:, 2], src_rgb[:, 1] * src_rgb[:, 2], np.power(src_rgb[:, 0], 3), np.power(src_rgb[:, 1], 3), np.power(src_rgb[:, 2], 3)])

        //return extend


    }
}
