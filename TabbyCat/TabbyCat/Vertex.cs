using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbyCat
{
    struct Vertex
    {
        double x;
        double y;
        double z;
        double one;

        public Vertex (double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.one = 1;
        }

        public double X
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

        public double Y
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

        public double Z
        {
            get
            {
                return z;
            }

            set
            {
                z = value;
            }
        }

        public double One
        {
            get
            {
                return one;
            }
        }

        public static Vertex operator -(Vertex left, Vertex right)
        {
            return new Vertex(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public double Length()
        {
            double ls = X * X + Y * Y + Z * Z + One * One;
            return Math.Sqrt(ls);

        }
    }
}
