using Numpy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HSEnvPredict
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private hsEnvPredictor m_predictor = new hsEnvPredictor();
        private String m_now_image_file;
        private List<hsEnvRegionInfo> m_reg_info = new List<hsEnvRegionInfo>();

        private hsHSITransfer m_hsi_transfer = new hsHSITransfer();

        private System.DateTime m_start_time;

        private Color Rainbow(float progress)
        {
            progress = progress * 0.7f + 0.1f;
            //progress = progress * 0.5f + 0.25f;

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
                default: // case 5:
                    return Color.FromArgb(255, 255, 0, descending);
            }
        }

        private void RefreshPBImage()
        {
            PBImage.PaintItems.Clear();

            Int32 disp_id = CBDispInfo.SelectedIndex;
            Int32 reg_id = CBRegion.SelectedIndex;

            if (CBIsMask.Checked)
            {
                foreach (hsEnvRegionInfo reg in m_reg_info)
                {
                    Single pred_value = 0;

                    if (disp_id == 0)
                    {
                        pred_value = (Single)reg.ResultClassIndex / (Single)m_predictor.RegionClassNum;
                    }
                    else
                    {
                        pred_value = reg.ResultReg;
                    }

                    Color c = Color.FromArgb((Int32)NUDImageAlpha.Value, Rainbow(pred_value));
                    Rectangle roi = reg.Roi;

                    Boolean is_show = false;
                    if (reg_id == m_predictor.RegionClassNum)
                    {
                        is_show = true;
                    }
                    else
                    {
                        is_show = reg.ResultClassIndex == reg_id;
                    }

                    if (disp_id == 0)
                        is_show = true;

                    if (is_show)
                        PBImage.PaintItems.Add(new mjControls.PaintRectangleF(roi, c));
                }
            }

            PBImage.Invalidate();
        }

        private void RefreshPBRainbow()
        {
            PBRainbow.PaintItems.Clear();

            Int32 y_size = 60;

            Rectangle tar_region = new Rectangle(0, 0, 100, m_predictor.PredClassNum * y_size);
            tar_region.Inflate(20, 20);

            for (Int32 i = 0; i < m_predictor.PredClassNum; i++)
            {
                Rectangle color_reg = new Rectangle(0, i * y_size, 100, y_size);
                Single color_value = (Single)i / (Single)m_predictor.PredClassNum;

                PBRainbow.PaintItems.Add(new mjControls.PaintRectangle(color_reg, Color.FromArgb(255, Rainbow(color_value))));
                PBRainbow.PaintItems.Add(new mjControls.PaintString(i.ToString(), "微軟正黑體", Color.White, 18.0f, FontStyle.Regular, StringAlignment.Center, StringAlignment.Near, color_reg));
            }

            //PBRainbow.PaintItems.Add(new mjControls.PaintRectangle(tar_region, Color.White, 1.0f));

            PBRainbow.BestFit();

            PBRainbow.Invalidate();
        }

        private void RefreshPBRainbow0()
        {
            PBRainbow.PaintItems.Clear();

            Int32 steps = 32;
            Int32 w = 12;

            Rectangle tar_region = new Rectangle(0, 0, 100, steps * 2);
            tar_region.Inflate(20, 20);

            for (Int32 i = 0; i < steps + 1; i++)
            {
                Rectangle color_reg = new Rectangle(0, i * 2, w, 2);
                Single color_value = 1.0f - (Single)i / (Single)steps;

                PBRainbow.PaintItems.Add(new mjControls.PaintRectangleF(color_reg, Color.FromArgb(255, Rainbow(color_value))));
            }

            //PBRainbow.PaintItems.Add(new mjControls.PaintRectangle(tar_region, Color.White, 1.0f));

            Rectangle str_roi0 = new Rectangle(-w, steps * 2, w, 0);
            Rectangle str_roi1 = new Rectangle(-w, 0, w, 0);
            str_roi0.Inflate(0, 6);
            str_roi1.Inflate(0, 6);

            PBRainbow.PaintItems.Add(new mjControls.PaintString("0.0", "微軟正黑體", Color.White, 18.0f, FontStyle.Regular, StringAlignment.Center, StringAlignment.Near, str_roi0));
            PBRainbow.PaintItems.Add(new mjControls.PaintString("1.0", "微軟正黑體", Color.White, 18.0f, FontStyle.Regular, StringAlignment.Center, StringAlignment.Near, str_roi1));

            PBRainbow.BestFit();

            PBRainbow.Invalidate();
        }

        private void OnProgressChanged(object sender, EventArgs e)
        {
            BWorkerAnalysis.ReportProgress((Int32)(m_predictor.NowProgress * 100.0));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String pred_region_md = "E:\\KK\\計畫\\SP7_1\\For SP1\\HSEnvPredict - 1.0.0.2\\model_VGG_2D.onnx";
            Int32 pred_region_num = 4;

            String pred_info_md = "E:\\KK\\計畫\\SP7_1\\For SP1\\HSEnvPredict - 1.0.0.2\\model_VGG_3D_reg.onnx";

            m_hsi_transfer.Initial(Application.StartupPath);

            m_predictor.InitialPred(pred_info_md, -1, m_hsi_transfer);
            m_predictor.InitailRegion(pred_region_md, pred_region_num);

            m_predictor.ProgressChanged += new System.EventHandler(this.OnProgressChanged);

            PBImage.ShowGrid = false;
            PBImage.ColorAlpha = 1.0;

            for (Int32 i = 0; i < pred_region_num; i++)
            {
                CBRegion.Items.Add("REG-" + i);
            }

            CBRegion.Items.Add("ALL");
            CBRegion.SelectedIndex = CBRegion.Items.Count - 1;

            CBDispInfo.SelectedIndex = 0;

            //Bitmap img = hsEnvPredictor.LoadImage("C:\\Users\\Tony\\Desktop\\TEST\\NBI\\640-480.jpg");

            //m_hsi_transfer.TestNBI(img);
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void BTNLoad_Click(object sender, EventArgs e)
        {
            if (OFDialog.ShowDialog() == DialogResult.OK)
            {
                m_now_image_file = OFDialog.FileName;

                m_reg_info.Clear();

                PBImage.Image = hsEnvPredictor.LoadImage(m_now_image_file);

                RefreshPBImage();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            RefreshPBRainbow0();
        }

        private void BTNAnalysis_Click(object sender, EventArgs e)
        {
            m_reg_info.Clear();

            RefreshPBImage();

            BTNLoad.Enabled = false;
            BTNAnalysis.Enabled = false;

            TSLLPredictProgress.Text = "";

            m_start_time = System.DateTime.Now;

            BWorkerAnalysis.RunWorkerAsync();
        }

        private void BWorkerAnalysis_DoWork(object sender, DoWorkEventArgs e)
        {
            hsEnvRegionInfo[] reg_info = m_predictor.Predict(m_now_image_file);

            e.Result = reg_info;
        }

        private void BWorkerAnalysis_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            TSLLPredictProgress.Text = String.Format("{0}%", e.ProgressPercentage);
        }

        private void BWorkerAnalysis_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Double sec = (System.DateTime.Now - m_start_time).TotalSeconds;

            hsEnvRegionInfo[] reg_info = e.Result as hsEnvRegionInfo[];

            m_reg_info.Clear();
            m_reg_info.AddRange(reg_info);

            RefreshPBImage();

            BTNLoad.Enabled = true;
            BTNAnalysis.Enabled = true;

            TSLLPredictProgress.Text = String.Format("{0:0.00}sec", sec);
        }

        private void PBImage_MouseMove(object sender, MouseEventArgs e)
        {
            Point img_pt = PBImage.GetImagePointFromPB(e.Location);
            Color pt_c = PBImage.GetPixelColor(img_pt);

            String msg = String.Format("(X:{0}, Y:{1}) | (R:{2}, G:{3}, B:{4})", img_pt.X, img_pt.Y, pt_c.R, pt_c.G, pt_c.B);

            TSLLImageInfo.Text = msg;
        }

        private void NUDImageAlpha_ValueChanged(object sender, EventArgs e)
        {
            RefreshPBImage();
        }

        private void CBIsMask_CheckedChanged(object sender, EventArgs e)
        {
            RefreshPBImage();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            PBImage.BestFit();
            PBRainbow.BestFit();
        }

        private void CBDispInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshPBImage();
        }

        private void CBRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshPBImage();
        }


    }
}
