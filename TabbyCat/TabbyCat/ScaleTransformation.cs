using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbyCat
{
    class ScaleTransformation
    {
        double scaleOffset = 0.25;

        Matrix4 matrix;

        public double ScaleOffset
        {
            get
            {
                return scaleOffset;
            }

            set
            {
                scaleOffset = value;

                matrix.Values[0] = scaleOffset;
                matrix.Values[5] = scaleOffset;
                matrix.Values[10] = scaleOffset;
            }
        }

        internal Matrix4 Matrix
        {
            get
            {
                return matrix;
            }

            set
            {
                matrix = value;
            }
        }

        public ScaleTransformation()
        {
            this.matrix = new Matrix4(
                new double[] {
                    scaleOffset, 0, 0, 0,
                    0, scaleOffset, 0, 0,
                    0, 0, scaleOffset, 0,
                    0, 0, 0, 1
                });
        }
    }
}
