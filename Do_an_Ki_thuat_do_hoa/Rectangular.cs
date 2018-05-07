using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Do_an_Ki_thuat_do_hoa
{
    [Serializable]
    class Rectangular : Hinh2D
    {
        private Point3D[] pArr;
        private int dai, rong, cao;
        public Rectangular() : base("Rectangular")
        {
            pArr = new Point3D[8];
            for (int i = 0; i < 8; i++)
            {
                pArr[i] = new Point3D();
            }
            dai = rong = cao = 0;
        }
        public Rectangular(Point3D p, int d, int r, int c) : base("Rectangular")
        {
            pArr = new Point3D[8];
            pArr[0] = new Point3D(p);
            dai = d;
            rong = r;
            cao = c;
            pArr[1] = new Point3D(p.X + d, p.Y, p.Z);
            pArr[2] = new Point3D(p.X + d, p.Y, p.Z + r);
            pArr[3] = new Point3D(p.X, p.Y, p.Z + r);
            pArr[4] = new Point3D(p.X, p.Y+c, p.Z);
            pArr[5] = new Point3D(p.X + d, p.Y+c, p.Z);
            pArr[6] = new Point3D(p.X + d, p.Y + c, p.Z + r);
            pArr[7] = new Point3D(p.X, p.Y + c, p.Z + r);
            base.Arr3D = PArr;
            base.Dai = dai;
            base.Rong = rong;
            base.Cao = cao;
        }

        internal Point3D[] PArr { get => pArr; set => pArr = value; }
    }
}
