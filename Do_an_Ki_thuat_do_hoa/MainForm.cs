using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Do_an_Ki_thuat_do_hoa
{
    public partial class MainForm : Form
    {
        private Color[,] colorArr;
        private const int size_LuoiPixel = 500;
        private bool mouseDown = false;
        public enum type { D2, D3 };
        public enum Mode2D { point, circle, line, rectangle, square, parall, triangle, ellipse, diamond, trape, rectangular, cone, pyramid };
        public enum ModeStyle { dda, bresenham, midpoint };
        private enum Option { point, drag, input, fill, eraser, picker, transform, move, scaling, choose, draw };
        private int mode;//chon hinh
        private int modePaint;//chon che do ve
        private int D;
        private int pointCount;
        private string path;
        private Point[] pointInput;
        private List<Hinh2D> hinh2D;
        private List<Hinh2D> hinh3D;
        private List<Point> l;
        private List<Hinh2D> hinhTmp;
        public static Color currentColor;
        public static Color fillColor;
        private bool hide = true;
        private Hinh2D hchoose;
        private int xmin = 102, ymin = 102, xmax = -1, ymax = -1;
        //private Image img;
        private BufferedGraphicsContext currentContext;
        private BufferedGraphics myBuffer;
        private static float currentBrush = 4f;

        public static float CurrentBrush { get => currentBrush; set => currentBrush = value; }

        public MainForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            InitProperties();
        }
        private void InitProperties()
        {
            mode = -1;
            modePaint = -1;
            InitColorArr();
            pointCount = -1;
            pointInput = new Point[4];
            hinh2D = new List<Hinh2D>();
            hinh3D = new List<Hinh2D>();
            D = -1;
            currentColor = toolStripButton5.BackColor;
            fillColor = toolStripButton9.BackColor;
            currentContext = BufferedGraphicsManager.Current;
            myBuffer = currentContext.Allocate(panel_Luoi_Pixel.CreateGraphics(), panel_Luoi_Pixel.DisplayRectangle);
            myBuffer.Graphics.Clear(panel_Luoi_Pixel.BackColor);
            path = "";
            contextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(ContextMenuStrip1_ItemClicked);
            contextBrush.ItemClicked += new ToolStripItemClickedEventHandler(ContextBrush_ItemClicked);
        }
        private void InitToado2D()
        {
            myBuffer.Graphics.Clear(panel_Luoi_Pixel.BackColor);
            for (int i = 0; i <= size_LuoiPixel / 5; i++)
            {
                myBuffer.Graphics.DrawLine(new Pen(Color.Tan), 5 * i, 0, 5 * i, size_LuoiPixel);
                myBuffer.Graphics.DrawLine(new Pen(Color.Tan), 0, 5 * i, size_LuoiPixel, 5 * i);
            }
            myBuffer.Graphics.DrawLine(new Pen(Color.Red), size_LuoiPixel / 2, 0, size_LuoiPixel / 2, size_LuoiPixel);
            myBuffer.Graphics.DrawLine(new Pen(Color.Red), 0, size_LuoiPixel / 2, size_LuoiPixel, size_LuoiPixel / 2);
            myBuffer.Render();
        }
        private void InitToado3D()
        {
            myBuffer.Graphics.Clear(panel_Luoi_Pixel.BackColor);
            for (int i = 0; i <= size_LuoiPixel / 5; i++)
            {
                myBuffer.Graphics.DrawLine(new Pen(Color.Tan), 5 * i, 0, 5 * i, size_LuoiPixel);
                myBuffer.Graphics.DrawLine(new Pen(Color.Tan), 0, 5 * i, size_LuoiPixel, 5 * i);
            }
            myBuffer.Graphics.DrawLine(new Pen(Color.Red), size_LuoiPixel / 2, 0, size_LuoiPixel / 2, size_LuoiPixel / 2);
            myBuffer.Graphics.DrawLine(new Pen(Color.Red), size_LuoiPixel / 2, size_LuoiPixel / 2, size_LuoiPixel, size_LuoiPixel / 2);
            myBuffer.Graphics.DrawLine(new Pen(Color.Red), size_LuoiPixel / 2, size_LuoiPixel / 2, 0, size_LuoiPixel);
            myBuffer.Render();
        }
        private void InitColorArr()
        {
            colorArr = new Color[size_LuoiPixel / 5 + 1, size_LuoiPixel / 5 + 1];
            for (int i = 0; i <= size_LuoiPixel / 5; i++)
                for (int j = 0; j <= size_LuoiPixel / 5; j++)
                    colorArr[i, j] = Color.Empty;
        }
        //
        //Thuoc tinh MainForm
        //
        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            if (((this.Location.X + this.Width) > Screen.PrimaryScreen.Bounds.Width) || ((this.Location.Y + this.Height) > Screen.PrimaryScreen.Bounds.Height))
            {
                Bitmap bm = new Bitmap(panel_Luoi_Pixel.Width, panel_Luoi_Pixel.Height);
                using (Graphics g = Graphics.FromImage(bm))
                {
                    myBuffer.Render(g);
                }
                panel_Luoi_Pixel.BackgroundImage = bm;
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (D == (int)type.D3) return;
            if (e.Control && e.KeyCode.Equals(Keys.S))
            {
                SaveToolStripButton_Click(sender, new EventArgs());
            }
            else if (e.Control && e.KeyCode.Equals(Keys.O))
            {
                OpenToolStripButton_Click(sender, new EventArgs());
            }
            else if (e.Control && e.KeyCode.Equals(Keys.N))
            {
                NewToolStripButton_Click(sender, new EventArgs());
            }

            else if (e.KeyCode.Equals(Keys.Tab)) Repaint();

        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (D == (int)type.D2 || D == (int)type.D3) Repaint();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Saveornot()) return;
            DialogResult dlr = MessageBox.Show("Do you want to save changes?", "Paint", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dlr == DialogResult.Yes) SaveToolStripButton_Click(sender, e);
            else if (dlr == DialogResult.No) Button_Clear_Click(sender, e);
            else if (dlr == DialogResult.Cancel) e.Cancel = true;
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            Button_2D_Click(sender, e);
        }
        //
        //In point ra luoi pixel
        //
        private void PutColor(Point p, Color color)
        {
            if (p.X >= 0 && p.Y >= 0 && p.X <= size_LuoiPixel / 5 && p.Y <= size_LuoiPixel / 5) colorArr[p.X, p.Y] = color;
        }
        private Point PutPixel(Point p, Color color)
        {
            if (p.X >= 0 && p.Y >= 0 && p.X <= size_LuoiPixel / 5 && p.Y <= size_LuoiPixel / 5)
            {
                PutColor(p, color);
                myBuffer.Graphics.FillRectangle(new SolidBrush(color), p.X * 5 - 1, p.Y * 5 - 1, currentBrush, currentBrush);
                myBuffer.Render();
            }
            return p;
        }
        private Point PutPixelBrush(Point p, Color color, float brush)
        {
            if (p.X >= 0 && p.Y >= 0 && p.X <= size_LuoiPixel / 5 && p.Y <= size_LuoiPixel / 5)
            {
                PutColor(p, color);
                myBuffer.Graphics.FillRectangle(new SolidBrush(color), p.X * 5 - 1, p.Y * 5 - 1, brush, brush);
                myBuffer.Render();
            }
            return p;
        }
        //
        //Ham can thiet
        //
        private void Repaint()
        {
            myBuffer.Graphics.Clear(panel_Luoi_Pixel.BackColor);
            if (D == (int)type.D2) InitToado2D();
            else if (D == (int)type.D3) InitToado3D();
            InitColorArr();
            foreach (Hinh2D token in D == (int)type.D2 ? hinh2D : hinh3D)
            {
                foreach (Point p in token.ArrPoint)
                {
                    PutPixelBrush(p, token.Color, token.Brush);
                }
            }
            myBuffer.Render();
        }
        private Hinh2D Checkpoint(Point p)
        {
            foreach (Hinh2D token in D == (int)type.D2 ? hinh2D : hinh3D)
            {
                foreach (Point to in token.ArrPoint)
                {
                    if (to.X - 1 <= p.X && p.X <= to.X + 1 && to.Y - 1 <= p.Y && p.Y <= to.Y + 1) return token;
                }
            }
            return null;
        }
        private Point[] MulMatrix(Point[] arrPoint, double[,] arr)
        {
            List<Point> l = new List<Point>();
            Point p = new Point();
            for (int i = 0; i < arrPoint.Count(); i++)
            {
                p.X = (int)Math.Ceiling(arrPoint[i].X * arr[0, 0] + arrPoint[i].Y * arr[1, 0] + arr[2, 0]);
                p.Y = (int)Math.Ceiling(arrPoint[i].X * arr[0, 1] + arrPoint[i].Y * arr[1, 1] + arr[2, 1]);
                l.Add(new Point(p));
            }
            return l.ToArray();
        }
        private bool Saveornot()
        {
            if (D == (int)type.D2)
            {
                if (hinh2D.Count == 0) return false;
                if (hinhTmp != null && hinhTmp.Count == hinh2D.Count)
                {
                    int i = 0;
                    while (i < hinhTmp.Count() && hinhTmp.ElementAt(i).Equals(hinh2D.ElementAt(i))) i++;
                    if (i == hinhTmp.Count) return false;
                }
            }
            else if (D == (int)type.D3)
            {
                if (hinh3D.Count == 0) return false;
                if (hinhTmp != null && hinhTmp.Count() == hinh3D.Count())
                {
                    int i = 0;
                    while (i < hinhTmp.Count() && hinhTmp.ElementAt(i).Equals(hinh3D.ElementAt(i))) i++;
                    if (i == hinhTmp.Count) return false;
                }
            }
            return true;
        }
        //
        //Ham ve danh dau
        //
        private void Danhdau(Point p1, Point p2, Point p3, Point p4, Color color)
        {
            Mid_line_choose(new Line(p1, p2), color);
            Mid_line_choose(new Line(p1, p4), color);
            Mid_line_choose(new Line(p3, p2), color);
            Mid_line_choose(new Line(p3, p4), color);
        }
        private void Mid_line_choose(Line l, Color color)
        {
            int x1 = l.P1.X, y1 = l.P1.Y, x2 = l.P2.X, y2 = l.P2.Y, d;
            List<Point> list = new List<Point>();
            int dx = Math.Abs(x1 - x2);
            int dy = Math.Abs(y1 - y2);
            if (dx == 0 && dy == 0) return;
            PutPixelChoose(new Point(x1, y1), color);
            if (dy < dx)
            {
                d = (dy - dx) / 2;
                do
                {
                    x1 += x1 < x2 ? 1 : -1;
                    if (d <= 0) d += dy;
                    else
                    {
                        d += dy - dx;
                        y1 += y1 < y2 ? 1 : -1;
                    }
                    PutPixelChoose(new Point(x1, y1), color);
                } while (x1 != x2);
            }
            else
            {
                d = (dx - dy) / 2;
                do
                {
                    y1 += y1 < y2 ? 1 : -1;
                    if (d <= 0) d += dx;
                    else
                    {
                        d += dx - dy;
                        x1 += x1 < x2 ? 1 : -1;
                    }
                    PutPixelChoose(new Point(x1, y1), color);
                } while (y1 != y2);
            }
        }
        private void PutPixelChoose(Point p, Color color)
        {
            if (p.X >= 0 && p.Y >= 0 && p.X <= size_LuoiPixel / 5 && p.Y <= size_LuoiPixel / 5)
            {
                PutColor(p, color);
                myBuffer.Graphics.FillRectangle(new SolidBrush(color), p.X * 5 - 1, p.Y * 5 - 1, 3f, 2f);
                myBuffer.Render();
            }
        }
        private void moveChoose(int dx, int dy, Hinh2D h)
        {
            if (dx == 0 && dy == 0) return;
            if (h.Name.Equals("Point") || h.Name.Equals("Circle") || h.Name.Equals("Ellipse"))
            {
                h.P1.X += dx;
                h.P1.Y += dy;
            }
            else if (h.Name.Equals("Line"))
            {
                h.P1.X += dx;
                h.P1.Y += dy;
                h.P2.X += dx;
                h.P2.Y += dy;
            }
            else if (h.Name.Equals("Rectangle") || h.Name.Equals("Square") || h.Name.Equals("Parallelogram") || h.Name.Equals("Diamond") || h.Name.Equals("Trapezoid"))
            {
                h.P1.X += dx;
                h.P1.Y += dy;
                h.P2.X += dx;
                h.P2.Y += dy;
                h.P3.X += dx;
                h.P3.Y += dy;
                h.P4.X += dx;
                h.P4.Y += dy;
            }
            else if (h.Name.Equals("Triangle"))
            {
                h.P1.X += dx;
                h.P1.Y += dy;
                h.P2.X += dx;
                h.P2.Y += dy;
                h.P3.X += dx;
                h.P3.Y += dy;
            }
            h.Tam.X += dx;
            h.Tam.Y += dy;
            h.ArrPoint = MulMatrix(h.ArrPoint, new TransClass().tinhtien(dx, dy));
            h.TransArr.Add(new TransClass().tinhtien(dx, dy));
            Repaint();
        }
        //
        //Ham ve hinh
        //
        private Point[] DDA_line(Line l, Color color)
        {
            List<Point> list = new List<Point>();
            int dx = l.EndP.X - l.StartP.X;
            int dy = l.EndP.Y - l.StartP.Y;
            int count = Math.Abs(dx) > Math.Abs(dy) ? Math.Abs(dx) : Math.Abs(dy);
            double deltaX = (double)dx / count;
            double deltaY = (double)dy / count;
            double x = l.StartP.X, y = l.StartP.Y;
            while (count >= 0)
            {
                list.Add(PutPixel(new Point((int)Math.Round(x), (int)Math.Round(y)), color));
                x += deltaX; y += deltaY;
                count--;
            }
            return list.ToArray();
        }
        private Point[] Mid_line(Point P1, Point P2, Color color)
        {
            int x1 = P1.X, y1 = P1.Y, x2 = P2.X, y2 = P2.Y, d;
            List<Point> list = new List<Point>();
            int dx = Math.Abs(x1 - x2);
            int dy = Math.Abs(y1 - y2);
            if (dx == 0 && dy == 0) return list.ToArray();
            list.Add(PutPixel(new Point(x1, y1), color));
            if (dy < dx)
            {
                d = (dy - dx) / 2;
                do
                {
                    x1 += x1 < x2 ? 1 : -1;
                    if (d < 0) d += dy;
                    else
                    {
                        d += dy - dx;
                        y1 += y1 < y2 ? 1 : -1;
                    }
                    list.Add(PutPixel(new Point(x1, y1), color));
                } while (x1 != x2);
            }
            else
            {
                d = (dx - dy) / 2;
                do
                {
                    y1 += y1 < y2 ? 1 : -1;
                    if (d < 0) d += dx;
                    else
                    {
                        d += dx - dy;
                        x1 += x1 < x2 ? 1 : -1;
                    }
                    list.Add(PutPixel(new Point(x1, y1), color));
                } while (y1 != y2);
            }
            return list.ToArray();
        }
        private Point[] Mid_line_hide(Line l, Color color)
        {
            int x1 = l.P1.X, y1 = l.P1.Y, x2 = l.P2.X, y2 = l.P2.Y, d;
            List<Point> list = new List<Point>();
            int dx = Math.Abs(x1 - x2);
            int dy = Math.Abs(y1 - y2);
            if (dx == 0 && dy == 0) return list.ToArray();
            if (hide)
            {
                list.Add(PutPixel(new Point(x1, y1), color));
                hide = false;
            }
            else hide = true;
            if (dy < dx)
            {
                d = (dy - dx) / 2;
                do
                {
                    x1 += x1 < x2 ? 1 : -1;
                    if (d <= 0) d += dy;
                    else
                    {
                        d += dy - dx;
                        y1 += y1 < y2 ? 1 : -1;
                    }
                    if (hide)
                    {
                        list.Add(PutPixel(new Point(x1, y1), color));
                        hide = false;
                    }
                    else hide = true;
                } while (x1 != x2);
            }
            else
            {
                d = (dx - dy) / 2;
                do
                {
                    y1 += y1 < y2 ? 1 : -1;
                    if (d <= 0) d += dx;
                    else
                    {
                        d += dx - dy;
                        x1 += x1 < x2 ? 1 : -1;
                    }
                    if (hide)
                    {
                        list.Add(PutPixel(new Point(x1, y1), color));
                        hide = false;
                    }
                    else hide = true;
                } while (y1 != y2);
            }
            return list.ToArray();
        }
        private Point[] Bre_circle(Point O, int R, Color color)
        {
            List<Point> list = new List<Point>();
            int x, y, p;
            x = 0;
            y = R;
            p = 3 - 2 * R;
            while (x <= y)
            {
                list.Add(PutPixel(new Point(O.X + x, O.Y + y), color));
                list.Add(PutPixel(new Point(O.X - x, O.Y + y), color));
                list.Add(PutPixel(new Point(O.X + x, O.Y - y), color));
                list.Add(PutPixel(new Point(O.X - x, O.Y - y), color));
                list.Add(PutPixel(new Point(O.X + y, O.Y + x), color));
                list.Add(PutPixel(new Point(O.X + y, O.Y - x), color));
                list.Add(PutPixel(new Point(O.X - y, O.Y + x), color));
                list.Add(PutPixel(new Point(O.X - y, O.Y - x), color));
                if (p < 0) p += 4 * x + 6;
                else
                {
                    p += 4 * (x - y) + 10;
                    y--;
                }
                x++;
            }
            return list.ToArray();
        }
        private Point[] Mid_circle(Point O, int R, Color color)
        {
            List<Point> list = new List<Point>();
            int x, y, d;
            x = 0;
            y = R;
            d = 1 - R;
            while (x <= y)
            {
                list.Add(PutPixel(new Point(O.X + x, O.Y + y), color));
                list.Add(PutPixel(new Point(O.X - x, O.Y + y), color));
                list.Add(PutPixel(new Point(O.X + x, O.Y - y), color));
                list.Add(PutPixel(new Point(O.X - x, O.Y - y), color));
                list.Add(PutPixel(new Point(O.X + y, O.Y + x), color));
                list.Add(PutPixel(new Point(O.X + y, O.Y - x), color));
                list.Add(PutPixel(new Point(O.X - y, O.Y + x), color));
                list.Add(PutPixel(new Point(O.X - y, O.Y - x), color));
                if (d < 0) d += 2 * x + 3;
                else
                {
                    d += 2 * (x - y) + 5;
                    y--;
                }
                x++;
            }
            return list.ToArray();
        }
        private Point[] Point_Paint(Point p1, Point p2, Point p3, Point p4, Color color)
        {
            List<Point> list = new List<Point>();
            list.AddRange(Mid_line(p1, p2, color));
            list.AddRange(Mid_line(p1, p4, color));
            list.AddRange(Mid_line(p3, p2, color));
            list.AddRange(Mid_line(p3, p4, color));
            return list.ToArray();
        }
        private Point[] Triangle_Paint(Point p1, Point p2, Point p3, Color color)
        {
            List<Point> list = new List<Point>();
            list.AddRange(Mid_line(p1, p2, color));
            list.AddRange(Mid_line(p1, p3, color));
            list.AddRange(Mid_line(p2, p3, color));
            return list.ToArray();
        }
        //private Point[] Ellipse_Paint(Ellipse ell, Color color)
        //{
        //    List<Point> list = new List<Point>();
        //    int x, y;
        //    float c, p;
        //    x = 0;
        //    y = ell.B;
        //    c = (float)ell.B / ell.A;
        //    c = c * c;
        //    p = 2 * c - 2 * ell.B + 1;
        //    while (c * x <= y)
        //    {
        //        list.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y + y), color));
        //        list.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y + y), color));
        //        list.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y - y), color));
        //        list.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y - y), color));
        //        if (p < 0) p += 2 * c * (2 * x + 3);
        //        else
        //        {
        //            p += 4 * (1 - y) + 2 * c * (2 * x + 3);
        //            y--;
        //        }
        //        x++;
        //    }
        //    y = 0;
        //    x = ell.A;
        //    c = (float)ell.A / ell.B;
        //    c = c * c;
        //    p = 2 * c - 2 * ell.A + 1;
        //    while (c * y <= x)
        //    {
        //        list.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y + y), color));
        //        list.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y + y), color));
        //        list.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y - y), color));
        //        list.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y - y), color));
        //        if (p < 0) p += 2 * c * (2 * y + 3);
        //        else
        //        {
        //            p += 4 * (1 - x) + 2 * c * (2 * y + 3);
        //            x--;
        //        }
        //        y++;
        //    }
        //    return list.ToArray();
        //}
        private Point[] Ellipse_Midpoint(Ellipse ell, Color color)
        {
            List<Point> list = new List<Point>();
            //int x, y, fx, fy, a2, b2, p;
            //x = 0;
            //y = ell.B;
            //a2 = ell.A * ell.A;
            //b2 = ell.B * ell.B;
            //fx = 0;
            //fy = 2 * a2 * y;
            //l.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y + y), currentColor));
            //l.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y - y), currentColor));
            //p = (int)Math.Round(b2 - (a2 * ell.B) + (0.25 * a2));
            //while (fx < fy)
            //{
            //    x++;
            //    fx += 2 * b2;
            //    if (p < 0)
            //    {
            //        p += b2 * (2 * x + 3);
            //    }
            //    else
            //    {
            //        y--;
            //        p += b2 * (2 * x + 3) + a2 * (2 - 2 * y);
            //        fy -= 2 * a2;
            //    }
            //    l.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y + y), color));
            //    l.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y + y), color));
            //    l.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y - y), color));
            //    l.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y - y), color));
            //}
            //p = (int)Math.Round(b2 * (x + 0.5) * (x + 0.5) + a2 * (y - 1) * (y - 1) - a2 * b2);
            //while (y > 0)
            //{
            //    y--;
            //    fy -= 2 * a2;
            //    if (p >= 0)
            //    {
            //        p += a2 * (3 - 2 * y);
            //    }
            //    else
            //    {
            //        x++;
            //        fx += 2 * b2;
            //        p += b2 * (2 * x + 2) + a2 * (3 - 2 * y);
            //    }
            //    l.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y + y), color));
            //    l.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y + y), color));
            //    l.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y - y), color));
            //    l.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y - y), color));
            //}
            int x, y;
            float z1, z2, p;
            x = 0;
            y = ell.B;
            z1 = (float)(ell.B * ell.B) / (ell.A * ell.A);
            z2 = 1 / z1;
            p = 2 * z1 - 2 * ell.B + 1;
            while (z1 * ((float)x / y) <= 1)
            {
                list.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y + y), color));
                list.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y + y), color));
                list.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y - y), color));
                list.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y - y), color));
                if (p < 0)
                {
                    p = p + 2 * z1 * (2 * x + 3);
                }
                else
                {
                    p = p + 2 * z1 * (2 * x + 3) + 4 * (1 - y);
                    y--;
                }
                x++;
            }
            x = ell.A;
            y = 0;
            p = 2 * z2 - 2 * ell.A + 1;
            while (z2 * ((float)y / x) <= 1)
            {
                list.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y + y), color));
                list.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y + y), color));
                list.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y - y), color));
                list.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y - y), color));
                if (p < 0)
                {
                    p = p + 2 * z2 * (2 * y + 3);
                }
                else
                {
                    p = p + 2 * z2 * (2 * y + 3) + 4 * (1 - x);
                    x--;
                }
                y++;
            }
            return list.ToArray();
        }
        private Point[] Ellipse_Midpoint_hide(Ellipse ell, Color color)
        {
            List<Point> list = new List<Point>();
            int x, y;
            float c, p;
            x = 0;
            y = ell.B;
            c = (float)ell.B / ell.A;
            c = c * c;
            p = 2 * c - 2 * ell.B + 1;
            while (c * x <= y)
            {
                list.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y + y), color));
                list.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y + y), color));
                if (!hide)
                {
                    list.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y - y), color));
                    list.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y - y), color));
                    hide = true;
                }
                else hide = false;
                if (p < 0) p += 2 * c * (2 * x + 3);
                else
                {
                    p += 4 * (1 - y) + 2 * c * (2 * x + 3);
                    y--;
                }
                x++;
            }
            y = 0;
            x = ell.A;
            c = (float)ell.A / ell.B;
            c = c * c;
            p = 2 * c - 2 * ell.A + 1;
            while (c * y <= x)
            {
                list.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y + y), color));
                list.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y + y), color));
                if (!hide)
                {
                    list.Add(PutPixel(new Point(ell.O.X + x, ell.O.Y - y), color));
                    list.Add(PutPixel(new Point(ell.O.X - x, ell.O.Y - y), color));
                    hide = true;
                }
                else hide = false;
                if (p < 0) p += 2 * c * (2 * y + 3);
                else
                {
                    p += 4 * (1 - x) + 2 * c * (2 * y + 3);
                    x--;
                }
                y++;
            }
            return list.ToArray();
        }
        //
        //Menu 2D 3D
        //
        private void Button_2D_Click(object sender, EventArgs e)
        {
            if (D == -1 || D == (int)type.D3)
            {
                button_2D.Image = Properties.Resources._3D;
                toolStripStatusLabel12.Image = null;
                toolStripButton1.Image = Properties.Resources.Pencil;
                path = "";
                toolStripStatusLabel6.Text = "|2D|";
                InitToado2D();
                groupBox_Hinh_3D.Visible = false;
                newToolStripButton.Enabled =
                openToolStripButton.Enabled =
                saveToolStripButton.Enabled =
                saveToolStripButton1.Enabled =
                groupBox_Hinh_2D.Visible =
                toolStripButton1.Enabled =
                toolStripButton2.Enabled =
                toolStripLabel2.Enabled =
                toolStripButton6.Enabled =
                toolStripButton7.Enabled =
                toolStripButton8.Enabled =
                button1.Enabled =
                button7.Enabled =
                toolStripButton9.Enabled = true;
                D = (int)type.D2;
                Repaint();
                toolStripStatusLabel9.Text = "";
                toolStripStatusLabel7.Image = null;
                toolStripStatusLabel8.Image = null;
                mode = -1;
                modePaint = -1;
                stmp = "";
            }
            else if (D == (int)type.D2)
            {
                button_2D.Image = Properties.Resources._2D;
                toolStripStatusLabel12.Image = null;
                path = "";
                toolStripStatusLabel6.Text = "|3D|";
                InitToado3D();
                groupBox_Hinh_3D.Visible = true;
                groupBox_Hinh_2D.Visible =
                toolStripButton1.Enabled =
                toolStripButton2.Enabled =
                toolStripLabel2.Enabled =
                toolStripButton6.Enabled =
                toolStripButton7.Enabled =
                toolStripButton8.Enabled =
                button1.Enabled =
                button7.Enabled =
                toolStripButton9.Enabled = false;
                D = (int)type.D3;
                Repaint();
                NhậpLiệuToolStripMenuItem_Click(sender, e);
                toolStripStatusLabel9.Text = "";
                mode = -1;
                stmp = "";
            }
        }
        private void Button_Clear_Click(object sender, EventArgs e)
        {
            InitColorArr();
            pointCount = -1;
            pointInput = new Point[4];
            currentContext = BufferedGraphicsManager.Current;
            myBuffer = currentContext.Allocate(panel_Luoi_Pixel.CreateGraphics(), panel_Luoi_Pixel.DisplayRectangle);
            myBuffer.Graphics.Clear(panel_Luoi_Pixel.BackColor);
            toolStripStatusLabel9.Text = "";
            if (D == (int)type.D2)
            {
                hinh2D = new List<Hinh2D>();
                //Button_2D_Click(sender, e);
            }
            else if (D == (int)type.D3)
            {
                hinh3D = new List<Hinh2D>();
                //Button_3D_Click(sender, e);
            }
            Repaint();
        }
        ContextMenuStrip contextBrush = new ContextMenuStrip();
        private void Button_Brush_Click(object sender, EventArgs e)
        {
            contextBrush.Items.Clear();
            var item = new ToolStripButton();
            item.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            item.Image = Properties.Resources.brush1;
            item.Size = new System.Drawing.Size(100, 25);
            item.Text = "Nấc 1";
            item.AutoSize = false;
            contextBrush.Items.Add(item);
            var item1 = new ToolStripButton();
            item1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            item1.Image = Properties.Resources.brush2;
            item1.Size = new System.Drawing.Size(100, 25);
            item1.Text = "Nấc 2";
            item1.AutoSize = false;
            contextBrush.Items.Add(item1);
            var item2 = new ToolStripButton();
            item2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            item2.Image = Properties.Resources.brush3;
            item2.Size = new System.Drawing.Size(100, 25);
            item2.Text = "Nấc 3";
            item2.AutoSize = false;
            contextBrush.Items.Add(item2);
            var item3 = new ToolStripButton();
            item3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            item3.Image = Properties.Resources.brush4;
            item3.Size = new System.Drawing.Size(100, 25);
            item3.Text = "Nấc 4";
            item3.AutoSize = false;
            contextBrush.Items.Add(item3);
            //contextBrush.Items.Add("Nấc 1");
            //contextBrush.Items.Add("Nấc 2");
            //contextBrush.Items.Add("Nấc 3");
            //contextBrush.Items.Add("Nấc 4");
            contextBrush.ImageScalingSize = new Size(100, 30);
            contextBrush.Show(button_Brush, new System.Drawing.Point(0, button_Brush.Height));
        }
        private void ContextBrush_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Nấc 1")
            {
                currentBrush = 2f;
            }
            else if (e.ClickedItem.Text == "Nấc 2")
            {
                currentBrush = 4f;
            }
            else if (e.ClickedItem.Text == "Nấc 3")
            {
                currentBrush = 6f;
            }
            else if (e.ClickedItem.Text == "Nấc 4")
            {
                currentBrush = 8f;
            }
        }
        //
        //Click button hinh
        //
        //
        //Hinh 2D
        //
        private void Button_Diem2D_Click(object sender, EventArgs e)
        {
            mode = (int)Mode2D.point;
            toolStripStatusLabel12.Image = button_Diem2D.Image;
            if (modePaint != (int)Option.input) return;
            PointForm pF = new PointForm();
            pF.ShowDialog();
            if (pF.Point != null)
            {
                pF.Point = PutPixel(new Point(pF.Point.X, pF.Point.Y), currentColor);
                hinh2D.Add(new Hinh2D(pF.Point, pF.Point.Name));
            }
        }
        private void Button_Binh2D_Click(object sender, EventArgs e)
        {
            mode = (int)Mode2D.parall;
            toolStripStatusLabel12.Image = button_Binh2D.Image;
            if (modePaint != (int)Option.input) return;
            ParallForm paF = new ParallForm();
            paF.ShowDialog();
            if (paF.Par != null)
            {
                paF.Par.ArrPoint = Point_Paint(paF.Par.P1, paF.Par.P2, paF.Par.P3, paF.Par.P4, currentColor);
                hinh2D.Add(paF.Par);
            }
        }
        private void Button_Duong2D_Click(object sender, EventArgs e)
        {
            mode = (int)Mode2D.line;
            toolStripStatusLabel12.Image = button_Duong2D.Image;
            if (modePaint != (int)Option.input) return;
            LineForm lF = new LineForm();
            lF.ShowDialog();
            if (lF.Line != null)
            {
                if (lF.Mode == (int)ModeStyle.midpoint) lF.Line.ArrPoint = Mid_line(lF.Line.P1, lF.Line.P2, currentColor);
                else if (lF.Mode == (int)ModeStyle.bresenham) lF.Line.ArrPoint = DDA_line(lF.Line, currentColor);
                hinh2D.Add(lF.Line);
            }
        }
        private void Button_Tron2D_Click(object sender, EventArgs e)
        {
            mode = (int)Mode2D.circle;
            toolStripStatusLabel12.Image = button_Tron2D.Image;
            if (modePaint != (int)Option.input) return;
            CircleForm cF = new CircleForm();
            cF.ShowDialog();
            if (cF.Cir == null) return;
            if (cF.Mode == (int)ModeStyle.midpoint) cF.Cir.ArrPoint = Mid_circle(cF.Cir.O, cF.Cir.R, currentColor);
            else if (cF.Mode == (int)ModeStyle.bresenham) cF.Cir.ArrPoint = Bre_circle(cF.Cir.O, cF.Cir.R, currentColor);
            hinh2D.Add(cF.Cir);
        }
        private void Button_Nhat2D_Click(object sender, EventArgs e)
        {
            mode = (int)Mode2D.rectangle;
            toolStripStatusLabel12.Image = button_Nhat2D.Image;
            if (modePaint != (int)Option.input) return;
            RectangleForm rF = new RectangleForm();
            rF.ShowDialog();
            if (rF.Rec != null)
            {
                rF.Rec.ArrPoint = Point_Paint(rF.Rec.P1, rF.Rec.P2, rF.Rec.P3, rF.Rec.P4, currentColor);
                hinh2D.Add(rF.Rec);
            }
        }
        private void Button_Vuong2D_Click(object sender, EventArgs e)
        {
            mode = (int)Mode2D.square;
            toolStripStatusLabel12.Image = button_Vuong2D.Image;
            if (modePaint != (int)Option.input) return;
            SquareForm sF = new SquareForm();
            sF.ShowDialog();
            if (sF.Squa != null)
            {
                sF.Squa.ArrPoint = Point_Paint(sF.Squa.P1, sF.Squa.P2, sF.Squa.P3, sF.Squa.P4, currentColor);
                hinh2D.Add(sF.Squa);
            }
        }
        private void Button_Triangle2D_Click(object sender, EventArgs e)
        {
            mode = (int)Mode2D.triangle;
            toolStripStatusLabel12.Image = button_Triangle.Image;
            if (modePaint != (int)Option.input) return;
            TriForm tF = new TriForm();
            tF.ShowDialog();
            if (tF.Tri != null)
            {
                tF.Tri.ArrPoint = Triangle_Paint(tF.Tri.P1, tF.Tri.P2, tF.Tri.P3, currentColor);
                hinh2D.Add(tF.Tri);
            }
        }
        private void Button_Ellipse2D_Click(object sender, EventArgs e)
        {
            mode = (int)Mode2D.ellipse;
            toolStripStatusLabel12.Image = button_Eclipse2D.Image;
            if (modePaint != (int)Option.input) return;
            EllipseForm eF = new EllipseForm();
            eF.ShowDialog();
            if (eF.Ell != null)
            {
                eF.Ell.ArrPoint = Ellipse_Midpoint(eF.Ell, currentColor);
                hinh2D.Add(eF.Ell);
            }
        }
        private void Button_Thoi2D_Click(object sender, EventArgs e)
        {
            mode = (int)Mode2D.diamond;
            toolStripStatusLabel12.Image = button_Thoi2D.Image;
            if (modePaint != (int)Option.input) return;
            DiamondForm dF = new DiamondForm();
            dF.ShowDialog();
            if (dF.Dia != null)
            {
                dF.Dia.ArrPoint = Point_Paint(new Point(2 * dF.Dia.P1.X - dF.Dia.P2.X, dF.Dia.P1.Y), new Point(dF.Dia.P1.X, dF.Dia.P2.Y), new Point(dF.Dia.P2.X, dF.Dia.P1.Y), new Point(dF.Dia.P1.X, 2 * dF.Dia.P1.Y - dF.Dia.P2.Y), dF.Dia.Color);
                hinh2D.Add(dF.Dia);
            }
        }
        private void Button_Thang2D_Click(object sender, EventArgs e)
        {
            mode = (int)Mode2D.trape;
            toolStripStatusLabel12.Image = button_Thang2D.Image;
            if (modePaint != (int)Option.input) return;
            TrapeForm tF = new TrapeForm();
            tF.ShowDialog();
            if (tF.Tra != null)
            {
                tF.Tra.ArrPoint = Point_Paint(tF.Tra.P1, tF.Tra.P2, tF.Tra.P3, tF.Tra.P4, currentColor);
                hinh2D.Add(tF.Tra);
            }
        }
        //
        //Hinh 3D
        //
        private void Button_Nhat3D_Click(object sender, EventArgs e)
        {
            mode = (int)Mode2D.rectangular;
            toolStripStatusLabel12.Image = button_Nhat2D.Image;
            modePaint = (int)Option.input;
            toolStripStatusLabel7.Image = Properties.Resources.Pencil;
            toolStripStatusLabel8.Image = nhậpLiệuToolStripMenuItem.Image;
            RectangularForm rtf = new RectangularForm();
            rtf.Text = "Rectangular";
            rtf.ShowDialog();
            if (rtf.Rtg == null) return;

            List<Point> l = new List<Point>();
            if (rtf.Rtg.Rong > 0)
            {
                //
                //Hinh binh hanh duoi
                //
                l.AddRange(Mid_line_hide(new Line(rtf.Rtg.PArr[0].GetPoint(), rtf.Rtg.PArr[1].GetPoint()), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[1].GetPoint(), rtf.Rtg.PArr[2].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[2].GetPoint(), rtf.Rtg.PArr[3].GetPoint(), currentColor));
                l.AddRange(Mid_line_hide(new Line(rtf.Rtg.PArr[3].GetPoint(), rtf.Rtg.PArr[0].GetPoint()), currentColor));
                //
                //Duong cao
                //
                l.AddRange(Mid_line_hide(new Line(rtf.Rtg.PArr[0].GetPoint(), rtf.Rtg.PArr[4].GetPoint()), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[1].GetPoint(), rtf.Rtg.PArr[5].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[2].GetPoint(), rtf.Rtg.PArr[6].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[3].GetPoint(), rtf.Rtg.PArr[7].GetPoint(), currentColor));
                //
                //Hinh binh hanh tren
                //
                l.AddRange(Mid_line(rtf.Rtg.PArr[4].GetPoint(), rtf.Rtg.PArr[5].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[5].GetPoint(), rtf.Rtg.PArr[6].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[6].GetPoint(), rtf.Rtg.PArr[7].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[7].GetPoint(), rtf.Rtg.PArr[4].GetPoint(), currentColor));
            }
            else
            {
                l.AddRange(Mid_line(rtf.Rtg.PArr[0].GetPoint(), rtf.Rtg.PArr[1].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[0].GetPoint(), rtf.Rtg.PArr[4].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[1].GetPoint(), rtf.Rtg.PArr[5].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[4].GetPoint(), rtf.Rtg.PArr[5].GetPoint(), currentColor));
                //
                //Duong cao
                //
                l.AddRange(Mid_line_hide(new Line(rtf.Rtg.PArr[0].GetPoint(), rtf.Rtg.PArr[3].GetPoint()), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[1].GetPoint(), rtf.Rtg.PArr[2].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[4].GetPoint(), rtf.Rtg.PArr[7].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[5].GetPoint(), rtf.Rtg.PArr[6].GetPoint(), currentColor));
                //
                //Hinh binh hanh tren
                //
                l.AddRange(Mid_line_hide(new Line(rtf.Rtg.PArr[3].GetPoint(), rtf.Rtg.PArr[2].GetPoint()), currentColor));
                l.AddRange(Mid_line_hide(new Line(rtf.Rtg.PArr[3].GetPoint(), rtf.Rtg.PArr[7].GetPoint()), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[2].GetPoint(), rtf.Rtg.PArr[6].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[6].GetPoint(), rtf.Rtg.PArr[7].GetPoint(), currentColor));
            }
            rtf.Rtg.ArrPoint = l.ToArray();
            hinh3D.Add(rtf.Rtg);
        }
        private void Button_Chop3D_Click(object sender, EventArgs e)
        {
            mode = (int)Mode2D.pyramid;
            toolStripStatusLabel12.Image = button_Tru3D.Image;
            modePaint = (int)Option.input;
            toolStripStatusLabel7.Image = Properties.Resources.Pencil;
            toolStripStatusLabel8.Image = nhậpLiệuToolStripMenuItem.Image;
            //mode = (int)Mode2D.cone;
            //ConeForm cf = new ConeForm();
            //cf.ShowDialog();
            //if (cf.Cone == null) return;
            //
            //ve 2 duong cao
            //
            //l.AddRange(Mid_line(new Line(new Point3D(cf.Cone.O1.X - cf.Cone.R1, cf.Cone.O1.Y, cf.Cone.O1.Z).GetPoint(), new Point3D(cf.Cone.O1.X - cf.Cone.R1, cf.Cone.O1.Y, cf.Cone.O1.Z + cf.Cone.H).GetPoint()), currentColor));
            //l.AddRange(Mid_line(new Line(new Point3D(cf.Cone.O1.X + cf.Cone.R1, cf.Cone.O1.Y, cf.Cone.O1.Z).GetPoint(), new Point3D(cf.Cone.O1.X + cf.Cone.R1, cf.Cone.O1.Y, cf.Cone.O1.Z + cf.Cone.H).GetPoint()), currentColor));
            ////
            ////ve 2 ellipse
            ////
            //l.AddRange(Ellipse_Midpoint(new Ellipse(new Point3D(cf.Cone.O1.X, cf.Cone.O1.Y, cf.Cone.O1.Z + cf.Cone.H).GetPoint(), cf.Cone.R1, 5), currentColor));
            //l.AddRange(Ellipse_Midpoint_hide(new Ellipse(cf.Cone.O1.GetPoint(), cf.Cone.R1, 5), currentColor));
            //cf.Cone.ArrPoint = l.ToArray();
            //hinh3D.Add(cf.Cone);
            //
            //Hinh binh hanh duoi
            //
            RectangularForm rtf = new RectangularForm();
            rtf.Text = "Pyramid";
            rtf.ShowDialog();
            if (rtf.Rtg == null) return;
            List<Point> l = new List<Point>();
            if (rtf.Rtg.Rong > 0)
            {
                l.AddRange(Mid_line_hide(new Line(rtf.Rtg.PArr[0].GetPoint(), rtf.Rtg.PArr[1].GetPoint()), currentColor));
                l.AddRange(Mid_line_hide(new Line(rtf.Rtg.PArr[0].GetPoint(), rtf.Rtg.PArr[4].GetPoint()), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[1].GetPoint(), rtf.Rtg.PArr[5].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[4].GetPoint(), rtf.Rtg.PArr[5].GetPoint(), currentColor));
                //
                //Duong cao
                //
                l.AddRange(Mid_line_hide(new Line(rtf.Rtg.PArr[0].GetPoint(), rtf.Rtg.PArr[3].GetPoint()), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[1].GetPoint(), rtf.Rtg.PArr[3].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[4].GetPoint(), rtf.Rtg.PArr[3].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[5].GetPoint(), rtf.Rtg.PArr[3].GetPoint(), currentColor));
            }
            else if (rtf.Rtg.Rong <= 0)
            {
                l.AddRange(Mid_line(rtf.Rtg.PArr[0].GetPoint(), rtf.Rtg.PArr[1].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[0].GetPoint(), rtf.Rtg.PArr[4].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[1].GetPoint(), rtf.Rtg.PArr[5].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[4].GetPoint(), rtf.Rtg.PArr[5].GetPoint(), currentColor));
                //
                //Duong cao
                //
                l.AddRange(Mid_line_hide(new Line(rtf.Rtg.PArr[0].GetPoint(), rtf.Rtg.PArr[3].GetPoint()), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[1].GetPoint(), rtf.Rtg.PArr[3].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[4].GetPoint(), rtf.Rtg.PArr[3].GetPoint(), currentColor));
                l.AddRange(Mid_line(rtf.Rtg.PArr[5].GetPoint(), rtf.Rtg.PArr[3].GetPoint(), currentColor));
            }
            rtf.Rtg.ArrPoint = l.ToArray();
            rtf.Rtg.Name = "Pyramid";
            hinh3D.Add(rtf.Rtg);
        }
        //
        //Ham bien doi
        //
        private void Biendoi(MouseEventArgs e)
        {
            Hinh2D shape = Checkpoint(new Point(e.X / 5, e.Y / 5));
            if (shape != null)
            {
                TransForm tF = new TransForm();
                tF.ShowDialog();
                if (tF.TransList != null)
                {
                    shape.TransArr.AddRange(tF.TransList.ToArray());
                    if (shape.Name.Equals("Point"))
                    {
                        foreach (double[,] arr in tF.TransList)
                        {
                            shape.ArrPoint = MulMatrix(shape.ArrPoint, arr);
                        }
                        shape.P1 = new Point(shape.ArrPoint[0]);
                    }
                    else if (shape.Name.Equals("Line"))
                    {
                        shape.ArrPoint = new Point[] { shape.P1, shape.P2 };
                        foreach (double[,] arr in tF.TransList)
                        {
                            shape.ArrPoint = MulMatrix(shape.ArrPoint, arr);
                        }
                        shape.P1 = new Point(shape.ArrPoint[0]);
                        shape.P2 = new Point(shape.ArrPoint[1]);
                        shape.ArrPoint = Mid_line(shape.P1, shape.P2, currentColor);
                    }
                    else if (shape.Name.Equals("Circle"))
                    {
                        shape.ArrPoint = new Point[] { shape.P1, new Point(shape.P1.X + shape.R, shape.P1.Y) };
                        foreach (double[,] arr in tF.TransList)
                        {
                            shape.ArrPoint = MulMatrix(shape.ArrPoint, arr);
                        }
                        shape.P1 = new Point(shape.ArrPoint[0]);
                        shape.R = (int)Math.Sqrt(Math.Pow(shape.ArrPoint[0].X - shape.ArrPoint[1].X, 2) + Math.Pow(shape.ArrPoint[0].Y - shape.ArrPoint[1].Y, 2));
                        shape.ArrPoint = Mid_circle(shape.P1, shape.R, currentColor);
                    }
                    else if (shape.Name.Equals("Diamond"))
                    {
                        shape.ArrPoint = new Point[] { shape.P1, shape.P2 };
                        foreach (double[,] arr in tF.TransList)
                        {
                            shape.ArrPoint = MulMatrix(shape.ArrPoint, arr);
                        }
                        shape.P1 = new Point(shape.ArrPoint[0]);
                        shape.P2 = new Point(shape.ArrPoint[1]);
                        shape.ArrPoint = Point_Paint(new Point(2 * shape.ArrPoint[0].X - shape.ArrPoint[1].X, shape.ArrPoint[0].Y), new Point(shape.ArrPoint[0].X, shape.ArrPoint[1].Y), new Point(shape.ArrPoint[1].X, shape.ArrPoint[0].Y), new Point(shape.ArrPoint[0].X, 2 * shape.ArrPoint[0].Y - shape.ArrPoint[1].Y), shape.Color);
                    }
                    else if (shape.Name.Equals("Rectangle") || shape.Name.Equals("Square") || shape.Name.Equals("Parallelogram") || shape.Name.Equals("Trapezoid"))
                    {
                        shape.ArrPoint = new Point[] { shape.P1, shape.P2, shape.P3, shape.P4 };
                        foreach (double[,] arr in tF.TransList)
                        {
                            shape.ArrPoint = MulMatrix(shape.ArrPoint, arr);
                        }
                        shape.P1 = new Point(shape.ArrPoint[0]);
                        shape.P2 = new Point(shape.ArrPoint[1]);
                        shape.P3 = new Point(shape.ArrPoint[2]);
                        shape.P4 = new Point(shape.ArrPoint[3]);
                        shape.ArrPoint = Point_Paint(shape.P1, shape.P2, shape.P3, shape.P4, currentColor);
                    }
                    else if (shape.Name.Equals("Triangle"))
                    {
                        shape.ArrPoint = new Point[] { shape.P1, shape.P2, shape.P3 };
                        foreach (double[,] arr in tF.TransList)
                        {
                            shape.ArrPoint = MulMatrix(shape.ArrPoint, arr);
                        }
                        shape.P1 = new Point(shape.ArrPoint[0]);
                        shape.P2 = new Point(shape.ArrPoint[1]);
                        shape.P3 = new Point(shape.ArrPoint[2]);
                        shape.ArrPoint = Triangle_Paint(shape.P1, shape.P2, shape.P3, currentColor);
                    }
                    else if (shape.Name.Equals("Ellipse"))
                    {
                        shape.ArrPoint = new Point[] { shape.P1, shape.P2 };
                        foreach (double[,] arr in tF.TransList)
                        {
                            shape.ArrPoint = MulMatrix(shape.ArrPoint, arr);
                        }
                        shape.P1 = new Point(shape.ArrPoint[0]);
                        shape.P2 = new Point(shape.ArrPoint[1]);
                        shape.ArrPoint = Ellipse_Midpoint(new Ellipse(shape.P1, shape.P2), currentColor);
                    }
                    Repaint();
                }
            }
        }
        private void Chucnang(MouseEventArgs e)
        {
            if (modePaint == (int)Option.eraser)
            {
                Hinh2D h = Checkpoint(new Point(e.X / 5, e.Y / 5));
                if (h == null) return;
                if (D == (int)type.D2) hinh2D.Remove(h);
                else if (D == (int)type.D3) hinh3D.Remove(h);
                toolStripStatusLabel9.Text = "";
                Repaint();
            }
            else if (modePaint == (int)Option.picker)
            {
                Color color = colorArr[e.X / 5, e.Y / 5];
                if (color != Color.Empty) currentColor = toolStripButton5.BackColor = color;
            }
            else if (modePaint == (int)Option.fill)
            {
                Hinh2D h = new Hinh2D();
                Point p = new Point(e.X / 5, e.Y / 5);
                List<Point> l = new List<Point>();
                List<Point> queue = new List<Point>();
                int x = p.X;
                int y = p.Y;
                queue.Add(p);
                while (queue.Count > 0)
                {
                    p = queue.ElementAt(0);
                    queue.Remove(queue.First());
                    if (colorArr[p.X, p.Y] == Color.Empty)
                    {
                        colorArr[p.X, p.Y] = fillColor;
                        PutPixel(new Point(p.X, p.Y), fillColor);
                        l.Add(new Point(p.X, p.Y));
                    }
                    if (0 <= p.Y - 1 && colorArr[p.X, p.Y - 1] == Color.Empty && !ExistList(queue, new Point(p.X, p.Y - 1))) queue.Add(new Point(p.X, p.Y - 1));
                    if (p.X + 1 <= 100 && colorArr[p.X + 1, p.Y] == Color.Empty && !ExistList(queue, new Point(p.X + 1, p.Y))) queue.Add(new Point(p.X + 1, p.Y));
                    if (p.Y + 1 <= 100 && colorArr[p.X, p.Y + 1] == Color.Empty && !ExistList(queue, new Point(p.X, p.Y + 1))) queue.Add(new Point(p.X, p.Y + 1));
                    if (0 <= p.X - 1 && colorArr[p.X - 1, p.Y] == Color.Empty && !ExistList(queue, new Point(p.X - 1, p.Y))) queue.Add(new Point(p.X - 1, p.Y));
                }
                h.ArrPoint = l.ToArray();
                h.Color = fillColor;
                hinh2D.Add(h);
            }
        }
        private void Vehinh(MouseEventArgs e)
        {
            switch (mode)
            {
                case (int)Mode2D.point:
                    if (Checkpoint(new Point(e.X / 5, e.Y / 5)) != null) return;
                    Point p = new Point(e.X / 5, e.Y / 5);
                    hinh2D.Add(new Hinh2D(p, p.Name));
                    PutPixel(p, currentColor);
                    break;
                case (int)Mode2D.line:
                    pointInput[++pointCount] = new Point(e.X / 5, e.Y / 5);
                    if (pointCount != 1)
                    {
                        PutPixel(pointInput[pointCount], currentColor);
                        return;
                    }
                    pointCount = -1;
                    Line l = new Line(pointInput[0], pointInput[1]);
                    l.ArrPoint = Mid_line(l.P1, l.P2, currentColor);
                    hinh2D.Add(l);
                    break;
                case (int)Mode2D.circle:
                    pointInput[++pointCount] = new Point(e.X / 5, e.Y / 5);
                    if (pointCount != 1)
                    {
                        PutPixel(pointInput[pointCount], currentColor);
                        PutColor(pointInput[0], Color.Empty);
                        return;
                    }
                    pointCount = -1;
                    Circle c = new Circle(pointInput[0], pointInput[1]);
                    c.ArrPoint = Mid_circle(c.O, c.R, currentColor);
                    hinh2D.Add(c);
                    break;
                case (int)Mode2D.rectangle:
                    pointInput[++pointCount] = new Point(e.X / 5, e.Y / 5);
                    if (pointCount != 1)
                    {
                        PutPixel(pointInput[pointCount], currentColor);
                        return;
                    }
                    Rectangle rec = new Rectangle(pointInput[pointCount--], pointInput[pointCount--]);
                    rec.ArrPoint = Point_Paint(rec.P1, rec.P2, rec.P3, rec.P4, currentColor);
                    hinh2D.Add(rec);
                    break;
                case (int)Mode2D.square:
                    pointInput[++pointCount] = new Point(e.X / 5, e.Y / 5);
                    if (pointCount != 1)
                    {
                        PutPixel(pointInput[pointCount], currentColor);
                        return;
                    }
                    pointCount = -1;
                    Square squa = new Square(pointInput[0], pointInput[1]);
                    squa.ArrPoint = Point_Paint(squa.P1, squa.P2, squa.P3, squa.P4, currentColor);
                    hinh2D.Add(squa);
                    break;
                case (int)Mode2D.triangle:
                    pointInput[++pointCount] = new Point(e.X / 5, e.Y / 5);
                    if (pointCount != 2)
                    {
                        PutPixel(pointInput[pointCount], currentColor);
                        return;
                    }
                    Triangle tri = new Triangle(pointInput[pointCount--], pointInput[pointCount--], pointInput[pointCount--]);
                    tri.ArrPoint = Triangle_Paint(tri.P1, tri.P2, tri.P3, currentColor);
                    hinh2D.Add(tri);
                    break;
                case (int)Mode2D.parall:
                    pointInput[++pointCount] = new Point(e.X / 5, e.Y / 5);
                    if (pointCount != 2)
                    {
                        PutPixel(pointInput[pointCount], currentColor);
                        return;
                    }
                    Parall par = new Parall(pointInput[pointCount--], pointInput[pointCount--], pointInput[pointCount--]);
                    par.ArrPoint = Point_Paint(par.P1, par.P2, par.P3, par.P4, currentColor);
                    hinh2D.Add(par);
                    break;
                case (int)Mode2D.ellipse:
                    pointInput[++pointCount] = new Point(e.X / 5, e.Y / 5);
                    if (pointCount != 1)
                    {
                        PutPixel(pointInput[pointCount], currentColor);
                        PutColor(pointInput[0], Color.Empty);
                        return;
                    }
                    pointCount = -1;
                    Ellipse ell = new Ellipse(pointInput[0], pointInput[1]);
                    ell.ArrPoint = Ellipse_Midpoint(ell, currentColor);
                    hinh2D.Add(ell);
                    break;
                case (int)Mode2D.diamond:
                    pointInput[++pointCount] = new Point(e.X / 5, e.Y / 5);
                    if (pointCount != 1)
                    {
                        PutPixel(pointInput[pointCount], currentColor);
                        return;
                    }
                    pointCount = -1;
                    Diamond dia = new Diamond(pointInput[0], pointInput[1]);
                    dia.ArrPoint = Point_Paint(new Point(2 * dia.P1.X - dia.P2.X, dia.P1.Y), new Point(dia.P1.X, dia.P2.Y), new Point(dia.P2.X, dia.P1.Y), new Point(dia.P1.X, 2 * dia.P1.Y - dia.P2.Y), dia.Color);
                    hinh2D.Add(dia);
                    break;
                case (int)Mode2D.trape:
                    pointInput[++pointCount] = new Point(e.X / 5, e.Y / 5);
                    if (pointCount != 3)
                    {
                        PutPixel(pointInput[pointCount], currentColor);
                        return;
                    }
                    pointCount = -1;
                    Trape tra = new Trape(pointInput[0], pointInput[1], pointInput[2], pointInput[3]);
                    tra.ArrPoint = Point_Paint(tra.P1, tra.P2, tra.P3, tra.P4, currentColor);
                    hinh2D.Add(tra);
                    break;
                default:
                    break;
            }
        }
        private void Dichuyen(MouseEventArgs e, Hinh2D h)
        {
            int dx = e.X / 5 - pTmp.X;
            int dy = e.Y / 5 - pTmp.Y;
            if (dx == 0 && dy == 0) return;
            if (h.Name.Equals("Point") || h.Name.Equals("Circle") || h.Name.Equals("Ellipse"))
            {
                h.P1.X += dx;
                h.P1.Y += dy;
            }
            else if (h.Name.Equals("Line"))
            {
                h.P1.X += dx;
                h.P1.Y += dy;
                h.P2.X += dx;
                h.P2.Y += dy;
            }
            else if (h.Name.Equals("Rectangle") || h.Name.Equals("Square") || h.Name.Equals("Parallelogram") || h.Name.Equals("Diamond") || h.Name.Equals("Trapezoid"))
            {
                h.P1.X += dx;
                h.P1.Y += dy;
                h.P2.X += dx;
                h.P2.Y += dy;
                h.P3.X += dx;
                h.P3.Y += dy;
                h.P4.X += dx;
                h.P4.Y += dy;
            }
            else if (h.Name.Equals("Triangle"))
            {
                h.P1.X += dx;
                h.P1.Y += dy;
                h.P2.X += dx;
                h.P2.Y += dy;
                h.P3.X += dx;
                h.P3.Y += dy;
            }
            h.Tam.X += dx;
            h.Tam.Y += dy;
            h.ArrPoint = MulMatrix(h.ArrPoint, new TransClass().tinhtien(dx, dy));
            h.TransArr.Add(new TransClass().tinhtien(dx, dy));
            Repaint();
        }
        private void Keotha(MouseEventArgs e)
        {
            pointInput[++pointCount] = new Point(e.X / 5, e.Y / 5);
            if (pointCount == 0)
            {
                hinh2D.Add(new Hinh2D(pointInput[0]));
                mouseDown = true;
            }
            if (pointCount == 1 && mode != (int)Mode2D.parall && mode != (int)Mode2D.triangle && mode != (int)Mode2D.trape)
            {
                Hinh2D h = new Hinh2D();
                if (mode == (int)Mode2D.point)
                {
                    h.P1 = new Point(pointInput[pointCount]);
                    h.ArrPoint = new Point[] { h.P1 };
                    h.Name = "Point";
                    pointCount = -1;
                }
                else if (mode == (int)Mode2D.line)
                {
                    h = new Line(pointInput[0], pointInput[1]);
                    h.ArrPoint = Mid_line(h.P2, h.P1, currentColor);
                }
                else if (mode == (int)Mode2D.diamond)
                {
                    h = new Diamond(pointInput[0], pointInput[1]);
                    h.ArrPoint = Point_Paint(new Point(2 * h.P1.X - h.P2.X, h.P1.Y), new Point(h.P1.X, h.P2.Y), new Point(h.P2.X, h.P1.Y), new Point(h.P1.X, 2 * h.P1.Y - h.P2.Y), currentColor);
                }
                else if (mode == (int)Mode2D.rectangle || mode == (int)Mode2D.square)
                {
                    if (mode == (int)Mode2D.rectangle)
                    {
                        h = new Rectangle(pointInput[0], pointInput[1]);
                    }
                    else if (mode == (int)Mode2D.square)
                    {
                        h = new Square(pointInput[0], pointInput[1]);
                    }
                    h.ArrPoint = Point_Paint(h.P1, h.P2, h.P3, h.P4, currentColor);
                }
                else if (mode == (int)Mode2D.circle)
                {
                    h = new Circle(pointInput[0], pointInput[1]);
                    h.ArrPoint = Mid_circle(h.P1, h.R, currentColor);
                }
                else if (mode == (int)Mode2D.ellipse)
                {
                    h = new Ellipse(pointInput[0], pointInput[1]);
                    h.ArrPoint = Ellipse_Midpoint(new Ellipse(h.P1, h.P2), currentColor);
                }
                hinh2D.Remove(hinh2D.Last());
                hinh2D.Add(h);
                mouseDown = false;
                pointCount = -1;
            }
            else if (pointCount == 2 && mode != (int)Mode2D.trape)
            {
                Hinh2D h = new Hinh2D();
                if (mode == (int)Mode2D.parall)
                {
                    h = new Parall(pointInput[0], pointInput[1], pointInput[2]);
                    h.ArrPoint = Point_Paint(h.P1, h.P2, h.P3, h.P4, currentColor);
                }
                else if (mode == (int)Mode2D.triangle)
                {
                    h = new Triangle(pointInput[0], pointInput[1], pointInput[2]);
                    h.ArrPoint = Triangle_Paint(h.P1, h.P2, h.P3, currentColor);
                }
                hinh2D.Remove(hinh2D.Last());
                hinh2D.Add(h);
                mouseDown = false;
                pointCount = -1;
            }
            else if (pointCount == 3)
            {
                Hinh2D h = new Hinh2D();
                if (mode == (int)Mode2D.trape)
                {
                    h = new Trape(pointInput[0], pointInput[1], pointInput[2], pointInput[3]);
                    h.ArrPoint = Point_Paint(h.P1, h.P2, h.P3, h.P4, currentColor);
                }
                hinh2D.Remove(hinh2D.Last());
                hinh2D.Add(h);
                mouseDown = false;
                pointCount = -1;
            }
            Repaint();
        }
        private bool ExistList(List<Point> l, Point p)
        {
            foreach (Point token in l)
            {
                if (token.X == p.X && token.Y == p.Y) return true;
            }
            return false;
        }
        private void Scaling(MouseEventArgs e, Hinh2D h)
        {
            double dx = e.X / 5 - pTmp.X;
            double dy = e.Y / 5 - pTmp.Y;
            double d = dx >= 1 ? 1.1 : 0.9;
            TransClass transclass = new TransClass();
            if (h.Name.Equals("Circle"))
            {
                h.R = (int)Math.Round(Math.Sqrt(Math.Pow(e.X / 5 - h.P1.X, 2) + Math.Pow(e.Y / 5 - h.P1.Y, 2)));
                h.ArrPoint = Mid_circle(h.P1, h.R, h.Color);
            }
            else if (h.Name.Equals("Ellipse"))
            {
                Point[] arr = MulMatrix(new Point[] { h.P2 }, transclass.tinhtien(-h.P1.X, -h.P1.Y));
                arr = MulMatrix(arr, transclass.tile(d, d));
                arr = MulMatrix(arr, transclass.tinhtien(h.P1.X, h.P1.Y));
                h.P2 = arr[0];
                h.ArrPoint = Ellipse_Midpoint(new Ellipse(h.P1, h.P2), currentColor);
            }
            else if (h.Name.Equals("Line"))
            {
                Point[] arr = MulMatrix(new Point[] { h.P1, h.P2 }, transclass.tinhtien(-Math.Round((double)(h.P1.X + h.P2.X) / 2), -Math.Round((double)(h.P1.Y + h.P2.Y) / 2)));
                arr = MulMatrix(arr, transclass.tile(d, d));
                arr = MulMatrix(arr, transclass.tinhtien((h.P1.X + h.P2.X) / 2, (h.P1.Y + h.P2.Y) / 2));
                h.P1 = arr[0];
                h.P2 = arr[1];
                h.ArrPoint = Mid_line(h.P2, h.P1, currentColor);
            }
            else if (h.Name.Equals("Rectangle") || h.Name.Equals("Square"))
            {
                Point[] arr = MulMatrix(new Point[] { h.P1, h.P3 }, transclass.tinhtien(-Math.Round((double)(h.P1.X + h.P3.X) / 2), -Math.Round((double)(h.P1.Y + h.P3.Y) / 2)));
                arr = MulMatrix(arr, transclass.tile(d, d));
                arr = MulMatrix(arr, transclass.tinhtien((h.P1.X + h.P3.X) / 2, (h.P1.Y + h.P3.Y) / 2));
                h.P1 = arr[0];
                h.P3 = arr[1];
                h.P2 = new Point(h.P1.X, h.P3.Y);
                h.P4 = new Point(h.P3.X, h.P1.Y);
                h.ArrPoint = Point_Paint(h.P1, h.P2, h.P3, h.P4, currentColor);
            }
            else if (h.Name.Equals("Parallelogram"))
            {
                Point[] arr = MulMatrix(new Point[] { h.P1, h.P2, h.P3 }, transclass.tinhtien(-Math.Round((double)(h.P1.X + h.P3.X) / 2), -Math.Round((double)(h.P1.Y + h.P3.Y) / 2)));
                arr = MulMatrix(arr, transclass.tile(d, d));
                arr = MulMatrix(arr, transclass.tinhtien(h.Tam.X, h.Tam.Y));
                h.P1 = arr[0];
                h.P2 = arr[1];
                h.P3 = arr[2];
                h.P4 = new Point(h.P1.X + h.P3.X - h.P2.X, h.P3.Y + h.P1.Y - h.P2.Y);
                h.ArrPoint = Point_Paint(h.P1, h.P2, h.P3, h.P4, currentColor);
            }
            else if (h.Name.Equals("Triangle"))
            {
                Point[] arr = MulMatrix(new Point[] { h.P1, h.P2, h.P3 }, transclass.tinhtien(-Math.Round((double)(h.P1.X + h.P2.X + h.P3.X) / 3), -Math.Round((double)h.P1.Y + h.P2.Y + h.P3.Y) / 3));
                arr = MulMatrix(arr, transclass.tile(d, d));
                arr = MulMatrix(arr, transclass.tinhtien(h.Tam.X, h.Tam.Y));
                h.P1 = arr[0];
                h.P2 = arr[1];
                h.P3 = arr[2];
                h.ArrPoint = Triangle_Paint(h.P1, h.P2, h.P3, currentColor);
            }
            else if (h.Name.Equals("Diamond"))
            {
                Point[] arr = MulMatrix(new Point[] { h.P2 }, transclass.tinhtien(-h.Tam.X, -h.Tam.Y));
                arr = MulMatrix(arr, transclass.tile(d, d));
                arr = MulMatrix(arr, transclass.tinhtien(h.Tam.X, h.Tam.Y));
                h.P2 = arr[0];
                h.ArrPoint = Point_Paint(new Point(2 * h.Tam.X - h.P2.X, h.Tam.Y), new Point(h.Tam.X, h.P2.Y), new Point(h.P2.X, h.Tam.Y), new Point(h.Tam.X, 2 * h.Tam.Y - h.P2.Y), currentColor);
            }
            else if (h.Name.Equals("Trapezoid"))
            {
                Point[] arr = MulMatrix(new Point[] { h.P1, h.P2, h.P3, h.P4 }, transclass.tinhtien(-Math.Round((double)(h.P1.X + h.P3.X) / 2), -Math.Round((double)(h.P1.Y + h.P3.Y) / 2)));
                arr = MulMatrix(arr, transclass.tile(d, d));
                arr = MulMatrix(arr, transclass.tinhtien(h.Tam.X, h.Tam.Y));
                h.P1 = arr[0];
                h.P2 = arr[1];
                h.P3 = arr[2];
                h.P4 = arr[3];
                h.ArrPoint = Point_Paint(h.P1, h.P2, h.P3, h.P4, currentColor);
            }
            Repaint();
        }
        //
        //Su kien luoi Pixel
        //
        private void Panel_Luoi_Pixel_MouseClick(object sender, MouseEventArgs e)
        {
            if (modePaint == (int)Option.choose)
            {
                xmin = ymin = 102;
                xmax = ymax = -1;
                hchoose = Checkpoint(new Point(e.X / 5, e.Y / 5));
                if (hchoose != null)
                {
                    Repaint();
                    foreach (Point p in hchoose.ArrPoint)
                    {
                        if (p.X < xmin) xmin = p.X;
                        if (p.X > xmax) xmax = p.X;
                        if (p.Y < ymin) ymin = p.Y;
                        if (p.Y > ymax) ymax = p.Y;
                    }
                    Danhdau(new Point(xmin - 1, ymin - 1), new Point(xmax + 1, ymin - 1), new Point(xmax + 1, ymax + 1), new Point(xmin - 1, ymax + 1), Color.Blue);
                }
                else Repaint();
            }
            if (modePaint == -1)
            {
                MessageBox.Show("Hãy chọn kiểu vẽ", "Gợi ý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (modePaint == (int)Option.transform)
            {
                Biendoi(e);
                return;
            }
            else if (modePaint == (int)Option.eraser || modePaint == (int)Option.picker || modePaint == (int)Option.fill)
            {
                Chucnang(e);
                return;
            }
            if (modePaint == (int)Option.point || modePaint == (int)Option.drag || modePaint == (int)Option.input)
            {
                if (mode == -1)
                {
                    MessageBox.Show("Hãy chọn hình vẽ", "Gợi ý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (modePaint == (int)Option.point)
                {
                    Vehinh(e);
                }
                else if (modePaint == (int)Option.drag)
                {
                    Keotha(e);
                }
            }
        }
        Point pTmp;
        Hinh2D hDraw;
        private void Panel_Luoi_Pixel_MouseDown(object sender, MouseEventArgs e)
        {
            pTmp = new Point(e.X / 5, e.Y / 5);
            if (modePaint == (int)Option.move)
            {
                mouseDown = true;
                pointInput[0] = new Point(e.X / 5, e.Y / 5);
            }
            else if (modePaint == (int)Option.scaling || modePaint == (int)Option.draw)
            {
                mouseDown = true;
                l = new List<Point>();
            }
        }
        Hinh2D h;
        private void Panel_Luoi_Pixel_MouseUp(object sender, MouseEventArgs e)
        {
            if (modePaint == (int)Option.draw)
            {
                mouseDown = false;
                hDraw = new Hinh2D();
                hDraw.ArrPoint = l.ToArray();
                hinh2D.Add(hDraw);
            }
            else if (modePaint == (int)Option.drag) return;
            h = null;
            mouseDown = false;
        }
        string stmp = "";
        private void Panel_Luoi_Pixel_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel1.Text = e.X + ":" + e.Y + " px";
            toolStripStatusLabel2.Text = "Toạ độ: (" + (e.X - size_LuoiPixel / 2) / 5 + "," + -(e.Y - size_LuoiPixel / 2) / 5 + ")";
            Hinh2D hInfo = null;
            if (modePaint != (int)Option.choose || hchoose == null) hInfo = Checkpoint(new Point(e.X / 5, e.Y / 5));
            if (hInfo != null)
            {
                if (!hInfo.toString().Equals(stmp))
                {
                    toolStripStatusLabel9.Text = stmp = hInfo.toString();
                }
            }
            if (pTmp != null)
            {
                if (e.X / 5 == pTmp.X && e.Y / 5 == pTmp.Y) return;
            }
            if (mouseDown && modePaint == (int)Option.drag)
            {
                Point p = new Point(e.X / 5, e.Y / 5);
                if (mode == (int)Mode2D.point)
                {
                    hinh2D.Last().ArrPoint = new Point[] { p };
                }
                else if (mode == (int)Mode2D.line)
                {
                    hinh2D.Last().ArrPoint = Mid_line(hinh2D.Last().P1, p, currentColor);
                }
                else if (mode == (int)Mode2D.circle)
                {
                    hinh2D.Last().ArrPoint = Mid_circle(hinh2D.Last().P1, (int)Math.Sqrt((hinh2D.Last().P1.X - p.X) * (hinh2D.Last().P1.X - p.X) + (hinh2D.Last().P1.Y - p.Y) * (hinh2D.Last().P1.Y - p.Y)), currentColor);
                }
                else if (mode == (int)Mode2D.ellipse)
                {
                    hinh2D.Last().ArrPoint = Ellipse_Midpoint(new Ellipse(hinh2D.Last().P1, p), currentColor);
                }
                else if (mode == (int)Mode2D.trape)
                {
                    List<Point> ltmp = new List<Point>();
                    if (pointCount == 0) ltmp.AddRange(Mid_line(pointInput[pointCount], p, currentColor));
                    else if (pointCount == 1)
                    {
                        ltmp.AddRange(Mid_line(pointInput[pointCount], pointInput[pointCount - 1], currentColor));
                        ltmp.AddRange(Mid_line(pointInput[pointCount], new Point(p.X, pointInput[pointCount].Y), currentColor));
                    }
                    else
                    {
                        ltmp.AddRange(Point_Paint(pointInput[pointCount - 2], pointInput[pointCount - 1], new Point(pointInput[pointCount].X, pointInput[pointCount - 1].Y), new Point(p.X, pointInput[pointCount - 2].Y), currentColor));
                    }
                    hinh2D.Last().ArrPoint = ltmp.ToArray();
                }
                else if (mode == (int)Mode2D.triangle)
                {
                    List<Point> ltmp = new List<Point>();
                    if (pointCount == 0) ltmp.AddRange(Mid_line(p, pointInput[pointCount], currentColor));
                    else ltmp.AddRange(Triangle_Paint(p, pointInput[pointCount], pointInput[pointCount - 1], currentColor));
                    hinh2D.Last().ArrPoint = ltmp.ToArray();
                }
                else if (mode == (int)Mode2D.parall)
                {
                    List<Point> ltmp = new List<Point>();
                    if (pointCount == 0) ltmp.AddRange(Mid_line(pointInput[pointCount], p, currentColor));
                    else ltmp.AddRange(Point_Paint(pointInput[pointCount - 1], pointInput[pointCount], p, new Point(pointInput[pointCount - 1].X + p.X - pointInput[pointCount].X, p.Y + pointInput[pointCount - 1].Y - pointInput[pointCount].Y), currentColor));
                    hinh2D.Last().ArrPoint = ltmp.ToArray();
                }
                else if (mode == (int)Mode2D.diamond)
                {
                    Hinh2D h = new Diamond(hinh2D.Last().P1, new Point(e.X / 5, e.Y / 5));
                    hinh2D.Last().ArrPoint = Point_Paint(new Point(2 * h.P1.X - h.P2.X, h.P1.Y), new Point(h.P1.X, h.P2.Y), new Point(h.P2.X, h.P1.Y), new Point(h.P1.X, 2 * h.P1.Y - h.P2.Y), currentColor);
                }
                else if (mode == (int)Mode2D.rectangle || mode == (int)Mode2D.square)
                {
                    Hinh2D h = new Hinh2D();
                    if (mode == (int)Mode2D.rectangle)
                    {
                        h = new Rectangle(hinh2D.Last().P1, p);
                    }
                    else if (mode == (int)Mode2D.square)
                    {
                        h = new Square(hinh2D.Last().P1, new Point(e.X / 5, e.Y / 5));
                    }
                    hinh2D.Last().ArrPoint = Point_Paint(h.P1, h.P2, h.P3, h.P4, currentColor);
                }
                Repaint();
            }
            else if (mouseDown && modePaint == (int)Option.move)
            {
                if (h != null)
                {
                    Dichuyen(e, h);
                }
                else h = Checkpoint(new Point(e.X / 5, e.Y / 5));
            }
            else if (mouseDown && modePaint == (int)Option.scaling)
            {
                if (h != null)
                {
                    Scaling(e, h);
                }
                else h = Checkpoint(new Point(e.X / 5, e.Y / 5));
            }
            else if (mouseDown && modePaint == (int)Option.draw)
            {
                l.Add(PutPixelBrush(new Point(e.X / 5, e.Y / 5), currentColor, CurrentBrush));
            }
            pTmp = new Point(e.X / 5, e.Y / 5);
        }
        //
        //Set up mode va modepaint
        //
        private void Button7_Click(object sender, EventArgs e)
        {
            modePaint = (int)Option.choose;
            if (mode != -1 && modePaint != -1) Repaint();
            toolStripStatusLabel7.Image = button7.Image;
            toolStripStatusLabel8.Image = null;
            hchoose = null;
        }
        private void ToolStripButton5_Click(object sender, EventArgs e)
        {
            ColorDialog colordialog = new ColorDialog();
            if (colordialog.ShowDialog() == DialogResult.OK)
            {
                currentColor = toolStripButton5.BackColor = colordialog.Color;
            }
        }
        private void NewToolStripButton_Click(object sender, EventArgs e)
        {
            if (!Saveornot())
            {
                InitColorArr();
                pointCount = -1;
                pointInput = new Point[4];
                currentContext = BufferedGraphicsManager.Current;
                myBuffer = currentContext.Allocate(panel_Luoi_Pixel.CreateGraphics(), panel_Luoi_Pixel.DisplayRectangle);
                myBuffer.Graphics.Clear(panel_Luoi_Pixel.BackColor);
                toolStripStatusLabel9.Text = "";
                if (D == (int)type.D2)
                {
                    hinh2D = new List<Hinh2D>();
                    D = (int)type.D3;
                }
                else if (D == (int)type.D3)
                {
                    hinh3D = new List<Hinh2D>();
                    D = (int)type.D2;
                }
                Button_2D_Click(sender, e);
            }
            else
            {
                DialogResult dlr = MessageBox.Show("Do you want to save changes?", "Paint", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dlr == DialogResult.Yes) SaveToolStripButton_Click(sender, e);
                else if (dlr == DialogResult.No) Button_Clear_Click(sender, e);
            }
        }
        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            if (D == (int)type.D2 && hinh2D.Count == 0) return;
            else if (D == (int)type.D3 && hinh3D.Count == 0) return;
            if (path == "")
            {
                SaveFileDialog savedl = new SaveFileDialog();
                savedl.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Object Image|*.obj";
                savedl.Title = "Save as";
                savedl.ShowDialog();
                if (savedl.FileName == "") return;
                path = savedl.FileName;
            }
            if (path.Contains(".obj"))
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                if (D == (int)type.D2) bf.Serialize(fs, hinh2D);
                else if (D == (int)type.D3) bf.Serialize(fs, hinh3D);
                fs.Close();
            }
            else
            {
                Bitmap bmp = new Bitmap(panel_Luoi_Pixel.Width, panel_Luoi_Pixel.Height);
                Graphics g = Graphics.FromImage(bmp);
                myBuffer.Render(g);
                bmp.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
            }
            if (D == (int)type.D2) hinhTmp = new List<Hinh2D>(hinh2D);
            else if (D == (int)type.D3) hinhTmp = new List<Hinh2D>(hinh3D);
        }
        private void SaveasToolStripButton_Click(object sender, EventArgs e)
        {
            if (D == (int)type.D2 && hinh2D.Count == 0) return;
            else if (D == (int)type.D3 && hinh3D.Count == 0) return;
            SaveFileDialog savedl = new SaveFileDialog();
            savedl.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Object Image|*.obj";
            savedl.Title = "Save as";
            savedl.ShowDialog();
            if (savedl.FileName == "") return;
            path = savedl.FileName;
            SaveToolStripButton_Click(sender, e);
        }
        private void OpenToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog opendl = new OpenFileDialog();
            opendl.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Object Image|*.obj";
            opendl.Title = "Mở ảnh";
            opendl.ShowDialog();
            if (opendl.FileName == "") return;
            if (opendl.FileName.Contains(".obj"))
            {
                FileStream fs = new FileStream(opendl.FileName, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                hinh2D = (List<Hinh2D>)bf.Deserialize(fs);
                fs.Close();
                Repaint();
            }
            else
            {
                Bitmap bm = new Bitmap(opendl.FileName);
                Graphics g = Graphics.FromImage(bm);
                myBuffer.Graphics.DrawImage(bm, 0, 0, bm.Width, bm.Height);
                myBuffer.Render();
            }
        }
        private void ChấmĐiểmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modePaint = (int)Option.point;
            toolStripStatusLabel7.Image = Properties.Resources.Pencil;
            toolStripStatusLabel8.Image = chấmĐiểmToolStripMenuItem.Image;
            toolStripButton1.Image = chấmĐiểmToolStripMenuItem.Image;
        }
        private void KéoThảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modePaint = (int)Option.drag;
            toolStripStatusLabel7.Image = Properties.Resources.Pencil;
            toolStripStatusLabel8.Image = kéoThảToolStripMenuItem.Image;
            toolStripButton1.Image = kéoThảToolStripMenuItem.Image;
        }
        private void NhậpLiệuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modePaint = (int)Option.input;
            mode = -1;
            toolStripStatusLabel7.Image = Properties.Resources.Pencil;
            toolStripStatusLabel8.Image = nhậpLiệuToolStripMenuItem.Image;
            toolStripButton1.Image = nhậpLiệuToolStripMenuItem.Image;
        }
        private void ToolStripButton3_Click(object sender, EventArgs e)
        {
            modePaint = (int)Option.eraser;
            toolStripStatusLabel7.Image = toolStripButton3.Image;
            toolStripStatusLabel8.Image = null;
        }
        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            modePaint = (int)Option.fill;
            toolStripStatusLabel7.Image = toolStripButton2.Image;
            toolStripStatusLabel8.Image = null;
        }
        private void ToolStripButton4_Click(object sender, EventArgs e)
        {
            modePaint = (int)Option.picker;
            toolStripStatusLabel7.Image = toolStripButton4.Image;
            toolStripStatusLabel8.Image = null;
        }
        private void ToolStripLabel2_Click(object sender, EventArgs e)
        {
            modePaint = (int)Option.transform;
            toolStripStatusLabel7.Image = toolStripLabel2.Image;
            toolStripStatusLabel8.Image = null;
        }
        private void ToolStripButton6_Click(object sender, EventArgs e)
        {
            modePaint = (int)Option.move;
            toolStripStatusLabel7.Image = toolStripButton6.Image;
            toolStripStatusLabel8.Image = null;
        }
        private void ToolStripButton9_Click(object sender, EventArgs e)
        {
            ColorDialog colordialog = new ColorDialog();
            if (colordialog.ShowDialog() == DialogResult.OK)
            {
                fillColor = toolStripButton9.BackColor = colordialog.Color;
            }
        }
        private void ToolStripButton7_Click(object sender, EventArgs e)
        {
            modePaint = (int)Option.scaling;
            toolStripStatusLabel7.Image = toolStripButton7.Image;
            toolStripStatusLabel8.Image = null;
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (toolStripStatusLabel9.Text == "") return;
            char ch = toolStripStatusLabel9.Text.ElementAt(0);
            toolStripStatusLabel9.Text = toolStripStatusLabel9.Text.Remove(0, 1);
            toolStripStatusLabel9.Text += ch;
        }
        ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
        private void Button1_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Items.Clear();
            contextMenuStrip1.Items.Add("Xoay phải 90°", Properties.Resources.rotate90);
            contextMenuStrip1.Items.Add("Xoay trái 90°", Properties.Resources.rotate_90);
            contextMenuStrip1.Items.Add("Xoay 180°", Properties.Resources.rotate180);
            contextMenuStrip1.Items.Add("Đối xứng O", Properties.Resources.dxO);
            contextMenuStrip1.Items.Add("Đối xứng Ox", Properties.Resources.dxOx);
            contextMenuStrip1.Items.Add("Đối xứng Oy", Properties.Resources.dxOy);
            contextMenuStrip1.Show(button1, new System.Drawing.Point(0, button1.Height));
        }
        private void ContextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            DialogResult dr = new DialogResult();
            if (modePaint != (int)Option.choose || hchoose == null) dr = MessageBox.Show("Chọn hình để biến đổi", "Gợi ý", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (dr == DialogResult.Cancel) return;
            if (dr == DialogResult.OK) Button7_Click(sender, new EventArgs());
            if (hchoose == null) return;
            List<double[,]> transList = new List<double[,]>();
            TransClass transclass = new TransClass();
            if (e.ClickedItem.Text == "Xoay phải 90°")
            {
                transList.Add(transclass.tinhtien(-hchoose.Tam.X, -hchoose.Tam.Y));
                transList.Add(transclass.quay(90 * (Math.PI / 180)));
                transList.Add(transclass.tinhtien(hchoose.Tam.X, hchoose.Tam.Y));
            }
            else if (e.ClickedItem.Text == "Xoay trái 90°")
            {
                transList.Add(transclass.tinhtien(-hchoose.Tam.X, -hchoose.Tam.Y));
                transList.Add(transclass.quay(-90 * (Math.PI / 180)));
                transList.Add(transclass.tinhtien(hchoose.Tam.X, hchoose.Tam.Y));
            }
            else if (e.ClickedItem.Text == "Xoay 180°")
            {
                transList.Add(transclass.tinhtien(-hchoose.Tam.X, -hchoose.Tam.Y));
                transList.Add(transclass.quay(180 * (Math.PI / 180)));
                transList.Add(transclass.tinhtien(hchoose.Tam.X, hchoose.Tam.Y));
            }
            else if (e.ClickedItem.Text == "Đối xứng O")
            {
                transList.Add(transclass.tinhtien(-50, -50));
                transList.Add(transclass.doixungO());
                transList.Add(transclass.tinhtien(50, 50));
            }
            else if (e.ClickedItem.Text == "Đối xứng Ox")
            {
                transList.Add(transclass.tinhtien(-50, -50));
                transList.Add(transclass.doixungOx());
                transList.Add(transclass.tinhtien(50, 50));
            }
            else if (e.ClickedItem.Text == "Đối xứng Oy")
            {
                transList.Add(transclass.tinhtien(-50, -50));
                transList.Add(transclass.doixungOy());
                transList.Add(transclass.tinhtien(50, 50));
            }
            hchoose.TransArr.AddRange(transList.ToArray());
            if (hchoose.Name.Equals("Line"))
            {
                hchoose.ArrPoint = new Point[] { hchoose.P1, hchoose.P2, hchoose.Tam };
                foreach (double[,] arr in transList)
                {
                    hchoose.ArrPoint = MulMatrix(hchoose.ArrPoint, arr);
                }
                hchoose.P1 = new Point(hchoose.ArrPoint[0]);
                hchoose.P2 = new Point(hchoose.ArrPoint[1]);
                hchoose.Tam = new Point(hchoose.ArrPoint[2]);
                hchoose.ArrPoint = Mid_line(hchoose.P1, hchoose.P2, hchoose.Color);
            }
            else if (hchoose.Name.Equals("Circle"))
            {
                hchoose.ArrPoint = new Point[] { hchoose.P1, new Point(hchoose.P1.X + hchoose.R, hchoose.P1.Y), hchoose.Tam };
                foreach (double[,] arr in transList)
                {
                    hchoose.ArrPoint = MulMatrix(hchoose.ArrPoint, arr);
                }
                hchoose.P1 = new Point(hchoose.ArrPoint[0]);
                hchoose.R = (int)Math.Sqrt(Math.Pow(hchoose.ArrPoint[0].X - hchoose.ArrPoint[1].X, 2) + Math.Pow(hchoose.ArrPoint[0].Y - hchoose.ArrPoint[1].Y, 2));
                hchoose.Tam = new Point(hchoose.ArrPoint[2]);
                hchoose.ArrPoint = Mid_circle(hchoose.P1, hchoose.R, hchoose.Color);
            }
            else if (hchoose.Name.Equals("Rectangle") || hchoose.Name.Equals("Square"))
            {
                hchoose.ArrPoint = new Point[] { hchoose.P1, hchoose.P3, hchoose.Tam };
                foreach (double[,] arr in transList)
                {
                    hchoose.ArrPoint = MulMatrix(hchoose.ArrPoint, arr);
                }
                hchoose.P1 = new Point(hchoose.ArrPoint[0]);
                hchoose.P2 = new Point(hchoose.ArrPoint[0].X, hchoose.ArrPoint[1].Y);
                hchoose.P3 = new Point(hchoose.ArrPoint[1]);
                hchoose.P4 = new Point(hchoose.ArrPoint[1].X, hchoose.ArrPoint[0].Y);
                hchoose.Tam = new Point(hchoose.ArrPoint[2]);
                hchoose.ArrPoint = Point_Paint(hchoose.P1, hchoose.P2, hchoose.P3, hchoose.P4, hchoose.Color);
            }
            else if (hchoose.Name.Equals("Diamond"))
            {
                hchoose.ArrPoint = new Point[] { hchoose.P1, hchoose.P2, hchoose.Tam };
                foreach (double[,] arr in transList)
                {
                    hchoose.ArrPoint = MulMatrix(hchoose.ArrPoint, arr);
                }
                hchoose.P1 = hchoose.ArrPoint[0];
                hchoose.P2 = hchoose.ArrPoint[1];
                hchoose.Tam = new Point(hchoose.ArrPoint[2]);
                hchoose.ArrPoint = Point_Paint(new Point(2 * hchoose.ArrPoint[0].X - hchoose.ArrPoint[1].X, hchoose.ArrPoint[0].Y), new Point(hchoose.ArrPoint[0].X, hchoose.ArrPoint[1].Y), new Point(hchoose.ArrPoint[1].X, hchoose.ArrPoint[0].Y), new Point(hchoose.ArrPoint[0].X, 2 * hchoose.ArrPoint[0].Y - hchoose.ArrPoint[1].Y), hchoose.Color);
            }
            else if (hchoose.Name.Equals("Parallelogram") || hchoose.Name.Equals("Trapezoid"))
            {
                hchoose.ArrPoint = new Point[] { hchoose.P1, hchoose.P2, hchoose.P3, hchoose.P4, hchoose.Tam };
                foreach (double[,] arr in transList)
                {
                    hchoose.ArrPoint = MulMatrix(hchoose.ArrPoint, arr);
                }
                hchoose.P1 = new Point(hchoose.ArrPoint[0]);
                hchoose.P2 = new Point(hchoose.ArrPoint[1]);
                hchoose.P3 = new Point(hchoose.ArrPoint[2]);
                hchoose.P4 = new Point(hchoose.ArrPoint[3]);
                hchoose.Tam = new Point(hchoose.ArrPoint[4]);
                hchoose.ArrPoint = Point_Paint(hchoose.P1, hchoose.P2, hchoose.P3, hchoose.P4, currentColor);
            }
            else if (hchoose.Name.Equals("Triangle"))
            {
                hchoose.ArrPoint = new Point[] { hchoose.P1, hchoose.P2, hchoose.P3, hchoose.Tam };
                foreach (double[,] arr in transList)
                {
                    hchoose.ArrPoint = MulMatrix(hchoose.ArrPoint, arr);
                }
                hchoose.P1 = new Point(hchoose.ArrPoint[0]);
                hchoose.P2 = new Point(hchoose.ArrPoint[1]);
                hchoose.P3 = new Point(hchoose.ArrPoint[2]);
                hchoose.Tam = new Point(hchoose.ArrPoint[3]);
                hchoose.ArrPoint = Triangle_Paint(hchoose.P1, hchoose.P2, hchoose.P3, currentColor);
            }
            else if (hchoose.Name.Equals("Ellipse"))
            {
                hchoose.ArrPoint = new Point[] { hchoose.P1, hchoose.P2, hchoose.Tam };
                foreach (double[,] arr in transList)
                {
                    hchoose.ArrPoint = MulMatrix(hchoose.ArrPoint, arr);
                }
                hchoose.P1 = new Point(hchoose.ArrPoint[0]);
                hchoose.P2 = new Point(hchoose.ArrPoint[1]);
                hchoose.Tam = new Point(hchoose.ArrPoint[2]);
                hchoose.ArrPoint = Ellipse_Midpoint(new Ellipse(hchoose.P1, hchoose.P2), currentColor);
            }
            Repaint();
            xmin = ymin = 102;
            xmax = ymax = -1;
            foreach (Point p in hchoose.ArrPoint)
            {
                if (p.X < xmin) xmin = p.X;
                if (p.X > xmax) xmax = p.X;
                if (p.Y < ymin) ymin = p.Y;
                if (p.Y > ymax) ymax = p.Y;
            }
            Danhdau(new Point(xmin - 1, ymin - 1), new Point(xmax + 1, ymin - 1), new Point(xmax + 1, ymax + 1), new Point(xmin - 1, ymax + 1), Color.Blue);
            toolStripStatusLabel9.Text = stmp = hchoose.toString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            modePaint = (int)Option.draw;
            toolStripStatusLabel7.Image = button2.Image;
            toolStripStatusLabel8.Image =
            toolStripStatusLabel12.Image = null;
        }

        private void ToolStripButton1_ButtonClick(object sender, EventArgs e)
        {
            if (toolStripButton1.Image == chấmĐiểmToolStripMenuItem.Image) ChấmĐiểmToolStripMenuItem_Click(sender, e);
            else if (toolStripButton1.Image == kéoThảToolStripMenuItem.Image) KéoThảToolStripMenuItem_Click(sender, e);
            else if (toolStripButton1.Image == nhậpLiệuToolStripMenuItem.Image) NhậpLiệuToolStripMenuItem_Click(sender, e);
        }
        private void InfoToolStripButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Trần Hoàng Minh  N14DCCN116\nĐặng Hoàng Phúc N14DCCN085\nNguyễn Duy Bình  N14DCCN077", "Thông tin nhóm 9", MessageBoxButtons.OK);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (modePaint != (int)Option.choose) return base.ProcessCmdKey(ref msg, keyData);
            try
            {
                switch (keyData)
                {
                    case Keys.D1:
                        hchoose = hinh2D.ElementAt(0);
                        Repaint();
                        break;
                    case Keys.D2:
                        hchoose = hinh2D.ElementAt(1);
                        Repaint();
                        break;
                    case Keys.D3:
                        hchoose = hinh2D.ElementAt(2);
                        Repaint();
                        break;
                    case Keys.D4:
                        hchoose = hinh2D.ElementAt(3);
                        Repaint();
                        break;
                    case Keys.D5:
                        hchoose = hinh2D.ElementAt(4);
                        Repaint();
                        break;
                    case Keys.D6:
                        hchoose = hinh2D.ElementAt(5);
                        Repaint();
                        break;
                    case Keys.D7:
                        hchoose = hinh2D.ElementAt(6);
                        Repaint();
                        break;
                    case Keys.D8:
                        hchoose = hinh2D.ElementAt(7);
                        Repaint();
                        break;
                    case Keys.D9:
                        hchoose = hinh2D.ElementAt(8);
                        Repaint();
                        break;
                    case Keys.Back:
                        if (hchoose != null)
                        {
                            hinh2D.Remove(hchoose);
                            hchoose = null;
                            Repaint();
                        }
                        break;
                    default:
                        MainForm_KeyDown(new object(), new KeyEventArgs(keyData));
                        break;
                }
                
            }
            catch { }
            if (hchoose == null) return true;
            xmin = ymin = 102;
            xmax = ymax = -1;
            foreach (Point p in hchoose.ArrPoint)
            {
                if (p.X < xmin) xmin = p.X;
                if (p.X > xmax) xmax = p.X;
                if (p.Y < ymin) ymin = p.Y;
                if (p.Y > ymax) ymax = p.Y;
            }
            switch (keyData)
            {
                case Keys.Up:
                    moveChoose(0, -1, hchoose);
                    ymin--;
                    ymax--;
                    break;
                case Keys.Down:
                    moveChoose(0, 1, hchoose);
                    ymin++;
                    ymax++;
                    break;
                case Keys.Left:
                    moveChoose(-1, 0, hchoose);
                    xmin--;
                    xmax--;
                    break;
                case Keys.Right:
                    moveChoose(1, 0, hchoose);
                    xmin++;
                    xmax++;
                    break;
            }
            Danhdau(new Point(xmin - 1, ymin - 1), new Point(xmax + 1, ymin - 1), new Point(xmax + 1, ymax + 1), new Point(xmin - 1, ymax + 1), Color.Blue);
            toolStripStatusLabel9.Text = stmp = hchoose.toString();
            return true;
        }
    }
}
