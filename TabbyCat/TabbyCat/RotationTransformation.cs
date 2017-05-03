using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbyCat
{
    class RotationTransformation
    {
        double oxAngle;
        double oyAngle;
        double ozAngle;

        Matrix4 oxMatrix;
        Matrix4 oyMatrix;
        Matrix4 ozMatrix;

        public double OxAngle
        {
            get
            {
                return oxAngle;
            }

            set
            {
                oxAngle = value;

                oxMatrix.Values[5] = Math.Cos(oxAngle);
                oxMatrix.Values[6] = Math.Sin(oxAngle);
                oxMatrix.Values[9] = -Math.Sin(oxAngle);
                oxMatrix.Values[10] = Math.Cos(oxAngle);
            }
        }

        public double OyAngle
        {
            get
            {
                return oyAngle;
            }

            set
            {
                oyAngle = value;

                oyMatrix.Values[0] = Math.Cos(oyAngle);
                oyMatrix.Values[2] = -Math.Sin(oyAngle);
                oyMatrix.Values[8] = Math.Sin(oyAngle);
                oyMatrix.Values[10] = Math.Cos(oyAngle);
            }
        }

        public double OzAngle
        {
            get
            {
                return ozAngle;
            }

            set
            {
                ozAngle = value;

                ozMatrix.Values[0] = Math.Cos(ozAngle);
                ozMatrix.Values[1] = Math.Sin(ozAngle);
                ozMatrix.Values[4] = -Math.Sin(ozAngle);
                ozMatrix.Values[5] = Math.Cos(ozAngle);
            }
        }

        internal Matrix4 OxMatrix
        {
            get
            {
                return oxMatrix;
            }

            set
            {
                oxMatrix = value;
            }
        }

        internal Matrix4 OyMatrix
        {
            get
            {
                return oyMatrix;
            }

            set
            {
                oyMatrix = value;
            }
        }

        internal Matrix4 OzMatrix
        {
            get
            {
                return ozMatrix;
            }

            set
            {
                ozMatrix = value;
            }
        }

        //public RotationTransformation()
        //{
        //    this.oxAngle = 0;
        //    this.oxMatrix = new Matrix4(
        //        new double[] {
        //            1, 0, 0, 0,
        //            0, Math.Cos(oxAngle), Math.Sin(oxAngle), 0,
        //            0, -Math.Sin(oxAngle), Math.Cos(oxAngle), 0,
        //            0, 0, 0, 1
        //        });

        //    this.oyAngle = 0;
        //    this.oyMatrix = new Matrix4(
        //        new double[] {
        //            Math.Cos(oyAngle), 0, -Math.Sin(oyAngle), 0,
        //            0, 1, 0, 0,
        //            Math.Sin(oyAngle), 0, Math.Cos(oyAngle), 0,
        //            0, 0, 0, 1
        //        });

        //    this.ozAngle = 0;
        //    this.ozMatrix = new Matrix4(
        //        new double[] {
        //            Math.Cos(ozAngle), Math.Sin(ozAngle), 0, 0,
        //            -Math.Sin(ozAngle), Math.Cos(ozAngle), 0, 0,
        //            0, 0, 1, 0,
        //            0, 0, 0, 1
        //        });
        //}

        public RotationTransformation(double oxAngle, double oyAngle, double ozAngle)
        {
            this.oxAngle = oxAngle;
            this.oxMatrix = new Matrix4(
                new double[] {
                    1, 0, 0, 0,
                    0, Math.Cos(oxAngle), Math.Sin(oxAngle), 0,
                    0, -Math.Sin(oxAngle), Math.Cos(oxAngle), 0,
                    0, 0, 0, 1
                });

            this.oyAngle = oyAngle;
            this.oyMatrix = new Matrix4(
                new double[] {
                    Math.Cos(oyAngle), 0, -Math.Sin(oyAngle), 0,
                    0, 1, 0, 0,
                    Math.Sin(oyAngle), 0, Math.Cos(oyAngle), 0,
                    0, 0, 0, 1
                });

            this.ozAngle = ozAngle;
            this.ozMatrix = new Matrix4(
                new double[] {
                    Math.Cos(ozAngle), Math.Sin(ozAngle), 0, 0,
                    -Math.Sin(ozAngle), Math.Cos(ozAngle), 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                });
        }
    }
}
