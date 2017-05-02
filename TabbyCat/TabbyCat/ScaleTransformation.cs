using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbyCat
{
    class ScaleTransformation
    {
        const double scaleOffsetLength = 0.05;

        double scaleOffset = 0.5;

        Matrix4 matrix;

        public static double ScaleOffsetLength
        {
            get
            {
                return scaleOffsetLength;
            }
        }

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
