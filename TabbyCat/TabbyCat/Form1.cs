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
        List<Triangle> tris;
        TranslationTransformation trTransform;
        RotationTransformation rtTransform;
        ScaleTransformation scTransform;

        public Form1()
        {
            InitializeComponent();

            tris = tetrahedronDrafting();

            trTransform = new TranslationTransformation();

            rtTransform = new RotationTransformation(
                degreeToRadian(oxAngleTrackbar.Value),
                degreeToRadian(oyAngleTrackbar.Value),
                degreeToRadian(ozAngleTrackbar.Value)
            );

            scTransform = new ScaleTransformation();
        }

        private double degreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private List<Triangle> tetrahedronDrafting()
        {
            List<Triangle> tris = new List<Triangle>();

            tris.Add(new Triangle(new Vertex(100, 100, 100, 1),
                new Vertex(-100, -100, 100, 1),
                new Vertex(-100, 100, -100, 1),
                Color.White));

            tris.Add(new Triangle(new Vertex(100, 100, 100, 1),
                new Vertex(-100, -100, 100, 1),
                new Vertex(100, -100, -100, 1),
                Color.Red));

            tris.Add(new Triangle(new Vertex(-100, 100, -100, 1),
                new Vertex(100, -100, -100, 1),
                new Vertex(100, 100, 100, 1),
                Color.Green));

            tris.Add(new Triangle(new Vertex(-100, 100, -100, 1),
                new Vertex(100, -100, -100, 1),
                new Vertex(-100, -100, 100, 1),
                Color.Blue));

            return tris;
        }

        private void render(Matrix4 transform, double[] zBuffer)
        {
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

                // compute rectangular bounds for triangle
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
                            double depth = b1 * v1.Z + b2 * v2.Z + b3 * v3.Z;
                            int zIndex = y * renderArea.Width + x;

                            if (zBuffer[zIndex] < depth)
                            {
                                renderArea.SetPixel(x, y, t.Color);
                                zBuffer[zIndex] = depth;
                            }
                        }
                    }
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            renderArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = renderArea;

            Graphics g;
            g = Graphics.FromImage(renderArea);

            // рисование фона
            g.FillRectangle(Brushes.Black, new RectangleF(0, 0, renderArea.Width, renderArea.Height));

            rtTransform.OxAngle = degreeToRadian(oxAngleTrackbar.Value);
            rtTransform.OyAngle = degreeToRadian(oyAngleTrackbar.Value);
            rtTransform.OzAngle = degreeToRadian(ozAngleTrackbar.Value);

            Matrix4 transform = rtTransform.OxMatrix.multiply(rtTransform.OyMatrix);
            transform = transform.multiply(rtTransform.OzMatrix);
            transform = transform.multiply(scTransform.Matrix);
            transform = transform.multiply(trTransform.Matrix);

            // инициализация заполнение z-буфера
            double[] zBuffer = new double[renderArea.Width * renderArea.Height];

            for (int q = 0; q < zBuffer.Length; q++)
            {
                zBuffer[q] = double.NegativeInfinity;
            }

            render(transform, zBuffer);
            
            g.DrawImage(renderArea, 0, 0);
        }

        private void trackBar1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.A)
            {
                trTransform.XOffset += -TranslationTransformation.XOffsetLength;
            }

            if(e.KeyCode == Keys.D)
            {
                trTransform.XOffset += TranslationTransformation.XOffsetLength;
            }

            if (e.KeyCode == Keys.W)
            {
                trTransform.YOffset += -TranslationTransformation.YOffsetLength;
            }

            if (e.KeyCode == Keys.S)
            {
                trTransform.YOffset += TranslationTransformation.XOffsetLength;
            }

            if (e.KeyCode == Keys.Z)
            {
                scTransform.ScaleOffset += ScaleTransformation.ScaleOffsetLength;
            }

            if (e.KeyCode == Keys.X)
            {
                scTransform.ScaleOffset += -ScaleTransformation.ScaleOffsetLength;
            }
        }
    }
}
