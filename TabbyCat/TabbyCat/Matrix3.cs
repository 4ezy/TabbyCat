using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbyCat
{
    class Matrix3
    {
        double[] values;

        public Matrix3(double[] values)
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

        public Matrix3 multiply(Matrix3 other)
        {

            double[] result = new double[9];

            for(int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        result[row * 3 + col] += this.values[row * 3 + i] * other.values[i * 3 + col];
                    }
                }
            }
                
            return new Matrix3(result);
        }

        /*public Vertex transform(Vertex inv)
        {
            return new Vertex(inv.X * values[0] + inv.Y * values[3] + inv.Z * values[6],
                inv.X * values[1] + inv.Y * values[4] + inv.Z * values[7],
                inv.X * values[2] + inv.Y * values[5] + inv.Z * values[8]);
        }*/
    }
}
