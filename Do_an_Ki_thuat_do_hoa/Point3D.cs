using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Do_an_Ki_thuat_do_hoa
{
    [Serializable]
    class Point3D
    {
        private int x, y, z;
        public Point3D()
        {
            x = y = z = 0;
        }
        public Point3D(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;

        }
        public Point3D(Point3D p)
        {
            this.x = p.x;
            this.y = p.y;
            this.z = p.z;
        }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Z { get => z; set => z = value; }

        public Point GetPoint()
        {
            return new Point(x - (int)((double)y * (Math.Sqrt(2) / 2)) + 50, -(z - (int)((double)y * (Math.Sqrt(2) / 2))) + 50);
        }
    }
}
