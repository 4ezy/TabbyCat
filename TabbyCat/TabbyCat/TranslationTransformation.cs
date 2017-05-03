﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TabbyCat
{
    class TranslationTransformation
    {
        double xOffset = 0;
        double yOffset = 0;
        double zOffset = 0;

        Matrix4 matrix;

        public double XOffset
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

        public double YOffset
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

        public double ZOffset
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

        public void setOffsets(decimal xOffset,
            decimal yOffset, decimal zOffset)
        {
            this.XOffset = (int)xOffset;
            this.YOffset = (int)yOffset;
            this.ZOffset = (int)zOffset;
        }

        public void setOffsets(int xOffset,
            int yOffset, int zOffset)
        {
            this.XOffset = xOffset;
            this.YOffset = yOffset;
            this.ZOffset = zOffset;
        }
    }
}
