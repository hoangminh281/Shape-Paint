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
    class Square : Hinh2D
    {
        private Point pFirst, pLast;
        private int size;

        public Square() : base("Square")
        {
            pFirst = new Point();
            pLast = new Point();
            size = 0;
        }
        public Square(Point p, int size) : base("Square")
        {
            this.pFirst = new Point(p);
            this.pLast = new Point(p.X + size, p.Y - size);
            base.P1 = new Point(pFirst);
            base.P2 = new Point(pFirst.X, pFirst.Y - size);
            base.P3 = new Point(pFirst.X + size, pFirst.Y - size);
            base.P4 = new Point(pFirst.X + size, pFirst.Y);
            this.size = size;
            base.Tam = new Point((PFirst.X + PLast.X) / 2, (PFirst.Y + PLast.Y) / 2);
        }
        public Square(Point p_First, Point p_Last) : base("Square")
        {
            size = Math.Abs(p_Last.X - p_First.X);
            this.pFirst = new Point(p_First);
            this.pLast = new Point(p_Last.X, p_First.Y + (p_Last.Y - p_First.Y > 0 ? size : -size));
            base.P1 = new Point(pFirst);
            base.P2 = new Point(pFirst.X, pFirst.Y+(p_Last.Y - p_First.Y > 0 ? size : -size));
            base.P3 = new Point(pLast);
            base.P4 = new Point(pFirst.X + +(p_Last.X - p_First.X > 0 ? size : -size), pFirst.Y);
            base.Tam = new Point((PFirst.X + PLast.X) / 2, (PFirst.Y + PLast.Y) / 2);
        }
        public Point PFirst
        {
            get
            {
                return pFirst;
            }

            set
            {
                pFirst = value;
            }
        }

        public Point PLast
        {
            get
            {
                return pLast;
            }

            set
            {
                pLast = value;
            }
        }
    }
}
