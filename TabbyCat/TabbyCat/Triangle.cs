using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TabbyCat
{
    struct Triangle
    {
        Vertex v1;
        Vertex v2;
        Vertex v3;

        Color color;

        internal Vertex V1
        {
            get
            {
                return v1;
            }

            set
            {
                v1 = value;
            }
        }

        internal Vertex V2
        {
            get
            {
                return v2;
            }

            set
            {
                v2 = value;
            }
        }

        internal Vertex V3
        {
            get
            {
                return v3;
            }

            set
            {
                v3 = value;
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public Triangle(Vertex v1, Vertex v2, Vertex v3, Color color)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.color = color;
        }
    }
}
