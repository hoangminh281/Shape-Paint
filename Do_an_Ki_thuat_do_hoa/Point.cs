using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Do_an_Ki_thuat_do_hoa
{
    [Serializable]
    class Point
    {
        private int x, y;
        private string name = "Point";
        public int X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
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

        public Point()
        {
            x = 0;
            y = 0;
        }
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public Point(Point p)
        {
            x = p.x;
            y = p.y;
        }
    }
}
