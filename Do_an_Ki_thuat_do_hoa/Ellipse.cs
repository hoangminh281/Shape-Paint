using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Do_an_Ki_thuat_do_hoa
{
    [Serializable]
    class Ellipse : Hinh2D
    {
        private Point o, r;
        private int a, b;

        public Ellipse() : base("Ellipse")
        {
            o = new Point();
            r = new Point();
            b = 0;
        }
        public Ellipse(Point o1, Point r1) : base("Ellipse")
        {
            o = new Point(o1);
            r = new Point(r1);
            a = Math.Abs(o1.X - r1.X);
            b = Math.Abs(o1.Y - r1.Y);
            base.P1 = new Point(o1);
            base.P2 = new Point(r1);
            base.Tam = new Point(o);
        }
        public Ellipse(Point o1, int dai, int rong)
        {
            o = new Point(o1);
            a = dai;
            b = rong;
            r = new Point(o1.X+dai,o1.Y+rong);
            base.P1 = new Point(o1);
            base.P2 = new Point(r);
            base.Tam = new Point(o);
        }
        public Point O
        {
            get
            {
                return o;
            }

            set
            {
                o = value;
            }
        }

        public new Point R
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

        public int B
        {
            get
            {
                return b;
            }

            set
            {
                b = value;
            }
        }

        public int A
        {
            get
            {
                return a;
            }

            set
            {
                a = value;
            }
        }
    }
}
