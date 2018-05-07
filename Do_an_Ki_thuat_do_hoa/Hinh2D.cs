using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Do_an_Ki_thuat_do_hoa
{
    [Serializable]
    class Hinh2D
    {
        private Point[] arrPoint;
        private Color color = MainForm.currentColor;
        private String name = "";
        private List<double[,]> transArr;
        private Point p1, p2, p3, p4;
        private int r;
        private Point3D[] arr3D;
        private int dai = 0, rong = 0, cao = 0;
        private Point tam = new Point();
        private float brush = MainForm.CurrentBrush;

        public Hinh2D()
        {
            transArr = new List<double[,]>();
            p1 = new Point();
            p2 = new Point();
            p3 = new Point();
            p4 = new Point();
        }
        public Hinh2D(Hinh2D h2D)
        {
            this.arrPoint = (Point[])h2D.arrPoint.Clone();
            this.name = h2D.name;
            transArr = new List<double[,]>();
            transArr.AddRange(h2D.transArr);
            p1 = new Point(h2D.p1);
            p2 = new Point(h2D.p2);
            p3 = new Point(h2D.p3);
            p4 = new Point(h2D.p4);
        }
        public Hinh2D(String st)
        {
            name = st;
            transArr = new List<double[,]>();
            p1 = new Point();
            p2 = new Point();
            p3 = new Point();
            p4 = new Point();
        }
        public Hinh2D(Point[] arr, String st)
        {
            arrPoint = (Point[])arr.Clone();
            name = st;
            transArr = new List<double[,]>();
            p1 = new Point();
            p2 = new Point();
            p3 = new Point();
            p4 = new Point();
        }
        public Hinh2D(int x, int y, String st)
        {
            arrPoint = new Point[] { new Point(x, y) };
            name = st;
            transArr = new List<double[,]>();
            p1 = new Point();
            p2 = new Point();
            p3 = new Point();
            p4 = new Point();
        }
        public Hinh2D(Point p, String st)
        {
            arrPoint = new Point[] { p };
            name = st;
            transArr = new List<double[,]>();
            p1 = new Point(p);
        }
        public Hinh2D(Point p)
        {
            arrPoint = new Point[] { p };
            transArr = new List<double[,]>();
            p1 = new Point(p);
        }
        public Point[] ArrPoint
        {
            get
            {
                return arrPoint;
            }

            set
            {
                arrPoint = value;
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public List<double[,]> TransArr
        {
            get
            {
                return transArr;
            }

            set
            {
                transArr = value;
            }
        }

        internal Point P1
        {
            get
            {
                return p1;
            }

            set
            {
                p1 = value;
            }
        }

        internal Point P2
        {
            get
            {
                return p2;
            }

            set
            {
                p2 = value;
            }
        }

        internal Point P3
        {
            get
            {
                return p3;
            }

            set
            {
                p3 = value;
            }
        }

        internal Point P4
        {
            get
            {
                return p4;
            }

            set
            {
                p4 = value;
            }
        }

        public int R
        {
            get
            {
                return r;
            }

            set
            {
                r = value;
            }
        }

        internal Point3D[] Arr3D { get => arr3D; set => arr3D = value; }
        public int Dai { get => dai; set => dai = value; }
        public int Rong { get => rong; set => rong = value; }
        public int Cao { get => cao; set => cao = value; }
        internal Point Tam { get => tam; set => tam = value; }
        public float Brush { get => brush; set => brush = value; }

        public string toString()
        {
            if (name.Equals("Point"))
            {
                return "   Tên hình: " + name + ", toạ độ: (" + (p1.X - 50) + "," + (-p1.Y + 50) + ")";
            }
            else if (name.Equals("Circle"))
            {
                return "   Tên hình: " + name + ", Tâm: (" + (p1.X - 50) + "," + (-p1.Y + 50) + ")" + ", bán kính: " + r;
            }
            else if (name.Equals("Line"))
            {
                return "   Tên hình: " + name + ", toạ độ: (" + (p1.X - 50) + "," + (-p1.Y + 50) + ") " + "(" + (p2.X - 50) + "," + (-p2.Y + 50) + ")" + ", độ dài: " + Math.Round(Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)), 2);
            }
            else if (name.Equals("Ellipse"))
            {
                return "   Tên hình: " + name + ", Tâm: (" + (p1.X - 50) + "," + (-p1.Y + 50) + ")" + ", chiều dài: " + Math.Abs(p1.X - p2.X) * 2 + ", chiều rộng: " + Math.Abs(p1.Y - p2.Y) * 2;
            }
            else if (name.Equals("Rectangle"))
            {
                return "   Tên hình: " + name + ", toạ độ: (" + (p1.X - 50) + "," + (-p1.Y + 50) + ") " + "(" + (p2.X - 50) + "," + (-p2.Y + 50) + ") " + "(" + (p3.X - 50) + "," + (-p3.Y + 50) + ") " + "(" + (p4.X - 50) + "," + (-p4.Y + 50) + ") " + ", chiều dài: " + Math.Abs(p1.X - p3.X) + ", chiều rộng: " + Math.Abs(p1.Y - p3.Y);
            }
            else if (name.Equals("Square"))
            {
                return "   Tên hình: " + name + ", toạ độ: (" + (p1.X - 50) + "," + (-p1.Y + 50) + ") " + "(" + (p2.X - 50) + "," + (-p2.Y + 50) + ") " + "(" + (p3.X - 50) + "," + (-p3.Y + 50) + ") " + "(" + (p4.X - 50) + "," + (-p4.Y + 50) + ") " + ", kích thước: " + Math.Abs(p1.X - p3.X);
            }
            else if (name.Equals("Parallelogram"))
            {
                return "   Tên hình: " + name + ", toạ độ: (" + (p1.X - 50) + "," + (-p1.Y + 50) + ") " + "(" + (p2.X - 50) + "," + (-p2.Y + 50) + ") " + "(" + (p3.X - 50) + "," + (-p3.Y + 50) + ") " + "(" + (p4.X - 50) + "," + (-p4.Y + 50) + ") " + ", chiều dài: " + Math.Round(Math.Sqrt(Math.Pow(p1.X - p4.X, 2) + Math.Pow(p1.Y - p4.Y, 2)), 2) + ", chiều rộng: " + Math.Round(Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)), 2);
            }
            else if (name.Equals("Diamond"))
            {
                return "   Tên hình: " + name + ", toạ độ: (" + (p1.X - 50) + "," + (-p1.Y + 50) + ") " + "(" + (p2.X - 50) + "," + (-p2.Y + 50) + ") " + "(" + (p3.X - 50) + "," + (-p3.Y + 50) + ") " + "(" + (p4.X - 50) + "," + (-p4.Y + 50) + ") " + ", chiều dài: " + Math.Abs(p2.X - p4.X) + ", chiều rộng: " + Math.Abs(p1.Y - p3.Y);
            }
            else if (name.Equals("Trapezoid"))
            {
                return "   Tên hình: " + name + ", toạ độ: (" + (p1.X - 50) + "," + (-p1.Y + 50) + ") " + "(" + (p2.X - 50) + "," + (-p2.Y + 50) + ") " + "(" + (p3.X - 50) + "," + (-p3.Y + 50) + ") " + "(" + (p4.X - 50) + ", " + (-p4.Y + 50) + ") " + ", cạnh dưới: " + Math.Abs(p1.X - p4.X) + ", cạnh trên: " + Math.Abs(p2.X - p3.X);
            }
            else if (name.Equals("Triangle"))
            {
                return "   Tên hình: " + name + ", toạ độ: (" + (p1.X - 50) + "," + (-p1.Y + 50) + ") " + "(" + (p2.X - 50) + "," + (-p2.Y + 50) + ") " + "(" + (p3.X - 50) + "," + (-p3.Y + 50) + ")";
            }
            else if (name.Equals("Rectangular"))
            {
                return "   Tên hình: " + name + ", toạ độ: (" + arr3D[0].X + "," + arr3D[0].Y + "," + arr3D[0].Z + ") " + "(" + arr3D[1].X + "," + arr3D[1].Y + "," + arr3D[1].Z + ") " + "(" + arr3D[2].X + "," + arr3D[2].Y + "," + arr3D[2].Z + ") " + "(" + arr3D[3].X + "," + arr3D[3].Y + "," + arr3D[3].Z + ") " + "(" + arr3D[4].X + "," + arr3D[4].Y + "," + arr3D[4].Z + ") " + "(" + arr3D[5].X + "," + arr3D[5].Y + "," + arr3D[5].Z + ") " + "(" + arr3D[6].X + "," + arr3D[6].Y + "," + arr3D[6].Z + ") " + "(" + arr3D[7].X + "," + arr3D[7].Y + "," + arr3D[7].Z + ") " + ", dài: " + dai + ", rộng: " + cao + ", cao: " + rong;
            }
            else if (name.Equals("Cone"))
            {
                return "   Tên hình: " + name + ", toạ độ: (" + arr3D[0].X + ", " + arr3D[0].Y + ", " + arr3D[0].Z + ")" + ", (" + arr3D[1].X + ", " + arr3D[1].Y + ", " + arr3D[1].Z + ")" + ", (" + arr3D[2].X + ", " + arr3D[2].Y + ", " + arr3D[2].Z + ")" + ", (" + arr3D[3].X + ", " + arr3D[3].Y + ", " + arr3D[3].Z + ")" + ", (" + arr3D[4].X + ", " + arr3D[4].Y + ", " + arr3D[4].Z + ")" + ", (" + arr3D[5].X + ", " + arr3D[5].Y + ", " + arr3D[5].Z + ")" + ", (" + arr3D[6].X + ", " + arr3D[6].Y + ", " + arr3D[6].Z + ")" + ", (" + arr3D[7].X + ", " + arr3D[7].Y + ", " + arr3D[7].Z + ")" + ", bán kính: " + rong + ", chiều cao: " + cao;
            }
            else if (name.Equals("Pyramid"))
            {
                return "   Tên hình: " + name + ", toạ độ: (" + arr3D[0].X + ", " + arr3D[0].Y + ", " + arr3D[0].Z + ")" + ", (" + arr3D[1].X + ", " + arr3D[1].Y + ", " + arr3D[1].Z + ")" + ", (" + arr3D[4].X + ", " + arr3D[4].Y + ", " + arr3D[4].Z + ")" + ", (" + arr3D[5].X + ", " + arr3D[5].Y + ", " + arr3D[5].Z + ")" + ", (" + arr3D[3].X + ", " + arr3D[3].Y + ", " + arr3D[3].Z + ")" + ", dài: " + dai + ", rộng: " + cao + ", cao: " + rong;
            }
            else return "              " + color;
        }
    }
}
