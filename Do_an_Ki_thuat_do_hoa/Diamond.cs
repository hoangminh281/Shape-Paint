using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Do_an_Ki_thuat_do_hoa
{
    [Serializable]
    class Diamond : Hinh2D
    {
        private Point o, o1, o2;
        private int width, height;
        public Diamond() : base("Diamond")
        {
            o = new Point();
            o1 = new Point();
            width = height = 0;
        }
    
        public Diamond(Point o, Point limt) : base("Diamond")
        {
            width = Math.Abs(o.X - limt.X);
            height = Math.Abs(o.Y - limt.Y);
            this.o = new Point(o);
            this.o1 = new Point(this.o.X + width, this.o.Y);
            this.o2 = new Point(this.o.X, this.o.Y + height);
            base.P1 = new Point(o);
            base.P2 = new Point(limt);
            base.Tam = new Point(P1);
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

        public Point O1
        {
            get
            {
                return o1;
            }

            set
            {
                o1 = value;
            }
        }

        public Point O2
        {
            get
            {
                return o2;
            }

            set
            {
                o2 = value;
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
    }
}
