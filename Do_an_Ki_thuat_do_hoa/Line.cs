using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Do_an_Ki_thuat_do_hoa
{
    [Serializable]
    class Line : Hinh2D
    {
        private Point startP, endP;
        public Line() : base("Line")
        {
            StartP = new Point();
            EndP = new Point();
        }
        public Line(Point p1, Point p2) : base("Line")
        {
            this.StartP = new Point(p1);
            this.endP = new Point(p2);
            base.P1 = new Point(endP);
            base.P2 = new Point(startP);
            base.Tam = new Point((startP.X + endP.X) / 2, (startP.Y + endP.Y) / 2);
        }
        public Point StartP
        {
            get
            {
                return startP;
            }

            set
            {
                startP = value;
            }
        }

        public Point EndP
        {
            get
            {
                return endP;
            }

            set
            {
                endP = value;
            }
        }
    }
}
