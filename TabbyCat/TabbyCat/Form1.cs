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
    public partial class tabbyCatRenderForm : Form
    {
        Bitmap renderArea;

        List<Box> boxes;
        List<Cylinder> cylinders;
        List<HalfCylinder> halfCylinders;

        TranslationTransformation trTransform;
        RotationTransformation rtTransform;
        ScaleTransformation scTransform;

        //Box mouth = new Box(new Vertex(85, 10, 45), new Vertex(95, -20, 55), Color.White);

        double pawsHeight;
        double pawsWidth;
        double tailLength;
        double tailWidth;

        public tabbyCatRenderForm()
        {
            InitializeComponent();

            boxes = boxesDrafting(Color.Orange);

            cylinders = cylindersDrafting(Color.White);

            halfCylinders = halfCylindersDrafting(Color.Brown);

            trTransform = new TranslationTransformation();

            rtTransform = new RotationTransformation(
                (double)xAngleControl.Value,
                (double)yAngleControl.Value,
                (double)zAngleControl.Value
            );

            scTransform = new ScaleTransformation();

            torsoLengthControl.Value = (decimal)boxes[0].getLength();
            torsoWidthControl.Value = (decimal)boxes[0].getWidth();

            pawsHeightControl.Value = (decimal)boxes[1].getHeight();
            pawsWidthControl.Value = (decimal)boxes[1].getWidth();
            pawsHeight = (double)pawsHeightControl.Value;
            pawsWidth = (double)pawsWidthControl.Value;


            tailLengthControl.Value = (decimal)boxes[10].getLength();
            tailWidthControl.Value = (decimal)boxes[10].getWidth();

            tailLength = (double)tailLengthControl.Value;
            tailWidth = (double)tailWidthControl.Value;
        }

        private List<Box> boxesDrafting(Color color)
        {
            List<Box> boxes = new List<Box>();

            // туловище
            boxes.Add(new Box(new Vertex(-10, 20, 35), new Vertex(70, -30, 60), color));

            // конечности
            boxes.Add(new Box(new Vertex(0, 3, 5), new Vertex(10, 13, 35), color));
            boxes.Add(new Box(new Vertex(0, -23, 5), new Vertex(10, -13, 35), color));
            boxes.Add(new Box(new Vertex(45, 3, 5), new Vertex(55, 13, 35), color));
            boxes.Add(new Box(new Vertex(45, -23, 5), new Vertex(55, -13, 35), color));

            // лапы
            boxes.Add(new Box(new Vertex(0, 3, 0), new Vertex(15, 13, 5), Color.Brown));
            boxes.Add(new Box(new Vertex(0, -23, 0), new Vertex(15, -13, 5), Color.Brown));
            boxes.Add(new Box(new Vertex(45, 3, 0), new Vertex(60, 13, 5), Color.Brown));
            boxes.Add(new Box(new Vertex(45, -23, 0), new Vertex(60, -13, 5), Color.Brown));

            // голова
            boxes.Add(new Box(new Vertex(70, 20, 35), new Vertex(95, -30, 75), color));

            // хвост
            boxes.Add(new Box(new Vertex(-10, -2, 45), new Vertex(-30, -8, 50), color));
            boxes.Add(new Box(new Vertex(-30, -2, 45), new Vertex(-35, -8, 75), color));
            boxes.Add(new Box(new Vertex(-30, -2, 75), new Vertex(-70, -8, 80), Color.Brown));

            // полосы
            boxes.Add(new Box(new Vertex(40, 10, 60), new Vertex(45, -20, 61), Color.Brown));
            boxes.Add(new Box(new Vertex(25, 10, 60), new Vertex(30, -20, 61), Color.Brown));
            boxes.Add(new Box(new Vertex(10, 10, 60), new Vertex(15, -20, 61), Color.Brown));

            return boxes;
        }

        private List<Cylinder> cylindersDrafting(Color color)
        {
            List<Cylinder> cylinders = new List<Cylinder>();

            // глаза
            cylinders.Add(new Cylinder(new Vertex(-95, -16, -63), 8, 4, 15, color));
            cylinders.Add(new Cylinder(new Vertex(-95, 6, -63), 8, 4, 15, color));

            // зрачки
            cylinders.Add(new Cylinder(new Vertex(-99, -16, -63), 4, 1, 15, Color.DarkGray));
            cylinders.Add(new Cylinder(new Vertex(-99, 6, -63), 4, 1, 15, Color.DarkGray));

            return cylinders;
        }


        private List<HalfCylinder> halfCylindersDrafting(Color color)
        {
            List<HalfCylinder> halfCylinders =  new List<HalfCylinder>();

            // уши
            halfCylinders.Add(new HalfCylinder(new Vertex(83, 75, -18), 10, 10, 10, color));
            halfCylinders.Add(new HalfCylinder(new Vertex(83, 75, 8), 10, 10, 10, color));

            return halfCylinders;
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

            if ((x2 > 0) && (x2 < renderArea.Width) && (y2 > 0) && (y2 < renderArea.Height))
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

        private void drawTriangles(List<Triangle> edge, Matrix4 transform, double[] zBuffer)
        {
            foreach (Triangle t in edge)
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
                    drawLine(v1.X, v1.Y, v2.X, v2.Y);
                    drawLine(v2.X, v2.Y, v3.X, v3.Y);
                    drawLine(v3.X, v3.Y, v1.X, v1.Y);
                }

                if (surfaceRadioButton.Checked)
                {
                    surfaceRender(zBuffer, v1, v2, v3, t.Color);

                    //drawLine(v1.X, v1.Y, v2.X, v2.Y);
                    //drawLine(v3.X, v3.Y, v1.X, v1.Y);
                }
            }
        }

        private void drawBoxes(Matrix4 transform, double[] zBuffer)
        {
            short counter = 0;
            double torsoXOffset = 0;
            double torsoYOffset = 0;

            foreach (Box b in boxes)
            {
                Box box = b;

                double xOffset = 0;
                double yOffset = 0;
                double zOffset = 0;

                double x0 = box.XStart;
                double y0 = box.YStart;
                double z0 = box.ZStart;
                double x1 = box.XEnd;
                double y1 = box.YEnd;
                double z1 = box.ZEnd;

                switch (counter)
                {
                    // торс
                    case 0:
                        torsoXOffset = ((double)torsoLengthControl.Value - box.getLength()) / 2; ;
                        torsoYOffset = ((double)torsoWidthControl.Value - box.getWidth()) / 2; ;

                        x0 = box.XStart - torsoXOffset;
                        y0 = box.YStart - torsoYOffset;
                        z0 = box.ZStart;
                        x1 = box.XEnd + torsoXOffset;
                        y1 = box.YEnd + torsoYOffset;
                        z1 = box.ZEnd;

                        break;
                    // конечности
                    case 1:
                        yOffset = ((double)pawsWidthControl.Value - pawsWidth) / 2;
                        zOffset = ((double)pawsHeightControl.Value - pawsHeight) / 2;

                        x0 = box.XStart - torsoXOffset;
                        y0 = box.YStart + torsoYOffset - yOffset;
                        z0 = box.ZStart - zOffset;
                        x1 = box.XEnd - torsoXOffset;
                        y1 = box.YEnd + torsoYOffset + yOffset;
                        z1 = box.ZEnd;

                        break;
                    case 2:
                        yOffset = ((double)pawsWidthControl.Value - pawsWidth) / 2;
                        zOffset = ((double)pawsHeightControl.Value - pawsHeight) / 2;

                        x0 = box.XStart - torsoXOffset;
                        y0 = box.YStart - torsoYOffset + yOffset;
                        z0 = box.ZStart - zOffset;
                        x1 = box.XEnd - torsoXOffset;
                        y1 = box.YEnd - torsoYOffset - yOffset;
                        z1 = box.ZEnd;

                        break;
                    case 3:
                        yOffset = ((double)pawsWidthControl.Value - pawsWidth) / 2;
                        zOffset = ((double)pawsHeightControl.Value - pawsHeight) / 2;

                        x0 = box.XStart + torsoXOffset;
                        y0 = box.YStart + torsoYOffset - yOffset;
                        z0 = box.ZStart - zOffset;
                        x1 = box.XEnd + torsoXOffset;
                        y1 = box.YEnd + torsoYOffset + yOffset;
                        z1 = box.ZEnd;

                        break;
                    case 4:
                        yOffset = ((double)pawsWidthControl.Value - pawsWidth) / 2;
                        zOffset = ((double)pawsHeightControl.Value - pawsHeight) / 2;

                        x0 = box.XStart + torsoXOffset;
                        y0 = box.YStart - torsoYOffset + yOffset;
                        z0 = box.ZStart - zOffset;
                        x1 = box.XEnd + torsoXOffset;
                        y1 = box.YEnd - torsoYOffset - yOffset;
                        z1 = box.ZEnd;

                        break;
                    // лапы
                    case 5:
                        yOffset = ((double)pawsWidthControl.Value - pawsWidth) / 2;
                        zOffset = ((double)pawsHeightControl.Value - pawsHeight) / 2;

                        x0 = box.XStart - torsoXOffset;
                        y0 = box.YStart + torsoYOffset - yOffset;
                        z0 = box.ZStart - zOffset;
                        x1 = box.XEnd - torsoXOffset;
                        y1 = box.YEnd + torsoYOffset + yOffset;
                        z1 = box.ZEnd - zOffset;

                        break;
                    case 6:
                        yOffset = ((double)pawsWidthControl.Value - pawsWidth) / 2;
                        zOffset = ((double)pawsHeightControl.Value - pawsHeight) / 2;

                        x0 = box.XStart - torsoXOffset;
                        y0 = box.YStart - torsoYOffset + yOffset;
                        z0 = box.ZStart - zOffset;
                        x1 = box.XEnd - torsoXOffset;
                        y1 = box.YEnd - torsoYOffset - yOffset;
                        z1 = box.ZEnd - zOffset;

                        break;
                    case 7:
                        yOffset = ((double)pawsWidthControl.Value - pawsWidth) / 2;
                        zOffset = ((double)pawsHeightControl.Value - pawsHeight) / 2;

                        x0 = box.XStart + torsoXOffset;
                        y0 = box.YStart + torsoYOffset - yOffset;
                        z0 = box.ZStart - zOffset;
                        x1 = box.XEnd + torsoXOffset;
                        y1 = box.YEnd + torsoYOffset + yOffset;
                        z1 = box.ZEnd - zOffset;

                        break;
                    case 8:
                        yOffset = ((double)pawsWidthControl.Value - pawsWidth) / 2;
                        zOffset = ((double)pawsHeightControl.Value - pawsHeight) / 2;

                        x0 = box.XStart + torsoXOffset;
                        y0 = box.YStart - torsoYOffset + yOffset;
                        z0 = box.ZStart - zOffset;
                        x1 = box.XEnd + torsoXOffset;
                        y1 = box.YEnd - torsoYOffset - yOffset;
                        z1 = box.ZEnd - zOffset;

                        break;
                    // голова
                    case 9:
                        x0 = box.XStart + torsoXOffset;
                        y0 = box.YStart;
                        z0 = box.ZStart;
                        x1 = box.XEnd + torsoXOffset;
                        y1 = box.YEnd;
                        z1 = box.ZEnd;

                        break;
                    // хвост
                    case 10:
                        xOffset = ((double)tailLengthControl.Value - tailLength) / 2;
                        yOffset = ((double)tailWidthControl.Value - tailWidth) / 2;

                        x0 = box.XStart - torsoXOffset;
                        y0 = box.YStart + yOffset;
                        z0 = box.ZStart;
                        x1 = box.XEnd - torsoXOffset - xOffset;
                        y1 = box.YEnd - yOffset;
                        z1 = box.ZEnd;

                        break;
                    case 11:
                        xOffset = ((double)tailLengthControl.Value - tailLength) / 2;
                        yOffset = ((double)tailWidthControl.Value - tailWidth) / 2; ;

                        x0 = box.XStart - torsoXOffset - xOffset;
                        y0 = box.YStart + yOffset;
                        z0 = box.ZStart;
                        x1 = box.XEnd - torsoXOffset - xOffset;
                        y1 = box.YEnd - yOffset;
                        z1 = box.ZEnd;

                        break;
                    case 12:
                        xOffset = ((double)tailLengthControl.Value - tailLength) / 2;
                        yOffset = ((double)tailWidthControl.Value - tailWidth) / 2; ;

                        x0 = box.XStart - torsoXOffset - xOffset;
                        y0 = box.YStart + yOffset;
                        z0 = box.ZStart;
                        x1 = box.XEnd - torsoXOffset - xOffset * 2;
                        y1 = box.YEnd - yOffset;
                        z1 = box.ZEnd;

                        break;
                }

                box = new Box(new Vertex(x0, y0, z0),
                    new Vertex(x1, y1, z1), box.Color);

                drawTriangles(box.BottomEdge, transform, zBuffer);
                drawTriangles(box.TopEdge, transform, zBuffer);
                drawTriangles(box.LeftEdge, transform, zBuffer);
                drawTriangles(box.RightEdge, transform, zBuffer);
                drawTriangles(box.NearEdge, transform, zBuffer);
                drawTriangles(box.DistantEdge, transform, zBuffer);

                counter++;
            }

        }

        private void drawCylinders(Matrix4 transformMatrix, double[] zBuffer)
        {
            foreach (Cylinder c in cylinders)
            {
                drawTriangles(c.BottomBase, transformMatrix, zBuffer);
                drawTriangles(c.TopBase, transformMatrix, zBuffer);
                drawTriangles(c.Surface, transformMatrix, zBuffer);
            }
        }

        private void drawHalfCylinders(Matrix4 transformMatrix, double[] zBuffer)
        {
            foreach (HalfCylinder hc in halfCylinders)
            {
                drawTriangles(hc.BottomBase, transformMatrix, zBuffer);
                drawTriangles(hc.TopBase, transformMatrix, zBuffer);
                drawTriangles(hc.Surface, transformMatrix, zBuffer);
            }
        }

        private void render(Graphics g, Matrix4 transformMatrix, double[] zBuffer)
        {
            drawBoxes(transformMatrix, zBuffer);
            drawCylinders(transformMatrix, zBuffer);
            drawHalfCylinders(transformMatrix, zBuffer);

            g.DrawImage(renderArea, 0, 0);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            renderArea = new Bitmap(renderPictureBox.Size.Width, renderPictureBox.Size.Height);
            renderPictureBox.Image = renderArea;

            Graphics g;
            g = Graphics.FromImage(renderArea);

            // рисование фона
            g.FillRectangle(Brushes.Black, new RectangleF(0, 0, renderArea.Width, renderArea.Height));

            trTransform.setOffsets(xOffsetControl.Value, yOffsetControl.Value, zOffsetControlControl.Value);

            scTransform.ScaleOffset = (double)scaleControl.Value;

            rtTransform.setAngles(xAngleControl.Value, yAngleControl.Value, zAngleControl.Value);

            Matrix4 transformMatrix = rtTransform.OxMatrix.multiply(rtTransform.OyMatrix);
            transformMatrix = transformMatrix.multiply(rtTransform.OzMatrix);
            transformMatrix = transformMatrix.multiply(scTransform.Matrix);
            transformMatrix = transformMatrix.multiply(trTransform.Matrix);

            // инициализация заполнение z-буфера
            double[] zBuffer = new double[renderArea.Width * renderArea.Height];

            for (int q = 0; q < zBuffer.Length; q++)
            {
                zBuffer[q] = double.NegativeInfinity;
            }

            render(g, transformMatrix, zBuffer);
        }

    }
}
