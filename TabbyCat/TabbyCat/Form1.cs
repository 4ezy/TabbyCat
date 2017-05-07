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
        List<Box> boxes;

        TranslationTransformation trTransform;
        RotationTransformation rtTransform;
        ScaleTransformation scTransform;

        public Form1()
        {
            InitializeComponent();

            trTransform = new TranslationTransformation();

            rtTransform = new RotationTransformation(
                (double)xAngleControl.Value,
                (double)yAngleControl.Value,
                (double)zAngleControl.Value
            );

            scTransform = new ScaleTransformation();

            boxes = boxDrafting(Color.Orange);
        }

        private List<Box> boxDrafting(Color color)
        {
            List<Box> boxes = new List<Box>();

            // подушки лап
            boxes.Add(new Box(0, 3, 0, 15, 13, 5, color));
            boxes.Add(new Box(0, -23, 0, 15, -13, 5, color));
            boxes.Add(new Box(45, 3, 0, 60, 13, 5, color));
            boxes.Add(new Box(45, -23, 0, 60, -13, 5, color));

            // лапы
            boxes.Add(new Box(0, 3, 5, 10, 13, 35, color));
            boxes.Add(new Box(0, -23, 5, 10, -13, 35, color));
            boxes.Add(new Box(45, 3, 5, 55, 13, 35, color));
            boxes.Add(new Box(45, -23, 5, 55, -13, 35, color));

            // туловище
            boxes.Add(new Box(-10, 20, 35, 70, -30, 60, color));

            // голова
            boxes.Add(new Box(70, 20, 35, 95, -30, 75, color));

            // хвост
            boxes.Add(new Box(-10, -2, 45, -30, -8, 50, color));
            boxes.Add(new Box(-30, -2, 45, -35, -8, 75, color));
            boxes.Add(new Box(-30, -2, 75, -70, -8, 80, color));

            // полосы
            boxes.Add(new Box(40, 10, 60, 45, -20, 61, Color.Red));
            boxes.Add(new Box(25, 10, 60, 30, -20, 61, Color.Red));
            boxes.Add(new Box(10, 10, 60, 15, -20, 61, Color.Red));

            return boxes;
        }

        private List<Triangle> tethraedronDrafting()
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

        private void drawLine(double xStart, double yStart, double xEnd, double yEnd)
        {
            // алгоритм Брезенхема
            int x1 = (int)(xStart);
            int y1 = (int)(yStart);
            int x2 = (int)(xEnd);
            int y2 = (int)(yEnd);

            int deltaX = Math.Abs(x2 - x1);
            int deltaY = Math.Abs(y2 - y1);

            int signX = x1 < x2 ? 1 : -1;
            int signY = y1 < y2 ? 1 : -1;

            int error = deltaX - deltaY;

            if((x2 > 0) && (x2 < renderArea.Width) && (y2 > 0) && (y2 < renderArea.Height)) 
            {
                renderArea.SetPixel(x2, y2, Color.White);
            }

            while (x1 != x2 || y1 != y2)
            {
                if ((x1 > 0) && (x1 < renderArea.Width) && (y1 > 0) && (y1 < renderArea.Height))
                {
                    renderArea.SetPixel(x1, y1, Color.White);
                }

                int error2 = error * 2;

                if (error2 > -deltaY)
                {
                    error -= deltaY;
                    x1 += signX;
                }
                if (error2 < deltaX)
                {
                    error += deltaX;
                    y1 += signY;
                }
            }
        }

        private void wireFrameRender(Vertex v1, Vertex v2, Vertex v3)
        {
            drawLine(v1.X, v1.Y, v2.X, v2.Y);
            drawLine(v2.X, v2.Y, v3.X, v3.Y);
            drawLine(v3.X, v3.Y, v1.X, v1.Y);
        }

        private void surfaceRender(double[] zBuffer, Vertex v1, Vertex v2, Vertex v3, Color color)
        {
            // алгоритм построчного заполнения
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
                        // z-буферизация
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

        private void drawBoxes(Matrix4 transform, double[] zBuffer)
        {
            foreach(Box b in boxes)
            {
                foreach (Triangle t in b.BottomEdge)
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

                    if (wireFrameRadioButton.Checked)
                    {
                        wireFrameRender(v1, v2, v3);
                    }

                    if (surfaceRadioButton.Checked)
                    {
                        surfaceRender(zBuffer, v1, v2, v3, t.Color);
                    }
                }

                foreach (Triangle t in b.LeftEdge)
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

                    if (wireFrameRadioButton.Checked)
                    {
                        wireFrameRender(v1, v2, v3);
                    }

                    if (surfaceRadioButton.Checked)
                    {
                        surfaceRender(zBuffer, v1, v2, v3, t.Color);
                    }
                }

                foreach (Triangle t in b.RightEdge)
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

                    if (wireFrameRadioButton.Checked)
                    {
                        wireFrameRender(v1, v2, v3);
                    }

                    if (surfaceRadioButton.Checked)
                    {
                        surfaceRender(zBuffer, v1, v2, v3, t.Color);
                    }
                }

                foreach (Triangle t in b.DistantEdge)
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

                    if (wireFrameRadioButton.Checked)
                    {
                        wireFrameRender(v1, v2, v3);
                    }

                    if (surfaceRadioButton.Checked)
                    {
                        surfaceRender(zBuffer, v1, v2, v3, t.Color);
                    }
                }

                foreach (Triangle t in b.NearEdge)
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

                    if (wireFrameRadioButton.Checked)
                    {
                        wireFrameRender(v1, v2, v3);
                    }

                    if (surfaceRadioButton.Checked)
                    {
                        surfaceRender(zBuffer, v1, v2, v3, t.Color);
                    }
                }

                foreach (Triangle t in b.TopEdge)
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

                    if (wireFrameRadioButton.Checked)
                    {
                        wireFrameRender(v1, v2, v3);
                    }

                    if (surfaceRadioButton.Checked)
                    {
                        surfaceRender(zBuffer, v1, v2, v3, t.Color);
                    }
                }
            }
        }

        private void render(Graphics g, Matrix4 transform, double[] zBuffer)
        {
            drawBoxes(transform, zBuffer);

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
