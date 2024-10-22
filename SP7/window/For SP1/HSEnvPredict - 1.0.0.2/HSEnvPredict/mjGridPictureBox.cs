using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Linq;

namespace mjControls
{
    [ToolboxBitmap(typeof(System.Windows.Forms.PictureBox)),
        ToolboxItem(true),
        ToolboxItemFilter("System.Windows.Forms"),
        Description("MJ PICTUREBOX")]
    public class GridPictureBox : System.Windows.Forms.UserControl
    {
        public GridPictureBox()
        {
            InitializeComponent();

            InitialData();
        }

        ~GridPictureBox()
        {
            if (m_client_image != null)
                m_client_image.Dispose();

            if (m_show_image != null)
                m_show_image.Dispose();

            if (m_image != null)
                m_image.Dispose();

            if (m_items != null)
            {
                m_items.Clear();
                m_items = null;
            }

            if (components != null)
                m_client_image.Dispose();
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components;

#pragma region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
        }
#pragma endregion

        private const Double m_scale_limit = 100.0;

        private Color m_back_color;

        private Bitmap m_image;

        private Double m_scale;
        private Point m_offset;

        private Boolean m_draw_grid;
        private Color m_grid_color;
        private Double m_grid_alpha;
        private Int32 m_grid_size;
        private Boolean m_fix_grid;

        private Boolean m_enable_zoom;
        private Boolean m_enable_move;
        private Boolean m_auto_zoom;

        private Boolean m_enable_double_click_fit;

        private Boolean m_enable_short_cut;

        private Boolean m_use_image_palette;

        private PaintItemCollection m_items;
        private PaintItemCollection m_cache_items;

        // 內部顯示元件
        private Bitmap m_show_image;
        private Bitmap m_client_image;

        // 內部狀態參數
        private Point m_move_start_pt;
        private Point m_move_start_offset;
        private Boolean m_moving;
        private Point m_back_offset;
        private Double m_back_scale;
        private Boolean m_absolute_update;
        private Boolean m_busy;


        [Category("MJ"),
            Description("Background Color"),
            RefreshProperties(RefreshProperties.Repaint)]
        public Color BackColor
        {
            get { return m_back_color; }
            set { m_back_color = value; }
        }

        [Category("MJ"),
            Description("Image Offset"),
            RefreshProperties(RefreshProperties.Repaint)]
        public Point Offset
        {
            get { return m_offset; }
            set { m_offset = value; this.Invalidate(); }
        }

        [Category("MJ"),
            Description("Image Scale"),
            RefreshProperties(RefreshProperties.Repaint)]
        public Double Scale
        {
            get { return m_scale; }
            set
            {
                if (m_scale != value)
                {
                    m_scale = value;

                    if (m_scale > m_scale_limit)
                        m_scale = m_scale_limit;

                    this.Invalidate();
                    OnScaleChanged(new EventArgs());
                }
            }
        }

        [Category("MJ"),
            Description("Image"),
            RefreshProperties(RefreshProperties.Repaint)]
        public Bitmap Image
        {
            get { return m_image; }
            set
            {
                Boolean scale_change = false;

                m_image = value;

                if (m_image != null && m_auto_zoom)
                {
                    Double scale = GetBestFitScale();
                    scale_change = System.Math.Abs(scale - m_scale) > 0.01;
                    m_scale = scale;

                    if (m_scale > m_scale_limit)
                        m_scale = m_scale_limit;

                    m_offset = GetShowOffset(new Point(m_image.Width / 2, m_image.Height / 2), new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2), m_scale);
                }

                m_absolute_update = true;
                this.Invalidate();

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                if (scale_change)
                    OnScaleChanged(new EventArgs());
            }
        }

        [Category("MJ"),
            Description("Draw grid line"),
            RefreshProperties(RefreshProperties.Repaint)]
        public Boolean ShowGrid
        {
            get { return m_draw_grid; }
            set
            {
                m_draw_grid = value;
                m_absolute_update = true;
                this.Invalidate();
            }
        }

        [Category("MJ"),
            Description("Grid line color"),
            RefreshProperties(RefreshProperties.Repaint)]
        public Color GridColor
        {
            get { return m_grid_color; }
            set
            {
                m_grid_color = value;
                m_absolute_update = true;
                this.Invalidate();
            }
        }

