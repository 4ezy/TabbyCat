using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbyCat
{
    class Matrix4
    {
        double[] values;

        public Matrix4(double[] values)
        {
            this.values = values;
        }

        public double[] Values
        {
            get
            {
                return values;
            }

            set
            {
                values = value;
            }
        }

        public Matrix4 multiply(Matrix4 other)
        {

            double[] result = new double[16];

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        result[row * 4 + col] += this.values[row * 4 + i] * other.values[i * 4 + col];
                    }
                }
            }

            return new Matrix4(result);
        }

        public Vertex transform(Vertex inv)
        {
            return new Vertex(
                inv.X * values[0] + inv.Y * values[4] + inv.Z * values[8] + inv.One * values[12],
                inv.X * values[1] + inv.Y * values[5] + inv.Z * values[9] + inv.One * values[13],
                inv.X * values[2] + inv.Y * values[6] + inv.Z * values[10] + inv.One * values[14]);
        }


    }
}
