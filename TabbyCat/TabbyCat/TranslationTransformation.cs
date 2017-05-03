using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbyCat
{
    class TranslationTransformation
    {
        int xOffset = 0;
        int yOffset = 0;
        int zOffset = 0;

        Matrix4 matrix;

        public int XOffset
        {
            get
            {
                return xOffset;
            }

            set
            {
                xOffset = value;
                matrix.Values[12] = xOffset; 
            }
        }

        public int YOffset
        {
            get
            {
                return yOffset;
            }

            set
            {
                yOffset = value;
                matrix.Values[13] = yOffset;
            }
        }

        public int ZOffset
        {
            get
            {
                return zOffset;
            }

            set
            {
                zOffset = value;
                matrix.Values[14] = xOffset;
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

        public TranslationTransformation()
        {
            this.matrix = new Matrix4(
                new double[] {
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    xOffset, yOffset, zOffset, 1
                });
        }
    }
}
