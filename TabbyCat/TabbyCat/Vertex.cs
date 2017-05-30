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
    }
}
