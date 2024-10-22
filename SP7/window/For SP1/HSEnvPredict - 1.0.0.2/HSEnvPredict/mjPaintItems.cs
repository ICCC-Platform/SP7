#pragma once
#pragma managed

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace mjControls
{
    public static class GEO
    {
        public static Rectangle RectFromLTRB(Point lt, Point rb)
        {
            Point real_lt = new Point(System.Math.Min(lt.X, rb.X), System.Math.Min(lt.Y, rb.Y));
            Point real_rb = new Point(System.Math.Max(lt.X, rb.X), System.Math.Max(lt.Y, rb.Y));

            return Rectangle.FromLTRB(real_lt.X, real_lt.Y, real_rb.X + 1, real_rb.Y + 1);
        }
        public static RectangleF RectFromLTRB(PointF lt, PointF rb)
        {
            PointF real_lt = new PointF(System.Math.Min(lt.X, rb.X), System.Math.Min(lt.Y, rb.Y));
            PointF real_rb = new PointF(System.Math.Max(lt.X, rb.X), System.Math.Max(lt.Y, rb.Y));

            return RectangleF.FromLTRB(real_lt.X, real_lt.Y, real_rb.X, real_rb.Y);
        }

        public static Point ScalePoint(Point point, Single scale)
        {
            return new Point((Int32)(point.X * scale + (point.X >= 0 ? 0.5 : -0.5)),
                (Int32)(point.Y * scale + (point.Y >= 0 ? 0.5 : -0.5)));
        }
        public static PointF ScalePoint(PointF point, Single scale)
        {
            return new PointF(point.X * scale, point.Y * scale);
        }
        public static Point ScalePoint(Point point, Single scale_x, Single scale_y)
        {
            return new Point((Int32)(point.X * scale_x + (point.X >= 0 ? 0.5 : -0.5)),
                (Int32)(point.Y * scale_y + (point.Y >= 0 ? 0.5 : -0.5)));
        }
        public static PointF ScalePoint(PointF point, Single scale_x, Single scale_y)
        {
            return new PointF(point.X * scale_x, point.Y * scale_y);
        }
        public static Size ScaleSize(Size size, Single scale)
        {
            return new Size((Int32)(size.Width * scale + (size.Width >= 0 ? 0.5 : -0.5)),
                (Int32)(size.Height * scale + (size.Height >= 0 ? 0.5 : -0.5)));
        }
        public static Size ScaleSize(Size size, Single scale_x, Single scale_y)
        {
            return new Size((Int32)(size.Width * scale_x + (size.Width >= 0 ? 0.5 : -0.5)),
                (Int32)(size.Height * scale_y + (size.Height >= 0 ? 0.5 : -0.5)));
        }
        public static SizeF ScaleSize(SizeF size, Single scale)
        {
            return new SizeF(size.Width * scale, size.Height * scale);
        }
        public static SizeF ScaleSize(SizeF size, Single scale_x, Single scale_y)
        {
            return new SizeF(size.Width * scale_x, size.Height * scale_y);
        }
        public static Rectangle ScaleRect(Rectangle rect, Single scale)
        {
            return new Rectangle(ScalePoint(rect.Location, scale), ScaleSize(rect.Size, scale));
        }
        public static Rectangle ScaleRect(Rectangle rect, Single scale_x, Single scale_y)
        {
            return new Rectangle(ScalePoint(rect.Location, scale_x, scale_y), ScaleSize(rect.Size, scale_x, scale_y));
        }
        public static RectangleF ScaleRect(RectangleF rect, Single scale)
        {
            return new RectangleF(ScalePoint(rect.Location, scale), ScaleSize(rect.Size, scale));
        }
        public static RectangleF ScaleRect(RectangleF rect, Single scale_x, Single scale_y)
        {
            return new RectangleF(ScalePoint(rect.Location, scale_x, scale_y), ScaleSize(rect.Size, scale_x, scale_y));
        }
        public static void ScalePointArray(Point[] ary_pts, Single scale)
        {
            for (Int32 n = 0; n < ary_pts.Length; n++)
            {
                ary_pts[n] = ScalePoint(ary_pts[n], scale);
            }
        }
        public static void ScalePointArray(Point[] ary_pts, Single scale_x, Single scale_y)
        {
            for (Int32 n = 0; n < ary_pts.Length; n++)
            {
                ary_pts[n] = ScalePoint(ary_pts[n], scale_x, scale_y);
            }
        }
        public static void ScalePointArray(PointF[] ary_pts, Single scale)
        {
            for (Int32 n = 0; n < ary_pts.Length; n++)
            {
                ary_pts[n] = ScalePoint(ary_pts[n], scale);
            }
        }
        public static void ScalePointArray(PointF[] ary_pts, Single scale_x, Single scale_y)
        {
            for (Int32 n = 0; n < ary_pts.Length; n++)
            {
                ary_pts[n] = ScalePoint(ary_pts[n], scale_x, scale_y);
            }
        }
        public static Point[] ScalePointArrayWithClone(Point[] ary_pts, Single scale)
        {
            Point[] tar_pts = new Point[ary_pts.Length];

            for (Int32 n = 0; n < ary_pts.Length; n++)
            {
                tar_pts[n] = ScalePoint(ary_pts[n], scale);
            }

            return tar_pts;
        }
        public static Point[] ScalePointArrayWithClone(Point[] ary_pts, Single scale_x, Single scale_y)
        {
            Point[] tar_pts = new Point[ary_pts.Length];

            for (Int32 n = 0; n < ary_pts.Length; n++)
            {
                tar_pts[n] = ScalePoint(ary_pts[n], scale_x, scale_y);
            }

            return tar_pts;
        }
        public static PointF[] ScalePointArrayWithClone(PointF[] ary_pts, Single scale)
        {
            PointF[] tar_pts = new PointF[ary_pts.Length];

            for (Int32 n = 0; n < ary_pts.Length; n++)
            {
                tar_pts[n] = ScalePoint(ary_pts[n], scale);
            }

            return tar_pts;
        }
        public static PointF[] ScalePointArrayWithClone(PointF[] ary_pts, Single scale_x, Single scale_y)
        {
            PointF[] tar_pts = new PointF[ary_pts.Length];

            for (Int32 n = 0; n < ary_pts.Length; n++)
            {
                tar_pts[n] = ScalePoint(ary_pts[n], scale_x, scale_y);
            }

            return tar_pts;
        }

        public static Point OffsetPoint(Point pt, Int32 offset_x, Int32 offset_y)
        {
            return new Point(pt.X + offset_x, pt.Y + offset_y);
        }
        public static Point OffsetPoint(Point pt, Point offset)
        {
            return OffsetPoint(pt, offset.X, offset.Y);
        }
        public static PointF OffsetPoint(PointF pt, Single offset_x, Single offset_y)
        {
            return new PointF(pt.X + offset_x, pt.Y + offset_y);
        }
        public static PointF OffsetPoint(PointF pt, PointF offset)
        {
            return OffsetPoint(pt, offset.X, offset.Y);
        }
        public static void OffsetPointArray(Point[] ary_pt, Int32 offset_x, Int32 offset_y)
        {
            for (Int32 n = 0; n < ary_pt.Length; n++)
            {
                ary_pt[n].Offset(offset_x, offset_y);
            }
        }
        public static void OffsetPointArray(Point[] ary_pt, Point offset)
        {
            for (Int32 n = 0; n < ary_pt.Length; n++)
            {
                ary_pt[n].Offset(offset);
            }
        }
        public static void OffsetPointArray(PointF[] ary_pt, Single offset_x, Single offset_y)
        {
            for (Int32 n = 0; n < ary_pt.Length; n++)
            {
                ary_pt[n] = OffsetPoint(ary_pt[n], offset_x, offset_y);
            }
        }
        public static void OffsetPointArray(PointF[] ary_pt, PointF offset)
        {
            for (Int32 n = 0; n < ary_pt.Length; n++)
            {
                ary_pt[n] = OffsetPoint(ary_pt[n], offset);
            }
        }
        public static Point[] OffsetPointArrayWithClone(Point[] ary_pt, Int32 offset_x, Int32 offset_y)
        {
            Int32 length = ary_pt.Length;

            Point[] ary_new = new Point[length];

            for (Int32 n = 0; n < length; n++)
            {
                ary_new[n] = new Point(ary_pt[n].X + offset_x, ary_pt[n].Y + offset_y);
            }

            return ary_new;
        }
        public static Point[] OffsetPointArrayWithClone(Point[] ary_pt, Point offset)
        {
            return OffsetPointArrayWithClone(ary_pt, offset.X, offset.Y);
        }
        public static PointF[] OffsetPointArrayWithClone(PointF[] ary_pt, Single offset_x, Single offset_y)
        {
            Int32 length = ary_pt.Length;

            PointF[] ary_new = new PointF[length];

            for (Int32 n = 0; n < length; n++)
            {
                ary_new[n] = new PointF(ary_pt[n].X + offset_x, ary_pt[n].Y + offset_y);
            }

            return ary_new;
        }
        public static PointF[] OffsetPointArrayWithClone(PointF[] ary_pt, PointF offset)
        {
            return OffsetPointArrayWithClone(ary_pt, offset.X, offset.Y);
        }

        public static Rectangle BoundingBox(Point[] points)
        {
            Int32 max_x = Int32.MinValue, max_y = Int32.MinValue;
            Int32 min_x = Int32.MaxValue, min_y = Int32.MaxValue;

            foreach (Point pt in points)
            {
                if (pt.X > max_x) max_x = pt.X;
                if (pt.X < min_x) min_x = pt.X;
                if (pt.Y > max_y) max_y = pt.Y;
                if (pt.Y < min_y) min_y = pt.Y;
            }

            return Rectangle.FromLTRB(min_x, min_y, max_x + 1, max_y + 1); ;
        }
        public static Rectangle BoundingBox(PointF[] points)
        {
            Single max_x = Single.MinValue, max_y = Single.MinValue;
            Single min_x = Single.MaxValue, min_y = Single.MaxValue;

            foreach (PointF pt in points)
            {
                if (pt.X > max_x) max_x = pt.X;
                if (pt.X < min_x) min_x = pt.X;
                if (pt.Y > max_y) max_y = pt.Y;
                if (pt.Y < min_y) min_y = pt.Y;
            }

            Rectangle rect = Rectangle.FromLTRB(
                (Int32)System.Math.Floor(min_x), (Int32)System.Math.Floor(min_y),
                (Int32)System.Math.Ceiling(max_x), (Int32)System.Math.Ceiling(max_y));

            return rect;
        }
    }

    public interface IPaintItem
    {
        void Paint(Graphics g, Double scale, Point offset);

        Boolean Contain(Rectangle rect);
    }

    public class PaintItem : IPaintItem
    {
        public Rectangle Rect;

        public virtual void Paint(Graphics g, Double scale, Point offset) { return; }

        public virtual Boolean Contain(Rectangle rect) { return false; }
    }

    public class PaintLine : PaintItem
    {
        private Point StartPoint;
        private Point EndPoint;
        private Pen DrawPen = null;

        public PaintLine(Point start, Point end, Color color, Single size)
        {
            StartPoint = start;
            EndPoint = end;
            DrawPen = new Pen(new SolidBrush(color), size);

            Rect = GEO.RectFromLTRB(StartPoint, EndPoint);
        }

        public PaintLine(Point start, Point end, Color color, Single size, System.Drawing.Drawing2D.DashCap dash)
        {
            StartPoint = start;
            EndPoint = end;
            DrawPen = new Pen(new SolidBrush(color), size);
            DrawPen.DashCap = dash;

            Rect = GEO.RectFromLTRB(StartPoint, EndPoint);
        }

        public PaintLine(Point start, Point end, System.Drawing.Drawing2D.LineCap start_cap, System.Drawing.Drawing2D.LineCap end_cap, Color color, Single size)
        {
            StartPoint = start;
            EndPoint = end;
            DrawPen = new Pen(new SolidBrush(color), size);
            DrawPen.StartCap = start_cap;
            DrawPen.EndCap = end_cap;

            Rect = GEO.RectFromLTRB(StartPoint, EndPoint);
        }

        public PaintLine(Point start, Point end, System.Drawing.Drawing2D.LineCap start_cap, System.Drawing.Drawing2D.LineCap end_cap, System.Drawing.Drawing2D.DashCap dash, Color color, Single size)
        {
            StartPoint = start;
            EndPoint = end;
            DrawPen = new Pen(new SolidBrush(color), size);
            DrawPen.StartCap = start_cap;
            DrawPen.EndCap = end_cap;
            DrawPen.DashCap = dash;

            Rect = GEO.RectFromLTRB(StartPoint, EndPoint);
        }

        ~PaintLine()
        {
            if (DrawPen != null)
                DrawPen.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            Point start_pt = new Point(StartPoint.X + offset.X, StartPoint.Y + offset.Y);
            Point end_pt = new Point(EndPoint.X + offset.X, EndPoint.Y + offset.Y);

            g.DrawLine(DrawPen,
                GEO.ScalePoint(start_pt, (Single)scale),
                GEO.ScalePoint(end_pt, (Single)scale));
        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);
        }
    }

    public class PaintLineF : PaintItem
    {
        private PointF StartPoint;
        private PointF EndPoint;
        private Pen DrawPen = null;

        public PaintLineF(PointF start, PointF end, Color color, Single size)
        {
            StartPoint = start;
            EndPoint = end;
            DrawPen = new Pen(new SolidBrush(color), size);

            Rect = Rectangle.Round(GEO.RectFromLTRB(StartPoint, EndPoint));
        }

        public PaintLineF(PointF start, PointF end, Color color, Single size, System.Drawing.Drawing2D.DashCap dash)
        {
            StartPoint = start;
            EndPoint = end;
            DrawPen = new Pen(new SolidBrush(color), size);
            DrawPen.DashCap = dash;

            Rect = Rectangle.Round(GEO.RectFromLTRB(StartPoint, EndPoint));
        }

        public PaintLineF(PointF start, PointF end, System.Drawing.Drawing2D.LineCap start_cap, System.Drawing.Drawing2D.LineCap end_cap, Color color, Single size)
        {
            StartPoint = start;
            EndPoint = end;
            DrawPen = new Pen(new SolidBrush(color), size);
            DrawPen.StartCap = start_cap;
            DrawPen.EndCap = end_cap;

            Rect = Rectangle.Round(GEO.RectFromLTRB(StartPoint, EndPoint));
        }

        public PaintLineF(PointF start, PointF end, System.Drawing.Drawing2D.LineCap start_cap, System.Drawing.Drawing2D.LineCap end_cap, System.Drawing.Drawing2D.DashCap dash, Color color, Single size)
        {
            StartPoint = start;
            EndPoint = end;
            DrawPen = new Pen(new SolidBrush(color), size);
            DrawPen.StartCap = start_cap;
            DrawPen.EndCap = end_cap;
            DrawPen.DashCap = dash;

            Rect = Rectangle.Round(GEO.RectFromLTRB(StartPoint, EndPoint));
        }

        ~PaintLineF()
        {
            if (DrawPen != null)
                DrawPen.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            PointF start_pt = new PointF(StartPoint.X + offset.X, StartPoint.Y + offset.Y);
            PointF end_pt = new PointF(EndPoint.X + offset.X, EndPoint.Y + offset.Y);

            g.DrawLine(DrawPen,
                GEO.ScalePoint(start_pt, (Single)scale),
               GEO.ScalePoint(end_pt, (Single)scale));
        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);
        }
    }

    public class PaintLines : PaintItem
    {
        private Point[] LinePoints = null;
        private Pen DrawPen = null;

        public PaintLines(Point[] points, Color color, Single size)
        {
            LinePoints = points;
            DrawPen = new Pen(new SolidBrush(color), size);

            Rect = GEO.BoundingBox(LinePoints);
        }

        public PaintLines(Point[] points, Color color, Single size, System.Drawing.Drawing2D.DashCap dash)
        {
            LinePoints = points;
            DrawPen = new Pen(new SolidBrush(color), size);
            DrawPen.DashCap = dash;

            Rect = GEO.BoundingBox(LinePoints);
        }

        ~PaintLines()
        {
            if (DrawPen != null)
                DrawPen.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            Point[] pts = new Point[LinePoints.Length];

            for (Int32 n = 0; n < LinePoints.Length; n++)
            {
                pts[n] = new Point(LinePoints[n].X + offset.X, LinePoints[n].Y + offset.Y);
                pts[n] = GEO.ScalePoint(pts[n], (Single)scale);
            }

            g.DrawLines(DrawPen, pts);

            pts = null;
        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);
        }
    }

    public class PaintLinesF : PaintItem
    {
        private PointF[] LinePoints = null;
        private Pen DrawPen = null;

        public PaintLinesF(PointF[] points, Color color, Single size)
        {
            LinePoints = points;
            DrawPen = new Pen(new SolidBrush(color), size);

            Rect = GEO.BoundingBox(LinePoints);
        }

        public PaintLinesF(PointF[] points, Color color, Single size, System.Drawing.Drawing2D.DashCap dash)
        {
            LinePoints = points;
            DrawPen = new Pen(new SolidBrush(color), size);
            DrawPen.DashCap = dash;

            Rect = GEO.BoundingBox(LinePoints);
        }

        ~PaintLinesF()
        {
            if (DrawPen != null)
                DrawPen.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            PointF[] new_pts = (PointF[])LinePoints.Clone();

            GEO.OffsetPointArray(new_pts, (Single)offset.X, (Single)offset.Y);
            GEO.ScalePointArray(new_pts, (Single)scale);

            g.DrawLines(DrawPen, new_pts);
        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);
        }
    }

    public class PaintRectangle : PaintItem
    {
        private Brush FillBrush = null;
        private Pen DrawPen = null;

        public PaintRectangle(Rectangle rect, Color pen_color, Single size, Color fill_color, System.Drawing.Drawing2D.DashStyle style)
        {
            Rect = rect;
            DrawPen = new Pen(new SolidBrush(pen_color), size);
            DrawPen.DashStyle = style;
            FillBrush = new SolidBrush(fill_color);
        }

        public PaintRectangle(Rectangle rect, Color color, Single size)
        {
            Rect = rect;
            DrawPen = new Pen(new SolidBrush(color), size);
            FillBrush = null;
        }

        public PaintRectangle(Rectangle rect, Color color, Single size, System.Drawing.Drawing2D.DashCap dash)
        {
            Rect = rect;
            DrawPen = new Pen(new SolidBrush(color), size);
            DrawPen.DashCap = dash;
            FillBrush = null;
        }

        public PaintRectangle(Rectangle rect, Color pen_color, Single size, Color fill_color)
        {
            Rect = rect;
            DrawPen = new Pen(new SolidBrush(pen_color), size);
            FillBrush = new SolidBrush(fill_color);
        }

        public PaintRectangle(Rectangle rect, Color pen_color, Single size, Color fill_color, System.Drawing.Drawing2D.DashCap dash)
        {
            Rect = rect;
            DrawPen = new Pen(new SolidBrush(pen_color), size);
            DrawPen.DashCap = dash;
            FillBrush = new SolidBrush(fill_color);
        }

        public PaintRectangle(Rectangle rect, Color fill_color)
        {
            Rect = rect;
            DrawPen = null;
            FillBrush = new SolidBrush(fill_color);
        }

        ~PaintRectangle()
        {
            if (FillBrush != null)
                FillBrush.Dispose();

            if (DrawPen != null)
                DrawPen.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            Rectangle rect = Rect;
            rect.X += offset.X;
            rect.Y += offset.Y;
            rect = GEO.ScaleRect(rect, (Single)scale);

            if (FillBrush != null)
                g.FillRectangle(FillBrush, rect);

            if (DrawPen != null)

                g.DrawRectangle(DrawPen, rect);
        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);
        }
    }

    public class PaintRectangleF : PaintItem
    {
        private RectangleF Rectf;

        private Brush FillBrush = null;
        private Pen DrawPen = null;

        public PaintRectangleF(RectangleF rect, Color pen_color, Single size, Color fill_color, System.Drawing.Drawing2D.DashStyle style)
        {
            Rectf = rect;
            Rect = Rectangle.Truncate(rect);

            DrawPen = new Pen(new SolidBrush(pen_color), size);
            DrawPen.DashStyle = style;
            FillBrush = new SolidBrush(fill_color);
        }

        public PaintRectangleF(RectangleF rect, Color color, Single size)
        {
            Rectf = rect;
            Rect = Rectangle.Truncate(rect);

            DrawPen = new Pen(new SolidBrush(color), size);
            FillBrush = null;
        }

        public PaintRectangleF(RectangleF rect, Color color, Single size, System.Drawing.Drawing2D.DashCap dash)
        {
            Rectf = rect;
            Rect = Rectangle.Truncate(rect);

            DrawPen = new Pen(new SolidBrush(color), size);
            DrawPen.DashCap = dash;
            FillBrush = null;
        }

        public PaintRectangleF(RectangleF rect, Color pen_color, Single size, Color fill_color)
        {
            Rectf = rect;
            Rect = Rectangle.Truncate(rect);

            DrawPen = new Pen(new SolidBrush(pen_color), size);
            FillBrush = new SolidBrush(fill_color);
        }

        public PaintRectangleF(RectangleF rect, Color pen_color, Single size, Color fill_color, System.Drawing.Drawing2D.DashCap dash)
        {
            Rectf = rect;
            Rect = Rectangle.Truncate(rect);

            DrawPen = new Pen(new SolidBrush(pen_color), size);
            DrawPen.DashCap = dash;
            FillBrush = new SolidBrush(fill_color);
        }

        public PaintRectangleF(RectangleF rect, Color fill_color)
        {
            Rectf = rect;
            Rect = Rectangle.Truncate(rect);

            DrawPen = null;
            FillBrush = new SolidBrush(fill_color);
        }

        ~PaintRectangleF()
        {
            if (FillBrush != null)
                FillBrush.Dispose();

            if (DrawPen != null)
                DrawPen.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            RectangleF rect = Rectf;
            rect.X += offset.X;
            rect.Y += offset.Y;
            rect = GEO.ScaleRect(rect, (Single)scale);

            if (FillBrush != null)

                g.FillRectangle(FillBrush, rect);
            if (DrawPen != null)

                g.DrawRectangle(DrawPen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);
        }
    }

    public class PaintEllipse : PaintItem
    {
        private Brush FillBrush = null;
        private Pen DrawPen = null;

        public PaintEllipse(Rectangle rect, Color color, Single size)
        {
            Rect = rect;
            DrawPen = new Pen(new SolidBrush(color), size);
            FillBrush = null;
        }

        public PaintEllipse(Rectangle rect, Color color, Single size, System.Drawing.Drawing2D.DashCap dash)
        {
            Rect = rect;
            DrawPen = new Pen(new SolidBrush(color), size);
            DrawPen.DashCap = dash;
            FillBrush = null;
        }

        public PaintEllipse(Rectangle rect, Color pen_color, Single size, Color fill_color)
        {
            Rect = rect;
            DrawPen = new Pen(new SolidBrush(pen_color), size);
            FillBrush = new SolidBrush(fill_color);
        }

        public PaintEllipse(Rectangle rect, Color pen_color, Single size, Color fill_color, System.Drawing.Drawing2D.DashCap dash)
        {
            Rect = rect;
            DrawPen = new Pen(new SolidBrush(pen_color), size);
            DrawPen.DashCap = dash;
            FillBrush = new SolidBrush(fill_color);
        }

        public PaintEllipse(Rectangle rect, Color fill_color)
        {
            Rect = rect;
            DrawPen = null;
            FillBrush = new SolidBrush(fill_color);
        }

        ~PaintEllipse()
        {
            if (FillBrush != null)
                FillBrush.Dispose();

            if (DrawPen != null)
                DrawPen.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            Rectangle rect = Rect;
            rect.X += offset.X;
            rect.Y += offset.Y;
            rect = GEO.ScaleRect(rect, (Single)scale);

            if (FillBrush != null)

                g.FillEllipse(FillBrush, rect);
            if (DrawPen != null)

                g.DrawEllipse(DrawPen, rect);
        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);
        }
    }

    public class PaintEllipseF : PaintItem
    {
        private RectangleF RectF;
        private Brush FillBrush = null;
        private Pen DrawPen = null;

        public PaintEllipseF(RectangleF rect, Color color, Single size)
        {
            RectF = rect;
            DrawPen = new Pen(new SolidBrush(color), size);
            FillBrush = null;

            Rect = Rectangle.Round(RectF);
        }

        public PaintEllipseF(RectangleF rect, Color color, Single size, System.Drawing.Drawing2D.DashCap dash)
        {
            RectF = rect;
            DrawPen = new Pen(new SolidBrush(color), size);
            DrawPen.DashCap = dash;
            FillBrush = null;

            Rect = Rectangle.Round(RectF);
        }

        public PaintEllipseF(RectangleF rect, Color pen_color, Single size, Color fill_color)
        {
            RectF = rect;
            DrawPen = new Pen(new SolidBrush(pen_color), size);
            FillBrush = new SolidBrush(fill_color);

            Rect = Rectangle.Round(RectF);
        }

        public PaintEllipseF(RectangleF rect, Color pen_color, Single size, Color fill_color, System.Drawing.Drawing2D.DashCap dash)
        {
            RectF = rect;
            DrawPen = new Pen(new SolidBrush(pen_color), size);
            DrawPen.DashCap = dash;
            FillBrush = new SolidBrush(fill_color);

            Rect = Rectangle.Round(RectF);
        }

        public PaintEllipseF(RectangleF rect, Color fill_color)
        {
            RectF = rect;
            DrawPen = null;
            FillBrush = new SolidBrush(fill_color);

            Rect = Rectangle.Round(RectF);
        }

        ~PaintEllipseF()
        {
            if (FillBrush != null)
                FillBrush.Dispose();

            if (DrawPen != null)
                DrawPen.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            RectangleF rect = RectF;
            rect.X += offset.X;
            rect.Y += offset.Y;
            rect = GEO.ScaleRect(rect, (Single)scale);

            if (FillBrush != null)

                g.FillEllipse(FillBrush, rect);
            if (DrawPen != null)

                g.DrawEllipse(DrawPen, rect);
        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);
        }
    }

    public class PaintArc : PaintItem
    {
        private Double StartAngle;
        private Double SweepAngle;
        private RectangleF RectF;

        private Brush FillBrush = null;
        private Pen DrawPen = null;

        public PaintArc(Rectangle rect, Double start_angle, Double sweep_angle, Color color, Single size)
        {
            Rect = rect;
            StartAngle = start_angle;
            SweepAngle = sweep_angle;

            DrawPen = new Pen(new SolidBrush(color), size);
            FillBrush = null;
        }

        public PaintArc(Rectangle rect, Double start_angle, Double sweep_angle, Color fill_color)
        {
            Rect = rect;
            StartAngle = start_angle;
            SweepAngle = sweep_angle;

            DrawPen = null;
            FillBrush = new SolidBrush(fill_color);
        }

        ~PaintArc()
        {
            if (FillBrush != null)
                FillBrush.Dispose();

            if (DrawPen != null)
                DrawPen.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            Rectangle rect = Rect;
            rect.X += offset.X;
            rect.Y += offset.Y;
            rect = GEO.ScaleRect(rect, (Single)scale);

            if (FillBrush != null)

                g.FillPie(FillBrush, rect, (Single)(StartAngle - 1.0), (Single)(SweepAngle + 1.0));
            if (DrawPen != null)

                g.DrawArc(DrawPen, rect, (Single)(StartAngle - 1.0), (Single)(SweepAngle + 1.0));

        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);
        }
    }

    public class PaintArcF : PaintItem
    {
        private Double StartAngle;
        private Double SweepAngle;
        private RectangleF Rectf;

        private Brush FillBrush = null;
        private Pen DrawPen = null;

        public PaintArcF(RectangleF rect, Double start_angle, Double sweep_angle, Color color, Single size)
        {
            Rectf = rect;
            Rect = Rectangle.Round(Rectf);

            StartAngle = start_angle;
            SweepAngle = sweep_angle;
            DrawPen = new Pen(new SolidBrush(color), size);
            FillBrush = null;
        }

        public PaintArcF(RectangleF rect, Double start_angle, Double sweep_angle, Color fill_color)
        {
            Rectf = rect;
            Rect = Rectangle.Round(Rectf);

            StartAngle = start_angle;
            SweepAngle = sweep_angle;
            DrawPen = null;
            FillBrush = new SolidBrush(fill_color);
        }

        ~PaintArcF()
        {
            if (FillBrush != null)
                FillBrush.Dispose();

            if (DrawPen != null)
                DrawPen.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            RectangleF rect = Rectf;
            rect.X += offset.X;
            rect.Y += offset.Y;
            rect = GEO.ScaleRect(rect, (Single)scale);

            if (FillBrush != null)

                g.FillPie(FillBrush, Rectangle.Round(rect), (Single)(StartAngle - 1.0), (Single)(SweepAngle + 1.0));
            if (DrawPen != null)

                g.DrawArc(DrawPen, rect, (Single)(StartAngle - 1.0), (Single)(SweepAngle + 1.0));
        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);
        }
    }

    public class PaintPolygon : PaintItem
    {
        private Point[] Polygon = null;
        private Point[][] PolygonInner = null;
        private Pen DrawPen = null;
        private Brush FillBrush = null;

        public PaintPolygon(Point[] polygon, Color fill_color)
        {
            Polygon = polygon;
            DrawPen = null;
            FillBrush = new SolidBrush(fill_color);

            Rect = GEO.BoundingBox(Polygon);
        }

        public PaintPolygon(Point[] polygon, Color color, Single size)
        {
            Polygon = polygon;
            DrawPen = new Pen(new SolidBrush(color), size);
            FillBrush = null;

            Rect = GEO.BoundingBox(Polygon);
        }

        public PaintPolygon(Point[] polygon, Color color, Single size, Color fill_color)
        {
            Polygon = polygon;
            DrawPen = new Pen(new SolidBrush(color), size);
            FillBrush = new SolidBrush(fill_color);

            Rect = GEO.BoundingBox(Polygon);
        }

        ~PaintPolygon()
        {
            if (FillBrush != null)
                FillBrush.Dispose();

            if (DrawPen != null)
                DrawPen.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            if (Polygon.Length > 2)
            {
                Point[] new_polygon = (Point[])Polygon.Clone();

                GEO.OffsetPointArray(new_polygon, offset.X, offset.Y);
                GEO.ScalePointArray(new_polygon, (Single)scale);

                if (FillBrush != null)

                    g.FillPolygon(FillBrush, new_polygon);
                if (DrawPen != null)

                    g.DrawPolygon(DrawPen, new_polygon);
            }
        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);

            // 					GraphicsPath^ path = new GraphicsPath();
            // 					path.AddPolygon( Polygon );
            // 
            // 					Region^ region = new Region( path );
            // 
            // 					return region.IsVisible( rect );
        }
    }

    public class PaintPolygonF : PaintItem
    {
        private PointF[] Polygon = null;
        private PointF[][] PolygonInner = null;
        private Pen DrawPen = null;
        private Brush FillBrush = null;

        public PaintPolygonF(PointF[] polygon, Color fill_color)
        {
            Polygon = polygon;
            PolygonInner = null;

            DrawPen = null;
            FillBrush = new SolidBrush(fill_color);

            Rect = GEO.BoundingBox(Polygon);
        }

        public PaintPolygonF(PointF[] polygon, Color color, Single size)
        {
            Polygon = polygon;
            PolygonInner = null;

            DrawPen = new Pen(new SolidBrush(color), size);
            FillBrush = null;

            Rect = GEO.BoundingBox(Polygon);
        }

        public PaintPolygonF(PointF[] polygon, Color color, Single size, Color fill_color)
        {
            Polygon = polygon;
            PolygonInner = null;

            DrawPen = new Pen(new SolidBrush(color), size);
            FillBrush = new SolidBrush(fill_color);

            Rect = GEO.BoundingBox(Polygon);
        }

        public PaintPolygonF(PointF[] polygon, PointF[][] polygon_inner, Color fill_color)
        {
            Polygon = polygon;
            PolygonInner = polygon_inner;

            DrawPen = null;
            FillBrush = new SolidBrush(fill_color);

            Rect = GEO.BoundingBox(Polygon);
        }

        public PaintPolygonF(PointF[] polygon, PointF[][] polygon_inner, Color color, Single size)
        {
            Polygon = polygon;
            PolygonInner = polygon_inner;

            DrawPen = new Pen(new SolidBrush(color), size);
            FillBrush = null;

            Rect = GEO.BoundingBox(Polygon);
        }

        public PaintPolygonF(PointF[] polygon, PointF[][] polygon_inner, Color color, Single size, Color fill_color)
        {
            Polygon = polygon;
            PolygonInner = polygon_inner;

            DrawPen = new Pen(new SolidBrush(color), size);
            FillBrush = new SolidBrush(fill_color);

            Rect = GEO.BoundingBox(Polygon);
        }

        ~PaintPolygonF()
        {
            if (FillBrush != null)
                FillBrush.Dispose();

            if (DrawPen != null)
                DrawPen.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            if (Polygon.Length <= 2)
                return;

            if (PolygonInner == null || PolygonInner.Length == 0)
            {
                PointF[] new_polygon = (PointF[])Polygon.Clone();

                GEO.OffsetPointArray(new_polygon, (Single)offset.X, (Single)offset.Y);
                GEO.ScalePointArray(new_polygon, (Single)scale);

                if (FillBrush != null)

                    g.FillPolygon(FillBrush, new_polygon);
                if (DrawPen != null)

                    g.DrawPolygon(DrawPen, new_polygon);
            }
            else
            {
                System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();

                PointF[] out_polygon = (PointF[])Polygon.Clone();

                GEO.OffsetPointArray(out_polygon, (Single)offset.X, (Single)offset.Y);
                GEO.ScalePointArray(out_polygon, (Single)scale);

                gp.AddPolygon(out_polygon);

                for (Int32 i = 0; i < PolygonInner.Length; i++)
                {
                    PointF[] in_polygon = (PointF[])PolygonInner[i].Clone();

                    GEO.OffsetPointArray(in_polygon, (Single)offset.X, (Single)offset.Y);
                    GEO.ScalePointArray(in_polygon, (Single)scale);

                    gp.AddPolygon(in_polygon);
                }

                if (FillBrush != null)

                    g.FillPath(FillBrush, gp);
                if (DrawPen != null)

                    g.DrawPath(DrawPen, gp);

            }
        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);
        }
    }

    public class PaintCross : PaintItem
    {
        private Pen DrawPen = null;

        public PaintCross(Rectangle rect, Color color, Single size)
        {
            Rect = rect;
            DrawPen = new Pen(new SolidBrush(color), size);
        }

        public PaintCross(PointF pt, Int32 cross_size, Color color, Single size)
        {
            Rect.X = (Int32)pt.X;
            Rect.Y = (Int32)pt.Y;
            Rect.Size = new Size(cross_size, cross_size);
            Rect.Offset(-cross_size / 2, -cross_size / 2);
            DrawPen = new Pen(new SolidBrush(color), size);
        }

        public PaintCross(Point pt, Int32 cross_size, Color color, Single size)
        {
            Rect.Location = pt;
            Rect.Size = new Size(cross_size, cross_size);
            Rect.Offset(-cross_size / 2, -cross_size / 2);
            DrawPen = new Pen(new SolidBrush(color), size);
        }

        ~PaintCross()
        {
            if (DrawPen != null)
                DrawPen.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            Rectangle rect = Rect;
            rect.X += offset.X;
            rect.Y += offset.Y;
            rect = GEO.ScaleRect(rect, (Single)scale);

            g.DrawLine(DrawPen, new Point(rect.X + rect.Width / 2, rect.Y), new Point(rect.X + rect.Width / 2, rect.Y + rect.Height));
            g.DrawLine(DrawPen, new Point(rect.X, rect.Y + rect.Height / 2), new Point(rect.X + rect.Width, rect.Y + rect.Height / 2));
        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);
        }
    }

    public class PaintCrossF : PaintItem
    {
        private RectangleF Rectf;
        private Pen DrawPen = null;

        public PaintCrossF(RectangleF rect, Color color, Single size)
        {
            Rectf = rect;
            Rect = Rectangle.Truncate(rect);
            DrawPen = new Pen(new SolidBrush(color), size);
        }

        public PaintCrossF(PointF pt, Int32 cross_size, Color color, Single size)
        {
            Rectf.Location = pt;
            Rectf.Size = new SizeF(0.0f, 0.0f);
            Rectf.Inflate(cross_size / 2.0f, cross_size / 2.0f);
            Rect = Rectangle.Truncate(Rectf);
            DrawPen = new Pen(new SolidBrush(color), size);
        }

        ~PaintCrossF()
        {
            if (DrawPen != null)
                DrawPen.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            RectangleF rect = Rectf;
            rect.X += offset.X;
            rect.Y += offset.Y;
            rect = GEO.ScaleRect(rect, (Single)scale);

            g.DrawLine(DrawPen, new PointF(rect.X + rect.Width / 2.0f, rect.Y), new PointF(rect.X + rect.Width / 2.0f, rect.Y + rect.Height));
            g.DrawLine(DrawPen, new PointF(rect.X, rect.Y + rect.Height / 2.0f), new PointF(rect.X + rect.Width, rect.Y + rect.Height / 2.0f));
        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);
        }
    }

    public class PaintXCross : PaintItem
    {
        private Pen DrawPen = null;

        public PaintXCross(Rectangle rect, Color color, Single size)
        {
            Rect = rect;
            DrawPen = new Pen(new SolidBrush(color), size);
        }

        ~PaintXCross()
        {
            if (DrawPen != null)
                DrawPen.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            Rectangle rect = Rect;
            rect.X += offset.X;
            rect.Y += offset.Y;
            rect = GEO.ScaleRect(rect, (Single)scale);

            g.DrawLine(DrawPen, rect.Location, new Point(rect.Right, rect.Bottom));
            g.DrawLine(DrawPen, new Point(rect.Right, rect.Top), new Point(rect.X, rect.Bottom));
        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);
        }
    }

    public class PaintXCrossF : PaintItem
    {
        private RectangleF Rectf;
        private Pen DrawPen = null;

        public PaintXCrossF(RectangleF rect, Color color, Single size)
        {
            Rectf = rect;
            Rect = Rectangle.Truncate(rect);
            DrawPen = new Pen(new SolidBrush(color), size);
        }

        ~PaintXCrossF()
        {
            if (DrawPen != null)
                DrawPen.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            RectangleF rect = Rectf;
            rect.X += offset.X;
            rect.Y += offset.Y;
            rect = GEO.ScaleRect(rect, (Single)scale);

            g.DrawLine(DrawPen, rect.Location, new PointF(rect.Right, rect.Bottom));
            g.DrawLine(DrawPen, new PointF(rect.Right, rect.Top), new PointF(rect.X, rect.Bottom));
        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);
        }
    }

    public class PaintString : PaintItem
    {
        private String Text = null;
        private Brush DrawBrush = null;
        private Brush DrawBackBrush = null;
        private StringFormat TextFormat = null;
        private Font TextFont = null;

        public PaintString(String text, String font, Color color, Single size, Rectangle rect)
        {
            Rect = rect;
            Text = text;
            FontFamily family = new FontFamily(font);
            TextFont = new Font(family, size);
            DrawBrush = new SolidBrush(color);
            DrawBackBrush = null;
            TextFormat = new StringFormat(StringFormatFlags.NoClip);
        }

        public PaintString(String text, String font, Color color, Single size, FontStyle style, StringAlignment linealign, StringAlignment align, Rectangle rect)
        {
            Rect = rect;
            Text = text;

            FontFamily family = new FontFamily(font);
            TextFont = new Font(family, size, style, GraphicsUnit.Pixel);
            DrawBrush = new SolidBrush(color);
            DrawBackBrush = null;
            TextFormat = new StringFormat(StringFormatFlags.NoClip);
            TextFormat.LineAlignment = linealign;
            TextFormat.Alignment = align;
        }

        public PaintString(String text, String font, Color color, Color back_color, Single size, FontStyle style, StringAlignment linealign, StringAlignment align, Rectangle rect)
        {
            Rect = rect;
            Text = text;

            FontFamily family = new FontFamily(font);
            TextFont = new Font(family, size, style, GraphicsUnit.Pixel);
            DrawBrush = new SolidBrush(color);
            DrawBackBrush = new SolidBrush(back_color);
            TextFormat = new StringFormat(StringFormatFlags.NoClip);
            TextFormat.LineAlignment = linealign;
            TextFormat.Alignment = align;
        }

        ~PaintString()
        {
            if (TextFont != null)
                TextFont.Dispose();

            if (TextFormat != null)
                TextFormat.Dispose();

            if (DrawBrush != null)
                DrawBrush.Dispose();

            if (DrawBackBrush != null)
                DrawBackBrush.Dispose();
        }

        public override void Paint(Graphics g, Double scale, Point offset)
        {
            Rectangle rect = Rect;
            rect.X += offset.X;
            rect.Y += offset.Y;
            rect = GEO.ScaleRect(rect, (Single)scale);

            if (DrawBackBrush != null)
            {
                SizeF str_size = g.MeasureString(Text, TextFont, rect.Size);
                PointF str_location = new PointF();

                switch (TextFormat.LineAlignment)
                {
                    case StringAlignment.Near:
                        str_location.Y = (Single)rect.Y;
                        break;
                    case StringAlignment.Center:
                        str_location.Y = (Single)rect.Y + (Single)rect.Height / 2.0f - (Single)str_size.Height / 2.0f;
                        break;
                    case StringAlignment.Far:
                        str_location.Y = (Single)rect.Y + (Single)rect.Height - (Single)str_size.Height;
                        break;
                }

                switch (TextFormat.Alignment)
                {
                    case StringAlignment.Near:
                        str_location.X = (Single)rect.X;
                        break;
                    case StringAlignment.Center:
                        str_location.X = (Single)rect.X + (Single)rect.Width / 2.0f - (Single)str_size.Width / 2.0f;
                        break;
                    case StringAlignment.Far:
                        str_location.X = (Single)rect.X + (Single)rect.Width - (Single)str_size.Width;
                        break;
                }

                g.FillRectangle(DrawBackBrush, new RectangleF(str_location, str_size));
            }

            g.DrawString(Text, TextFont, DrawBrush, rect, TextFormat);
        }

        public override Boolean Contain(Rectangle rect)
        {
            return rect.IntersectsWith(Rect);
        }
    }

    public class PaintItemCollection : Collection<PaintItem> { }

}




