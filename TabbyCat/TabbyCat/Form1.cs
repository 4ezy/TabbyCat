using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TabbyCat
{
    public partial class Form1 : Form
    {
        Bitmap renderArea;

        public Form1()
        {
            InitializeComponent();
        }

        private double degreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private List<Triangle> tetrahedronDrafting()
        {
            List<Triangle> tris = new List<Triangle>();

            tris.Add(new Triangle(new Vertex(100, 100, 100),
                new Vertex(-100, -100, 100),
                new Vertex(-100, 100, -100),
                Color.White));

            tris.Add(new Triangle(new Vertex(100, 100, 100),
                new Vertex(-100, -100, 100),
                new Vertex(100, -100, -100),
                Color.Red));

            tris.Add(new Triangle(new Vertex(-100, 100, -100),
                new Vertex(100, -100, -100),
                new Vertex(100, 100, 100),
                Color.Green));

            tris.Add(new Triangle(new Vertex(-100, 100, -100),
                new Vertex(100, -100, -100),
                new Vertex(-100, -100, 100),
                Color.Blue));

            return tris;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            List<Triangle> tris = tetrahedronDrafting();

            renderArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = renderArea;

            Graphics g;
            g = Graphics.FromImage(renderArea);

            g.FillRectangle(Brushes.Black, new RectangleF(0, 0, renderArea.Width, renderArea.Height));

            double horizontal = degreeToRadian(trackBar1.Value);

            Matrix3 horizTansform = new Matrix3(new double[] {Math.Cos(horizontal), 0, -Math.Sin(horizontal),
                                                     0, 1, 0,
                                                     Math.Sin(horizontal), 0, Math.Cos(horizontal)});

            double vertical = degreeToRadian(trackBar2.Value);
            Matrix3 vertTransform = new Matrix3(new double[] {1, 0, 0,
                                                   0, Math.Cos(vertical), Math.Sin(vertical),
                                                   0, -Math.Sin(vertical), Math.Cos(vertical)});

            Matrix3 transform = horizTansform.multiply(vertTransform);

            Bitmap bmp = new Bitmap(renderArea.Width, renderArea.Height);

            //double[] zBuffer = new double[renderArea.Width * renderArea.Height];

            //// initialize array with extremely far away depths
            //for (int q = 0; q < zBuffer.Length; q++)
            //{
            //    zBuffer[q] = Double.NegativeInfinity;
            //}

            foreach (Triangle t in tris)
            {
                Vertex v1 = transform.transform(t.V1);
                Vertex v2 = transform.transform(t.V2);
                Vertex v3 = transform.transform(t.V3);

                v1.X += renderArea.Width / 2;
                v1.Y += renderArea.Height / 2;
                v2.X += renderArea.Width / 2;
                v2.Y += renderArea.Height / 2;
                v3.X += renderArea.Width / 2;
                v3.Y += renderArea.Height / 2;

                int minX = (int)Math.Max(0, Math.Ceiling(Math.Min(v1.X, Math.Min(v2.X, v3.X))));
                int maxX = (int)Math.Min(renderArea.Width - 1, Math.Floor(Math.Max(v1.X, Math.Max(v2.X, v3.X))));
                int minY = (int)Math.Max(0, Math.Ceiling(Math.Min(v1.Y, Math.Min(v2.Y, v3.Y))));
                int maxY = (int)Math.Min(renderArea.Height - 1, Math.Floor(Math.Max(v1.Y, Math.Max(v2.Y, v3.Y))));

                double triangleArea = (v1.Y - v3.Y) * (v2.X - v3.X) + (v2.Y - v3.Y) * (v3.X - v1.X);
                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        double b1 = ((y - v3.Y) * (v2.X - v3.X) + (v2.Y - v3.Y) * (v3.X - x)) / triangleArea;
                        double b2 = ((y - v1.Y) * (v3.X - v1.X) + (v3.Y - v1.Y) * (v1.X - x)) / triangleArea;
                        double b3 = ((y - v2.Y) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v2.X - x)) / triangleArea;
                        if (b1 >= 0 && b1 <= 1 && b2 >= 0 && b2 <= 1 && b3 >= 0 && b3 <= 1)
                        {
                            // for each rasterized pixel:
                            //double depth = b1 * v1.Z + b2 * v2.Z + b3 * v3.Z;
                            //int zIndex = y * renderArea.Width + x;
                            //if (zBuffer[zIndex] < depth)
                            //{
                            renderArea.SetPixel(x, y, t.Color);
                            //    zBuffer[zIndex] = depth;
                            //}
                        }
                    }
                }
            }

            g.DrawImage(renderArea, 0, 0);
        }
    }
}