        [Category("MJ"),
            Description("Grid line alpha"),
            RefreshProperties(RefreshProperties.Repaint)]
        public Double ColorAlpha
        {
            get { return m_grid_alpha; }
            set
            {
                m_grid_alpha = value > 1.0 ? 1.0 : value;
                m_absolute_update = true;
                this.Invalidate();
            }
        }

        [Category("MJ"),
            Description("Grid size"),
            RefreshProperties(RefreshProperties.Repaint)]
        public Int32 GridSize
        {
            get { return m_grid_size; }
            set
            {
                m_grid_size = value < 1 ? 1 : value;
                m_absolute_update = true;
                this.Invalidate();
            }
        }

        [Category("MJ"),
            Description("Fix Grid"),
            RefreshProperties(RefreshProperties.Repaint)]
        public Boolean FixGrid
        {
            get { return m_fix_grid; }
            set
            {
                m_fix_grid = value;
                m_absolute_update = true;
                this.Invalidate();
            }
        }

        [Category("MJ"),
            Description("Enable move image"),
            RefreshProperties(RefreshProperties.Repaint)]
        public Boolean EnableMove
        {
            get { return m_enable_move; }
            set { m_enable_move = value; }
        }

        [Category("MJ"),
            Description("Auto scale image when image has been changed"),
            RefreshProperties(RefreshProperties.Repaint)]
        public Boolean AutoZoom
        {
            get { return m_auto_zoom; }
            set { m_auto_zoom = value; }
        }

        [Category("MJ"),
            Description("Enable zoom image"),
            RefreshProperties(RefreshProperties.Repaint)]
        public Boolean EnableZoom
        {
            get { return m_enable_zoom; }
            set { m_enable_zoom = value; }
        }

        [Category("MJ"),
    Description("Enable Double Click Fit"),
    RefreshProperties(RefreshProperties.Repaint)]
        public Boolean EnableDoubleClickFit
        {
            get { return m_enable_double_click_fit; }
            set { m_enable_double_click_fit = value; }
        }

        [Category("MJ"),
            Description("Enable Short Cut"),
            RefreshProperties(RefreshProperties.Repaint)]
        public Boolean EnableShortCut
        {
            get { return m_enable_short_cut; }
            set { m_enable_short_cut = value; }
        }

        [Category("MJ"),
            Description("Paint Items"),
            RefreshProperties(RefreshProperties.Repaint)]
        public PaintItemCollection PaintItems
        {
            get { return m_items; }
        }

        [Category("MJ"),
            Description("Paint Items"),
            RefreshProperties(RefreshProperties.Repaint)]
        public PaintItemCollection CachePaintItems
        {
            get { return m_cache_items; }
        }

        [Category("MJ"),
            Description("Image Center Point"),
            RefreshProperties(RefreshProperties.Repaint)]
        public Point CenterPoint
        {
            get
            {
                Rectangle roi = GetShowRect();
                return new Point(roi.Width / 2 + roi.X, roi.Height / 2 + roi.Y);
            }
            set { MoveToImagePoint(value); }
        }

        [Category("MJ"),
            Description("Image Show ROI"),
            RefreshProperties(RefreshProperties.Repaint)]
        public Rectangle ShowImageROI
        {
            get { return GetShowRect(); }
        }

        [Category("MJ"),
            Description("Use Image Palette"),
            RefreshProperties(RefreshProperties.Repaint)]
        public Boolean UseImagePalette
        {
            get { return m_use_image_palette; }
            set { m_use_image_palette = value; }
        }

        [Category("MJ"),
            Description("Scale Changed")]
        public event EventHandler ScaleChanged;

        [Category("MJ"),
            Description("Offset Changed")]
        public event EventHandler OffsetChanged;

