using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Do_an_Ki_thuat_do_hoa
{
    [Serializable]
    class Trape : Hinh2D
    {
        private Point t1, t2, t3, t4;

        public Point T1
        {
            get
            {
                return t1;
            }

            set
            {
                t1 = value;
            }
        }

        public Point T2
        {
            get
            {
                return t2;
            }

            set
            {
                t2 = value;
            }
        }

        public Point T3
        {
            get
            {
                return t3;
            }

            set
            {
                t3 = value;
            }
        }

        public Point T4
        {
            get
            {
                return t4;
            }

            set
            {
                t4 = value;
            }
        }

        public Trape() : base("Trapezoid")
        {
            t1 = new Point();
            t2 = new Point();
            t3 = new Point();
            t4 = new Point();
        }
        public Trape(Point tp1, Point tp2, Point tp3, Point tp4) : base("Trapezoid")
        {
            t1 = new Point(tp1);
            t2 = new Point(tp2);
            t3 = new Point(t2.X + Math.Abs(t2.X - tp3.X), t2.Y);
            t4 = new Point(t1.X + Math.Abs(t1.X - tp4.X), t1.Y);
            base.P1 = new Point(t1);
            base.P2 = new Point(t2);
            base.P3 = new Point(t3);
            base.P4 = new Point(t4);
            base.Tam = new Point((t1.X + t3.X) / 2, (t1.Y + t3.Y) / 2);
        }
        public Trape(Point botP, Point topP, int botWidth, int topWidth) : base("Trapezoid")
        {
            t1 = new Point(botP);
            t2 = new Point(topP);
            t3 = new Point(t2.X + topWidth, t2.Y);
            t4 = new Point(t1.X + botWidth, t1.Y);
            base.P1 = new Point(t1);
            base.P2 = new Point(t2);
            base.P3 = new Point(t3);
            base.P4 = new Point(t4);
            base.Tam = new Point((t1.X + t3.X) / 2, (t1.Y + t3.Y) / 2);
        }
    }
}
