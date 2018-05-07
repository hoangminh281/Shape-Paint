using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
namespace Do_an_Ki_thuat_do_hoa
{
    [Serializable]
    class Circle : Hinh2D
    {
        private Point o;
        private int r;
        public Circle() : base("Circle")
        {
            o = new Point();
            R = 0;
        }
        public Circle(Point O, int r) : base("Circle")
        {
            this.o = new Point(O);
            this.R = r;
            base.P1 = new Point(o);
            base.R = r;
            base.Tam = new Point(o);
        }
        public Circle(Point pFirst, Point pLast) : base("Circle")
        {
            this.o = new Point(pFirst);
            r = (int)Math.Sqrt((pFirst.X - pLast.X) * (pFirst.X - pLast.X) + (pFirst.Y - pLast.Y) * (pFirst.Y - pLast.Y));
            base.P1 = new Point(o);
            base.R = r;
            base.Tam = new Point(o);
        }

        public new int R
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
    }
}