        private void InitialData()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, false);

            m_items = new PaintItemCollection();
            m_cache_items = new PaintItemCollection();
            m_client_image = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            m_image = null;
            m_draw_grid = false;

            m_offset = new Point(0, 0);
            m_absolute_update = false;

            m_back_color = Color.Black;
            this.ForeColor = Color.White;

            m_grid_alpha = 0.1;
            m_grid_color = Color.White;
            m_grid_size = 100;
            m_fix_grid = true;

            m_moving = false;
            m_enable_move = true;
            m_enable_zoom = true;
            m_auto_zoom = true;
            m_enable_short_cut = false;

            m_enable_double_click_fit = true;

            m_busy = false;
            m_use_image_palette = true;
        }

        private Boolean OnShortCut(Keys key)
        {
            if (!m_enable_short_cut)
                return false;

            if (!m_enable_move)
                return false;

            Boolean handled = true;

            Int32 step = System.Math.Max(1, (Int32)(1 / m_scale + 0.5));

            switch (key)
            {
                case Keys.Right:
                    m_offset.X += step;
                    break;

                case Keys.Left:
                    m_offset.X -= step;
                    break;

                case Keys.Up:
                    m_offset.Y -= step;
                    break;

                case Keys.Down:
                    m_offset.Y += step;
                    break;

                case Keys.W:
                    ZoomIn(GetImagePointFromPB(new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2)), new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2));
                    break;

                case Keys.Q:
                    ZoomOut(GetImagePointFromPB(new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2)), new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2));
                    break;

                default:
                    handled = false;
                    break;
            }

            this.Invalidate();

            return handled;
        }

        private Int32 GetBytePerPixel(System.Drawing.Imaging.PixelFormat format)
        {
            switch (format)
            {
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                    return 4;

                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    return 3;

                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    return 1;

                default:
                    return 0;
            }
        }

        private Rectangle GetLockRect(Point offset, Double scale, Bitmap img)
        {
            Rectangle show_rect = new Rectangle();

            show_rect.X = -offset.X;
            show_rect.Y = -offset.Y;

            // 強制進位，多Buffer一些空間
            show_rect.Width = (Int32)(this.ClientSize.Width / scale + 1.0);
            show_rect.Height = (Int32)(this.ClientSize.Height / scale + 1.0);

            if (img != null)
            {
                Rectangle img_rect = new Rectangle(0, 0, img.Width, img.Height);
                show_rect = Rectangle.Intersect(show_rect, img_rect);
            }

            return show_rect;
        }

        private Point GetShowOffset(Point img_pt, Point show_pt, Double scale)
        {
            Point offset = new Point();

            offset.X = (Int32)(show_pt.X / scale - img_pt.X);
            offset.Y = (Int32)(show_pt.Y / scale - img_pt.Y);

            return offset;
        }

        private Double GetBestFitScale()
        {
            if (m_image != null)
            {
                Double scale_x = this.ClientSize.Width / (Double)m_image.Width;
                Double scale_y = this.ClientSize.Height / (Double)m_image.Height;

                return ((scale_x > scale_y) ? scale_y : scale_x);
            }

            return 1.0;
        }

        private void DrawImageToBuffer()
        {
            if (m_show_image != null)
            {
                m_show_image.Dispose();
                m_show_image = null;
            }

            if (m_image == null)
                return;

            Boolean is_1bpp = (m_image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed);

            if (is_1bpp)
            {
                m_show_image = new Bitmap(this.ClientSize.Width, this.ClientSize.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            }
            else
            {
                m_show_image = new Bitmap(this.ClientSize.Width, this.ClientSize.Height, m_image.PixelFormat);
            }

            Rectangle from_lock_rect = GetLockRect(m_offset, m_scale, m_image);

            if (from_lock_rect.Width == 0 || from_lock_rect.Height == 0)
                return;

            Int32 from_bytes_per_pixel = GetBytePerPixel(m_image.PixelFormat);
            Int32 dest_bytes_per_pixel = GetBytePerPixel(m_show_image.PixelFormat);

            if (dest_bytes_per_pixel == 0)
                return;

            // Get raw data
            System.Drawing.Imaging.BitmapData from_bitmap_data = m_image.LockBits(from_lock_rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, m_image.PixelFormat);
            System.Drawing.Imaging.BitmapData dest_bitmap_data = m_show_image.LockBits(new Rectangle(0, 0, m_show_image.Width, m_show_image.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, m_show_image.PixelFormat);

            Byte[] from_img_data = new Byte[from_bitmap_data.Height * from_bitmap_data.Stride - from_bitmap_data.Stride + from_bitmap_data.Width * from_bytes_per_pixel];
            Byte[] dest_img_data = new Byte[dest_bitmap_data.Height * dest_bitmap_data.Stride - dest_bitmap_data.Stride + dest_bitmap_data.Width * dest_bytes_per_pixel];
            //Byte[] from_img_data = new Byte[from_bitmap_data.Height * from_bitmap_data.Stride];
            //Byte[] dest_img_data = new Byte[dest_bitmap_data.Height * dest_bitmap_data.Stride];

            Marshal.Copy(from_bitmap_data.Scan0, from_img_data, 0, from_img_data.Length);
            Marshal.Copy(dest_bitmap_data.Scan0, dest_img_data, 0, dest_img_data.Length);

            Int32 from_pitch = from_bitmap_data.Stride;
            Int32 dest_pitch = dest_bitmap_data.Stride;

            Int32 dest_width = m_show_image.Width;
            Int32 dest_height = m_show_image.Height;

            Int32 from_line_index = 0;
            Int32 dest_line_index = 0;
            Int32 from_now_index = 0;
            Int32 dest_now_index = 0;

            Int32 offset_h = System.Math.Max((Int32)(m_offset.Y * m_scale + 0.5), 0);

            for (Int32 h = offset_h; h < dest_height; h++)
            {
                // 反算來源影像y值
                Int32 form_h = (Int32)((h - offset_h) / m_scale);

                if (form_h >= from_lock_rect.Height)
                    break;

                from_line_index = from_pitch * form_h;
                dest_line_index = dest_pitch * h;
                from_now_index = from_line_index;
                dest_now_index = dest_line_index;

                Int32 offset_w = System.Math.Max((Int32)(m_offset.X * m_scale + 0.5), 0);

                dest_now_index += (offset_w * dest_bytes_per_pixel);

                for (Int32 w = offset_w; w < dest_width; w++)
                {
                    Int32 form_w = (Int32)((w - offset_w) / m_scale);

                    if (form_w >= from_lock_rect.Width)
                        break;

                    if (is_1bpp)
                    {
                        from_now_index = from_line_index + (form_w >> 3);

                        if (((0x80 >> (form_w & 0x7)) & from_img_data[from_now_index]) > 0)
                        {
                            dest_img_data[from_now_index] = 255;
                        }
                        else
                        {
                            dest_img_data[from_now_index] = 0;
                        }

                        dest_now_index += dest_bytes_per_pixel;
                    }
                    else
                    {
                        from_now_index = from_line_index + form_w * from_bytes_per_pixel;

                        for (Int32 n = 0; n < from_bytes_per_pixel; n++)
                            dest_img_data[dest_now_index + n] = from_img_data[from_now_index + n];

                        dest_now_index += dest_bytes_per_pixel;
                    }
                }
            }

            Marshal.Copy(dest_img_data, 0, dest_bitmap_data.Scan0, dest_img_data.Length);

            m_image.UnlockBits(from_bitmap_data);
            m_show_image.UnlockBits(dest_bitmap_data);

            if (m_image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                m_show_image.Palette = m_image.Palette;
        }

        private void DrawGrid(Graphics g)
        {
            Rectangle rect = new Rectangle(new Point(0, 0), this.ClientSize);

            Point offset = new Point();

            if (m_fix_grid)
            {
                offset = new Point(0, 0);
            }
            else
            {
                offset.X = (Int32)(m_offset.X % m_grid_size * m_scale);
                offset.Y = (Int32)(m_offset.Y % m_grid_size * m_scale);
            }

            Int32 alpha1 = (Int32)(m_grid_alpha * 255);
            Int32 alpha2 = (Int32)(m_grid_alpha * 255 * 0.6);

            Pen pen1 = null;
            Pen pen2 = null;

            Int32 grid_size = (Int32)(m_grid_size * m_scale + 0.5);
            try
            {
                pen1 = new Pen(Color.FromArgb(alpha1, m_grid_color));
                pen2 = new Pen(Color.FromArgb(alpha2, m_grid_color));

                for (Int32 x = offset.X; x < rect.Width; x += grid_size)
                {
                    Boolean dash_line = (x / grid_size) % 2 == 0;

                    Pen pen = dash_line ? pen2 : pen1;

                    g.DrawLine(pen, new Point(x, 0), new Point(x, rect.Height));
                }

                for (Int32 y = offset.Y; y < rect.Height; y += grid_size)
                {
                    Boolean dash_line = (y / grid_size) % 2 == 0;

                    Pen pen = dash_line ? pen2 : pen1;

                    g.DrawLine(pen, new Point(0, y), new Point(rect.Width, y));
                }
            }
            catch
            {

            }

            finally
            {
                pen1.Dispose();
                pen2.Dispose();
            }
        }

        private void ZoomIn(Point ref_img_pt, Point show_client_pt)
        {
            Double old_scale_value = m_scale;

            m_scale *= 1.25;

            if (m_scale > m_scale_limit)
                m_scale = m_scale_limit;

            if (old_scale_value != m_scale)
            {
                m_offset = GetShowOffset(ref_img_pt, show_client_pt, m_scale);

                OnScaleChanged(new EventArgs());

                this.Invalidate();
            }
        }

        private void ZoomOut(Point ref_img_pt, Point show_client_pt)
        {
            Double old_scale_value = m_scale;

            m_scale /= 1.25;

            if (m_scale > m_scale_limit)
                m_scale = m_scale_limit;

            if (old_scale_value != m_scale)
            {
                m_offset = GetShowOffset(ref_img_pt, show_client_pt, m_scale);

                OnScaleChanged(new EventArgs());

                this.Invalidate();
            }
        }

        protected override Boolean ProcessCmdKey(ref Message m, Keys key)

        {
            // WM_KEYDOWN 0x0100
            // WM_SYSKEYDOWN 0x0104

            Boolean handled = false;

            if (m.Msg == 0x0100 || m.Msg == 0x0104)
                handled = OnShortCut(key);

            if (!handled)
                handled = base.ProcessCmdKey(ref m, key);

            return handled;
        }

        protected virtual void OnScaleChanged(EventArgs e)
        {
            if (ScaleChanged != null)
                ScaleChanged(this, e);
        }

        protected virtual void OnOffsetChanged(EventArgs e)
        {
            if (OffsetChanged != null)
                OffsetChanged(this, e);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            try
            {
                if (m_back_offset != m_offset || m_back_scale != m_scale || m_absolute_update)
                {
                    DrawImageToBuffer();
                }

                m_back_offset = m_offset;
                m_back_scale = m_scale;
                m_absolute_update = false;
            }
            catch (Exception ex)
            {
                String error_str = "Error: " + ex.Message;

                Font font = null;
                Brush brush = null;
                StringFormat format = null;

                try
                {
                    font = new Font(new FontFamily("Arial"), 16.0f);
                    brush = new SolidBrush(this.ForeColor);
                    format = new StringFormat(StringFormatFlags.NoClip);

                    e.Graphics.DrawString(error_str, font, brush, new Rectangle(new Point(0, 0), this.ClientSize), format);
                }
                finally
                {
                    font.Dispose();
                    brush.Dispose();
                    format.Dispose();
                }

                return;
            }

            Graphics buf_g = Graphics.FromImage(m_client_image);
            buf_g.Clear(m_back_color);
            buf_g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (m_show_image != null)
            {
                // 8 bit 要修正 gray level 色盤
                if (m_show_image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed && !m_use_image_palette)
                {
                    System.Drawing.Imaging.ColorPalette palette = m_show_image.Palette;

                    for (Int32 n = 0; n < 256; n++)
                        palette.Entries[n] = Color.FromArgb(n, n, n);

                    m_show_image.Palette = palette;
                }

                buf_g.DrawImageUnscaled(m_show_image, 0, 0);
            }

            Rectangle show_rect = GetShowRect();

            for (Int32 i = 0; i < m_items.Count; i++)
            {
                if (m_items[i].Contain(show_rect))
                    m_items[i].Paint(buf_g, m_scale, m_offset);
            }

            for (Int32 i = 0; i < m_cache_items.Count; i++)
            {
                if (m_cache_items[i].Contain(show_rect))
                    m_cache_items[i].Paint(buf_g, m_scale, m_offset);
            }

            if (m_draw_grid)
                DrawGrid(buf_g);

            buf_g.Dispose();

            Graphics g = e.Graphics;
            g.DrawImageUnscaled(m_client_image, new Point(0, 0));

            base.OnPaint(e);
        }

        protected override void OnClientSizeChanged(System.EventArgs e)
        {
            if (this.ClientSize.Width == 0 || this.ClientSize.Height == 0)
                return;

            if (m_client_image != null)
                m_client_image.Dispose();

            m_client_image = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

            m_absolute_update = true;
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (m_enable_move)
            {
                m_moving = true;
                m_move_start_pt = e.Location;
                m_move_start_offset = m_offset;
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            if (m_moving && !m_busy)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    m_busy = true;

                    m_offset = m_move_start_offset;
                    m_offset.X += (Int32)((e.Location.X - m_move_start_pt.X) / m_scale);
                    m_offset.Y += (Int32)((e.Location.Y - m_move_start_pt.Y) / m_scale);

                    this.Invalidate();

                    m_busy = false;
                }
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right && m_moving)
                OnOffsetChanged(new EventArgs());

            m_moving = false;

            base.OnMouseUp(e);
        }

        protected override void OnMouseClick(System.Windows.Forms.MouseEventArgs e)
        {
            if (m_enable_zoom && !m_busy)
            //if (m_enable_zoom && !m_busy && m_image != null)
            {
                m_busy = true;

                if (e.Button == System.Windows.Forms.MouseButtons.Middle)
                {
                    //m_scale = GetBestFitScale();

                    //if (m_scale > m_scale_limit)
                    //    m_scale = m_scale_limit;

                    //m_offset = GetShowOffset(new Point(m_image.Width / 2, m_image.Height / 2), new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2), m_scale);

                    //this.Invalidate();

                    BestFit();

                    OnScaleChanged(new EventArgs());
                }

                m_busy = false;
            }

            base.OnMouseClick(e);
        }

        protected override void OnMouseDoubleClick(System.Windows.Forms.MouseEventArgs e)
        {
            if (m_enable_zoom && m_enable_move && m_enable_double_click_fit && !m_busy && m_image != null)
            {
                m_busy = true;

                // Fit Scale 1, SetCenter
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Point client_pt = e.Location;
                    Point img_pt = GetImagePointFromPB(client_pt);

                    m_scale = 1.0;
                    m_offset = GetShowOffset(img_pt, client_pt, m_scale);

                    this.Invalidate();

                    OnScaleChanged(new EventArgs());
                }

                m_busy = false;
            }

            base.OnMouseDoubleClick(e);
        }

        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (m_enable_zoom && !m_busy)
            {
                m_busy = true;

                Point center_pt = GetImagePointFromPB(new Point(this.Width / 2, this.Height / 2));

                if (e.Delta > 0)
                    ZoomIn(GetImagePointFromPB(e.Location), e.Location);
                else if (e.Delta < 0)
                    ZoomOut(GetImagePointFromPB(e.Location), e.Location);

                m_busy = false;
            }
        }

        protected override void OnMouseEnter(System.EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            base.OnMouseLeave(e);
        }

        public void SetImage(Bitmap img)
        {
            m_image = img;
        }

        public void ClearPaintItems()
        {
            m_items.Clear();
        }

        public void AddPaintItem(PaintItem item)
        {
            m_items.Add(item);
        }

        public void ClearCacheItems()
        {
            m_cache_items.Clear();
        }

        public Point GetImagePointFromPB(Point pb_pt)
        {
            Point img_pt = new Point();
            img_pt.X = (Int32)(pb_pt.X / m_scale - m_offset.X);
            img_pt.Y = (Int32)(pb_pt.Y / m_scale - m_offset.Y);
            return img_pt;
        }

        public Point GetPBPointFromImage(Point img_pt)
        {
            Point pb_pt = new Point();
            pb_pt.X = (Int32)((img_pt.X + m_offset.X) * m_scale);
            pb_pt.Y = (Int32)((img_pt.Y + m_offset.Y) * m_scale);
            return pb_pt;
        }

        public void BestFit()
        {
            Double scale = 1.0;
            Point offset = new Point();

            if (m_image != null)
            {
                scale = GetBestFitScale();
                offset = GetShowOffset(new Point(m_image.Width / 2, m_image.Height / 2), new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2), scale);
            }
            else
            {
                Rectangle show_roi = new Rectangle();

                if (m_items.Count > 0)
                {
                    show_roi = m_items[0].Rect;
                }
                else if (m_cache_items.Count > 0)
                {
                    show_roi = m_cache_items[0].Rect;
                }

                foreach (PaintItem item in m_items)
                    show_roi = Rectangle.Union(show_roi, item.Rect);

                foreach (PaintItem item in m_cache_items)
                    show_roi = Rectangle.Union(show_roi, item.Rect);

                if (show_roi.Width > 0 && show_roi.Height > 0)
                {
                    Double scale_x = this.ClientSize.Width / (Double)show_roi.Width;
                    Double scale_y = this.ClientSize.Height / (Double)show_roi.Height;

                    scale = ((scale_x > scale_y) ? scale_y : scale_x);
                    offset = GetShowOffset(new Point(show_roi.X + show_roi.Width / 2, show_roi.Y + show_roi.Height / 2), new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2), scale);
                }
            }

            if (System.Math.Abs(m_scale - scale) > 0.000001 || (offset.X - m_offset.X != 0) || (offset.Y - m_offset.Y != 0))
            {
                m_scale = scale;

                if (m_scale > m_scale_limit)
                    m_scale = m_scale_limit;

                m_offset = offset;

                m_absolute_update = true;
                this.Invalidate();
            }
        }

        public void FitRoi(Rectangle show_roi)
        {
            if (show_roi.Width > 0 && show_roi.Height > 0)
            {
                Double scale_x = this.ClientSize.Width / (Double)show_roi.Width;
                Double scale_y = this.ClientSize.Height / (Double)show_roi.Height;

                m_scale = ((scale_x > scale_y) ? scale_y : scale_x);
                m_offset = GetShowOffset(new Point(show_roi.X + show_roi.Width / 2, show_roi.Y + show_roi.Height / 2), new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2), m_scale);
            }

            OnScaleChanged(new EventArgs());

            m_absolute_update = true;
            this.Invalidate();
        }

        public Rectangle GetShowRect()
        {
            Rectangle show_rect = new Rectangle();

            show_rect.Location = new Point(-m_offset.X, -m_offset.Y);
            show_rect.Width = (Int32)(this.ClientSize.Width / m_scale + 0.5);
            show_rect.Height = (Int32)(this.ClientSize.Height / m_scale + 0.5);

            return show_rect;
        }

        public void MoveToImagePoint(Point img_pt, Double scale)
        {
            Boolean sacle_changed = (m_scale != scale);

            m_scale = scale;

            m_offset = GetShowOffset(img_pt, new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2), m_scale);

            if (sacle_changed)
                OnScaleChanged(new EventArgs());

            this.Invalidate();
        }

        public void MoveToImagePoint(Point img_pt)
        {
            m_offset = GetShowOffset(img_pt, new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2), m_scale);
            this.Invalidate();
        }

        public Color GetPixelColor(Point query_pt)
        {
            if (m_image == null)
                return Color.Black;

            if (query_pt.X < 0 || query_pt.X >= m_image.Width ||
                query_pt.Y < 0 || query_pt.Y >= m_image.Height)
                return Color.Black;

            Color clr = new Color();

            switch (m_image.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                case System.Drawing.Imaging.PixelFormat.Format1bppIndexed:
                    clr = m_image.GetPixel(query_pt.X, query_pt.Y);
                    break;
            }

            return clr;
        }

        public Single[] GetRegionColor(Rectangle rect)
        {
            if (m_image == null)
                return new Single[0];

            Rectangle img_roi = new Rectangle(0, 0, m_image.Width, m_image.Height);
            Rectangle tar_roi = Rectangle.Intersect(img_roi, rect);

            if ((tar_roi.Width == 0) || (tar_roi.Height == 0))
                return new Single[0];

            Int32 bytes_per_pixel = GetBytePerPixel(m_image.PixelFormat);

            if (bytes_per_pixel == 0)
                return new Single[0];

            Single[] avg_color = new Single[bytes_per_pixel];
            for (Int32 i = 0; i < avg_color.Length; i++)
            {
                avg_color[i] = 0.0f;
            }

            System.Drawing.Imaging.BitmapData img_bitmap_data = m_image.LockBits(tar_roi, System.Drawing.Imaging.ImageLockMode.ReadOnly, m_image.PixelFormat);
            Int32 img_pitch = img_bitmap_data.Stride;

            Byte[] image_data = new Byte[img_bitmap_data.Height * img_pitch];
            Marshal.Copy(img_bitmap_data.Scan0, image_data, 0, image_data.Length);

            for (Int32 h = 0; h < tar_roi.Height; h++)
            {
                for (Int32 w = 0; w < tar_roi.Width; w++)
                {
                    Int32 now_index = h * img_pitch + w * bytes_per_pixel;

                    for (Int32 n = 0; n < bytes_per_pixel; n++)
                    {
                        avg_color[n] += (Single)image_data[now_index + n];
                    }
                }
            }

            Int32 pixel_count = tar_roi.Width * tar_roi.Height;

            for (Int32 n = 0; n < bytes_per_pixel; n++)
            {
                avg_color[n] = avg_color[n] / (Single)pixel_count;
            }

            m_image.UnlockBits(img_bitmap_data);

            return avg_color;
        }
    }
}


