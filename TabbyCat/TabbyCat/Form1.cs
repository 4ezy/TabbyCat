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
                (double)xAngleControl.Value,
                (double)yAngleControl.Value,
                (double)zAngleControl.Value
            );

            scTransform = new ScaleTransformation();
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

        private GraphicsPath wireFrameRender(Vertex v1, Vertex v2, Vertex v3)
        {
            GraphicsPath gPath = new GraphicsPath();

            gPath.AddLine((float)v1.X, (float)v1.Y, (float)v2.X, (float)v2.Y);
            gPath.AddLine((float)v2.X, (float)v2.Y, (float)v3.X, (float)v3.Y);
            gPath.AddLine((float)v3.X, (float)v3.Y, (float)v1.X, (float)v1.Y);

            return gPath;
        }

        private void surfaceRender(double[] zBuffer, Vertex v1, Vertex v2, Vertex v3, Color color)
        {
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
                            renderArea.SetPixel(x, y, color);
                            zBuffer[zIndex] = depth;
                        }
                    }
                }
            }
        }

        private void render(Graphics g, Matrix4 transform, double[] zBuffer)
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

                if(wireFrameRadioButton.Checked)
                {
                    GraphicsPath gPath = wireFrameRender(v1, v2, v3);

                    g.DrawPath(new Pen(Color.White, 2), gPath); 
                }

                if(surfaceRadioButton.Checked)
                {
                    surfaceRender(zBuffer, v1, v2, v3, t.Color);
                }
            }

            g.DrawImage(renderArea, 0, 0);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            renderArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = renderArea;

            Graphics g;
            g = Graphics.FromImage(renderArea);

            // рисование фона
            g.FillRectangle(Brushes.Black, new RectangleF(0, 0, renderArea.Width, renderArea.Height));

            trTransform.setOffsets(xOffsetControl.Value, yOffsetControl.Value, zOffsetControlControl.Value);

            scTransform.ScaleOffset = (double)scaleControl.Value;

            rtTransform.setAngles(xAngleControl.Value, yAngleControl.Value, zAngleControl.Value);

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

            render(g, transform, zBuffer);
        }
    }
}
