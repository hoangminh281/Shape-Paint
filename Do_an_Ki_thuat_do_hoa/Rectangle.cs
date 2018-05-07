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
    class Rectangle : Hinh2D
    {
        private Point pFirst, pLast;
        private int width, height;
        public Rectangle() : base("Rectangle")
        {
            pFirst = new Point();
            pLast = new Point();
            width = height = 0;
        }
        public Rectangle(Point p_First, Point p_Last) : base("Rectangle")
        {
            pFirst = new Point(p_First);
            pLast = new Point(p_Last);
            width = pFirst.X - pLast.X;
            height = pFirst.Y - pLast.Y;
            base.P1 = new Point(pFirst);
            base.P3 = new Point(pLast);
            base.P2 = new Point(pFirst.X, pFirst.Y - height);
            base.P4 = new Point(pFirst.X - width, pFirst.Y);
            base.Tam = new Point((PFirst.X + pLast.X) / 2, (pFirst.Y + pLast.Y) / 2);
        }
        public Rectangle(Point p_First, int w, int h) : base("Rectangle")
        {
            pFirst = new Point(p_First);
            pLast = new Point(pFirst.X + w, pFirst.Y - h);
            width = pFirst.X - pLast.X;
            height = pFirst.Y - pLast.Y;
            base.P1 = new Point(pFirst);
            base.P3 = new Point(pLast);
            base.P2 = new Point(pFirst.X, pFirst.Y - height);
            base.P4 = new Point(pFirst.X - width, pFirst.Y);
            base.Tam = new Point((PFirst.X + pLast.X) / 2, (pFirst.Y + pLast.Y) / 2);
        }
        public Rectangle(Point p_First, Point p_Last, Point p1, Point p2)
        {
            pFirst = new Point(p_First);
            pLast = new Point(p_Last);
            base.P1 = new Point(pFirst);
            base.P2 = new Point(pLast);
            base.P2 = new Point(p1);
            base.P4 = new Point(p2);
            base.Tam = new Point((PFirst.X + pLast.X) / 2, (pFirst.Y + pLast.Y) / 2);
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

        public int Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
            }
        }
    }
}
