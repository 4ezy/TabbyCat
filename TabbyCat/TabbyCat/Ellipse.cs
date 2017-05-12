using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbyCat
{
    struct Ellipse
    {
        Vertex center;
        Vertex width;
        Vertex height;
        Color color;

        internal Vertex Center
        {
            get
            {
                return center;
            }

            set
            {
                center = value;
            }
        }

        internal Vertex Width
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

        internal Vertex Height
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

        public Ellipse(Vertex center, Vertex width, Vertex height, Color color)
        {
            this.center = center;
            this.width = width;
            this.height = height;
            this.color = color;
        }
    }
}
