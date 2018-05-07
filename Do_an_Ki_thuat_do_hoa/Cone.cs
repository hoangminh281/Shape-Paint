using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do_an_Ki_thuat_do_hoa
{
    [Serializable]
    class Cone : Hinh2D
    {
        private Point3D O;
        private int r, h;
        public Cone() : base("Cone")
        {
            O = new Point3D();
            r = h = 0;
        }
        public Cone(Point3D o, int r, int h) : base("Cone")
        {
            this.O = new Point3D(o);
            this.r = r;
            this.h = h;
            base.Arr3D = new Point3D[1];
            base.Arr3D[0] = new Point3D(o);
            base.Rong = r;
            base.Cao = h;
        }

        public int R1 { get => r; set => r = value; }
        public int H { get => h; set => h = value; }
        internal Point3D O1 { get => O; set => O = value; }
    }
}
