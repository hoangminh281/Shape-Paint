using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do_an_Ki_thuat_do_hoa
{
    class TransClass
    {
        public double[,] tinhtien(double x, double y)
        {
            return new double[,] {
                { 1,0,0 },
                { 0,1,0 },
                { x, y, 1}
            };
        }
        public double[,] tile(double x, double y)
        {
            return new double[,] {
                { x,0,0 },
                { 0,y,0 },
                { 0, 0, 1}
            };
        }
        public double[,] quay(double alpha)
        {
            Console.Write(Math.Cos(alpha));
            return new double[,] {
                { Math.Cos(alpha),Math.Sin(alpha),0 },
                { -Math.Sin(alpha),Math.Cos(alpha),0 },
                { 0,0,1 }
            };
            
        }
        public double[,] doixungOy()
        {
            return new double[,] {
                {-1,0,0 },
                {0,1,0 },
                {0,0,1 }
            };
        }
        public double[,] doixungOx()
        {
            return new double[,] {
                {1,0,0 },
                {0,-1,0 },
                {0,0,1 }
            };
        }
        public double[,] doixungO()
        {
            return new double[,] {
                {-1,0,0 },
                {0,-1,0 },
                {0,0,1 }
            };
        }
    }
}
