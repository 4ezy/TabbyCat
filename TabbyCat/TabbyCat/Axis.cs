using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbyCat
{
    class Axis
    {
        Vertex zero;
        Vertex x;
        Vertex y;

        Color xColor;
        Color yColor;

        public Axis(Vertex zero, Vertex x, Vertex y, Color xColor, Color yColor)
        {
            this.zero = zero;
            this.x = x;
            this.y = y;
            this.xColor = xColor;
            this.yColor = yColor;
        }
    }
}
