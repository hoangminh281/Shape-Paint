using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Do_an_Ki_thuat_do_hoa
{
    [Serializable]
    class Triangle : Hinh2D
    {
        private Point pBotL, pTop, pBotR;

        public Point PBotL
        {
            get
            {
                return pBotL;
            }

            set
            {
                pBotL = value;
            }
        }

        public Point PTop
        {
            get
            {
                return pTop;
            }

            set
            {
                pTop = value;
            }
        }

        public Point PBotR
        {
            get
            {
                return pBotR;
            }

            set
            {
                pBotR = value;
            }
        }

        public Triangle() : base("Triangle")
        {
            pBotR = new Point();
            pTop = new Point();
            pBotL = new Point();
        }
        public Triangle(Point pBR, Point pT, Point pBL) : base("Triangle")
        {
            pBotR = new Point(pBR);
            pTop = new Point(pT);
            pBotL = new Point(pBL);
            base.P1 = new Point(pBotL);
            base.P2 = new Point(pTop);
            base.P3 = new Point(pBotR);
            base.Tam = new Point((pBotR.X + PTop.X + PBotL.X) / 3, (pBotR.Y + PTop.Y + PBotL.Y) / 3);
        }
    }
}
