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
    class Parall : Hinh2D
    {
        private Point pTopL, pBotL, pBotR;
        private int width, height;

        public Parall() : base("Parallelogram")
        {
            pTopL = new Point();
            pBotL = new Point();
            pBotR = new Point();
            width = height = 0;
        }
        public Parall(Point pTL, Point pBL, Point pBR) : base("Parallelogram")
        {
            this.pTopL = new Point(pTL);
            this.pBotL = new Point(pBL);
            this.pBotR = new Point(pBR);
            width = pBotL.X - pBotR.X;
            height = pTopL.Y - pBotL.Y;
            base.P1 = new Point(PBotL);
            base.P2 = new Point(pTopL);
            base.P3 = new Point(PTopL.X - Width, PBotR.Y + Height);
            base.P4 = new Point(pBotR);
            base.Tam = new Point((pTopL.X + pBotR.X) / 2, (pTopL.Y + pBotR.Y) / 2);
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

        public Point PTopL
        {
            get
            {
                return pTopL;
            }

            set
            {
                pTopL = value;
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
