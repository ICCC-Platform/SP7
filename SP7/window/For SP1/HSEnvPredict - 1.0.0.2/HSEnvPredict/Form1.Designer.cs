namespace HSEnvPredict
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.BTNLoad = new System.Windows.Forms.Button();
            this.BTNAnalysis = new System.Windows.Forms.Button();
            this.BWorkerAnalysis = new System.ComponentModel.BackgroundWorker();
            this.OFDialog = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.CBRegion = new System.Windows.Forms.ComboBox();
            this.CBIsMask = new System.Windows.Forms.CheckBox();
            this.PBRainbow = new mjControls.GridPictureBox();
            this.NUDImageAlpha = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PBImage = new mjControls.GridPictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.TSLLImageInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.TSLLPredictProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.CBDispInfo = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUDImageAlpha)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BTNLoad
            // 
            this.BTNLoad.Location = new System.Drawing.Point(9, 9);
            this.BTNLoad.Name = "BTNLoad";
            this.BTNLoad.Size = new System.Drawing.Size(116, 45);
            this.BTNLoad.TabIndex = 1;
            this.BTNLoad.Text = "載入影像";
            this.BTNLoad.UseVisualStyleBackColor = true;
            this.BTNLoad.Click += new System.EventHandler(this.BTNLoad_Click);
            // 
            // BTNAnalysis
            // 
            this.BTNAnalysis.Location = new System.Drawing.Point(9, 60);
            this.BTNAnalysis.Name = "BTNAnalysis";
            this.BTNAnalysis.Size = new System.Drawing.Size(116, 45);
            this.BTNAnalysis.TabIndex = 2;
            this.BTNAnalysis.Text = "分析";
            this.BTNAnalysis.UseVisualStyleBackColor = true;
            this.BTNAnalysis.Click += new System.EventHandler(this.BTNAnalysis_Click);
            // 
            // BWorkerAnalysis
            // 
            this.BWorkerAnalysis.WorkerReportsProgress = true;
            this.BWorkerAnalysis.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BWorkerAnalysis_DoWork);
            this.BWorkerAnalysis.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BWorkerAnalysis_ProgressChanged);
            this.BWorkerAnalysis.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BWorkerAnalysis_RunWorkerCompleted);
            // 
            // OFDialog
            // 
            this.OFDialog.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.CBDispInfo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.CBRegion);
            this.panel1.Controls.Add(this.CBIsMask);
            this.panel1.Controls.Add(this.PBRainbow);
            this.panel1.Controls.Add(this.NUDImageAlpha);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.BTNLoad);
            this.panel1.Controls.Add(this.BTNAnalysis);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(194, 576);
            this.panel1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(11, 220);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 29);
            this.label2.TabIndex = 8;
            this.label2.Text = "區域";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CBRegion
            // 
            this.CBRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBRegion.FormattingEnabled = true;
            this.CBRegion.Location = new System.Drawing.Point(86, 220);
            this.CBRegion.Name = "CBRegion";
            this.CBRegion.Size = new System.Drawing.Size(83, 28);
            this.CBRegion.TabIndex = 7;
            this.CBRegion.SelectedIndexChanged += new System.EventHandler(this.CBRegion_SelectedIndexChanged);
            // 
            // CBIsMask
            // 
            this.CBIsMask.Appearance = System.Windows.Forms.Appearance.Button;
            this.CBIsMask.AutoSize = true;
            this.CBIsMask.Checked = true;
            this.CBIsMask.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBIsMask.Location = new System.Drawing.Point(9, 121);
            this.CBIsMask.Name = "CBIsMask";
            this.CBIsMask.Size = new System.Drawing.Size(83, 30);
            this.CBIsMask.TabIndex = 6;
            this.CBIsMask.Text = "顯示遮罩";
            this.CBIsMask.UseVisualStyleBackColor = true;
            this.CBIsMask.CheckedChanged += new System.EventHandler(this.CBIsMask_CheckedChanged);
            // 
            // PBRainbow
            // 
            this.PBRainbow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.PBRainbow.AutoZoom = true;
            this.PBRainbow.CenterPoint = new System.Drawing.Point(-1073741824, -1073741824);
            this.PBRainbow.ColorAlpha = 0.1D;
            this.PBRainbow.EnableDoubleClickFit = true;
            this.PBRainbow.EnableMove = true;
            this.PBRainbow.EnableShortCut = false;
            this.PBRainbow.EnableZoom = true;
            this.PBRainbow.FixGrid = true;
            this.PBRainbow.ForeColor = System.Drawing.Color.White;
            this.PBRainbow.GridColor = System.Drawing.Color.White;
            this.PBRainbow.GridSize = 100;
            this.PBRainbow.Image = null;
            this.PBRainbow.Location = new System.Drawing.Point(15, 253);
            this.PBRainbow.Name = "PBRainbow";
            this.PBRainbow.Offset = new System.Drawing.Point(0, 0);
            this.PBRainbow.Scale = 0D;
            this.PBRainbow.ShowGrid = false;
            this.PBRainbow.Size = new System.Drawing.Size(164, 303);
            this.PBRainbow.TabIndex = 5;
            this.PBRainbow.UseImagePalette = true;
            // 
            // NUDImageAlpha
            // 
            this.NUDImageAlpha.Location = new System.Drawing.Point(86, 158);
            this.NUDImageAlpha.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NUDImageAlpha.Name = "NUDImageAlpha";
            this.NUDImageAlpha.Size = new System.Drawing.Size(89, 29);
            this.NUDImageAlpha.TabIndex = 4;
            this.NUDImageAlpha.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.NUDImageAlpha.ValueChanged += new System.EventHandler(this.NUDImageAlpha_ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 29);
            this.label1.TabIndex = 3;
            this.label1.Text = "透明度";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.PBImage, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(793, 582);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // PBImage
            // 
            this.PBImage.AutoZoom = true;
            this.PBImage.CenterPoint = new System.Drawing.Point(-1073741824, -1073741824);
            this.PBImage.ColorAlpha = 0.1D;
            this.PBImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PBImage.EnableDoubleClickFit = true;
            this.PBImage.EnableMove = true;
            this.PBImage.EnableShortCut = false;
            this.PBImage.EnableZoom = true;
            this.PBImage.FixGrid = true;
            this.PBImage.ForeColor = System.Drawing.Color.White;
            this.PBImage.GridColor = System.Drawing.Color.White;
            this.PBImage.GridSize = 100;
            this.PBImage.Image = null;
            this.PBImage.Location = new System.Drawing.Point(203, 3);
            this.PBImage.Name = "PBImage";
            this.PBImage.Offset = new System.Drawing.Point(0, 0);
            this.PBImage.Scale = 0D;
            this.PBImage.ShowGrid = false;
            this.PBImage.Size = new System.Drawing.Size(587, 576);
            this.PBImage.TabIndex = 0;
            this.PBImage.UseImagePalette = true;
            this.PBImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PBImage_MouseMove);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSLLImageInfo,
            this.TSLLPredictProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 582);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(793, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // TSLLImageInfo
            // 
            this.TSLLImageInfo.Name = "TSLLImageInfo";
            this.TSLLImageInfo.Size = new System.Drawing.Size(0, 17);
            // 
            // TSLLPredictProgress
            // 
            this.TSLLPredictProgress.Name = "TSLLPredictProgress";
            this.TSLLPredictProgress.Size = new System.Drawing.Size(0, 17);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(11, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 29);
            this.label3.TabIndex = 10;
            this.label3.Text = "顯示資訊";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CBDispInfo
            // 
            this.CBDispInfo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBDispInfo.FormattingEnabled = true;
            this.CBDispInfo.Items.AddRange(new object[] {
            "分區",
            "汙染預測"});
            this.CBDispInfo.Location = new System.Drawing.Point(86, 189);
            this.CBDispInfo.Name = "CBDispInfo";
            this.CBDispInfo.Size = new System.Drawing.Size(83, 28);
            this.CBDispInfo.TabIndex = 9;
            this.CBDispInfo.SelectedIndexChanged += new System.EventHandler(this.CBDispInfo_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(793, 604);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "環境影像偵測工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUDImageAlpha)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private mjControls.GridPictureBox PBImage;
        private System.Windows.Forms.Button BTNLoad;
        private System.Windows.Forms.Button BTNAnalysis;
        private System.ComponentModel.BackgroundWorker BWorkerAnalysis;
        private System.Windows.Forms.OpenFileDialog OFDialog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel TSLLImageInfo;
        private System.Windows.Forms.NumericUpDown NUDImageAlpha;
        private System.Windows.Forms.Label label1;
        private mjControls.GridPictureBox PBRainbow;
        private System.Windows.Forms.CheckBox CBIsMask;
        private System.Windows.Forms.ToolStripStatusLabel TSLLPredictProgress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CBRegion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CBDispInfo;
    }
}

