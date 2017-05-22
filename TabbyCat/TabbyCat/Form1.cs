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

        List<Box> boxes = new List<Box>();
        List<Cylinder> cylinders = new List<Cylinder>();
        List<HalfCylinder> halfCylinders = new List<HalfCylinder>();
        List<Box> bands = new List<Box>();
        List<Box> teeth = new List<Box>();

        TranslationTransformation trTransform;
        RotationTransformation rtTransform;
        ScaleTransformation scTransform;

        double torsoXOffset = 0;
        double torsoYOffset = 0;

        double pawsHeight;
        double pawsWidth;
        double tailLength;
        double tailWidth;

        public tabbyCatRenderForm()
        {
            InitializeComponent();

            boxes = boxesDrafting(Color.Orange);
            cylinders = cylindersDrafting(Color.White);
            halfCylinders = halfCylindersDrafting(Color.DarkGray);
            bands = bandsDrafting(Color.Brown);
            teeth = teethDrafting(Color.White);

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

            irisSizeControl.Value = (decimal)cylinders[0].Radius;

            pupilSizeControl.Value = (decimal)cylinders[4].Radius;

            earsWidthControl.Value = (decimal)halfCylinders[0].Height;

            tongueLengthControl.Value = (decimal)boxes[14].getLength();
            tongueWidthControl.Value = (decimal)boxes[14].getWidth();

            teethWidthControl.Value = (decimal)teeth[0].getWidth();

            bandsWidthControl.Value = (decimal)bands[0].getLength();
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
            boxes.Add(new Box(new Vertex(0, 3, 0), new Vertex(15, 13, 5), Color.DarkGray));
            boxes.Add(new Box(new Vertex(0, -23, 0), new Vertex(15, -13, 5), Color.DarkGray));
            boxes.Add(new Box(new Vertex(45, 3, 0), new Vertex(60, 13, 5), Color.DarkGray));
            boxes.Add(new Box(new Vertex(45, -23, 0), new Vertex(60, -13, 5), Color.DarkGray));

            // голова
            boxes.Add(new Box(new Vertex(70, 20, 35), new Vertex(95, -30, 75), color));

            // хвост
            boxes.Add(new Box(new Vertex(-10, -2, 45), new Vertex(-30, -8, 50), color));
            boxes.Add(new Box(new Vertex(-30, -2, 45), new Vertex(-35, -8, 75), color));
            boxes.Add(new Box(new Vertex(-30, -2, 75), new Vertex(-70, -8, 80), Color.Brown));

            // рот
            boxes.Add(new Box(new Vertex(95, 10, 40), new Vertex(96, -20, 50), Color.Blue));

            // язык
            boxes.Add(new Box(new Vertex(96, 6, 40), new Vertex(97, -16, 44), Color.Red));

            return boxes;
        }

        private List<Cylinder> cylindersDrafting(Color color)
        {
            List<Cylinder> cylinders = new List<Cylinder>();

            // глаза
            cylinders.Add(new Cylinder(new Vertex(-95, -16, -63), 8, 1, 15, color));
            cylinders.Add(new Cylinder(new Vertex(-95, 6, -63), 8, 1, 15, color));

            cylinders.Add(new Cylinder(new Vertex(-96, -16, -63), 6, 1, 15, Color.Green));
            cylinders.Add(new Cylinder(new Vertex(-96, 6, -63), 6, 1, 15, Color.Green));

            // зрачки
            cylinders.Add(new Cylinder(new Vertex(-97, -16, -63), 3, 1, 15, Color.Black));
            cylinders.Add(new Cylinder(new Vertex(-97, 6, -63), 3, 1, 15, Color.Black));

            return cylinders;
        }


        private List<HalfCylinder> halfCylindersDrafting(Color color)
        {
            List<HalfCylinder> halfCylinders =  new List<HalfCylinder>();

            // уши
            halfCylinders.Add(new HalfCylinder(new Vertex(83, 75, -18), 10, 10, 10, color));
            halfCylinders.Add(new HalfCylinder(new Vertex(83, 75, 8), 10, 10, 10, color));
            halfCylinders.Add(new HalfCylinder(new Vertex(93, 75, -18), 5, 1, 10, Color.LightPink));
            halfCylinders.Add(new HalfCylinder(new Vertex(93, 75, 8), 5, 1, 10, Color.LightPink));

            return halfCylinders;
        }

        private List<Box> bandsDrafting(Color color)
        {
            List<Box> boxes = new List<Box>();

            // полосы
            boxes.Add(new Box(new Vertex(40, 10, 60), new Vertex(45, -20, 61), color));
            boxes.Add(new Box(new Vertex(25, 10, 60), new Vertex(30, -20, 61), color));
            boxes.Add(new Box(new Vertex(10, 10, 60), new Vertex(15, -20, 61), color));

            return boxes;
        }

        private List<Box> teethDrafting(Color color)
        {
            List<Box> boxes = new List<Box>();

            // зубы
            boxes.Add(new Box(new Vertex(96, 0, 50), new Vertex(97, -4, 47), color));
            boxes.Add(new Box(new Vertex(96, -6, 50), new Vertex(97, -10, 47), color));

            return boxes;
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
                        torsoXOffset = ((double)torsoLengthControl.Value - box.getLength()) / 2;
                        torsoYOffset = ((double)torsoWidthControl.Value - box.getWidth()) / 2;

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
                        yOffset = ((double)tailWidthControl.Value - tailWidth) / 2;

                        x0 = box.XStart - torsoXOffset - xOffset;
                        y0 = box.YStart + yOffset;
                        z0 = box.ZStart;
                        x1 = box.XEnd - torsoXOffset - xOffset;
                        y1 = box.YEnd - yOffset;
                        z1 = box.ZEnd;

                        break;
                    case 12:
                        xOffset = ((double)tailLengthControl.Value - tailLength) / 2;
                        yOffset = ((double)tailWidthControl.Value - tailWidth) / 2;

                        x0 = box.XStart - torsoXOffset - xOffset;
                        y0 = box.YStart + yOffset;
                        z0 = box.ZStart;
                        x1 = box.XEnd - torsoXOffset - xOffset * 2;
                        y1 = box.YEnd - yOffset;
                        z1 = box.ZEnd;

                        break;
                    case 13:

                        x0 = box.XStart + torsoXOffset;
                        y0 = box.YStart;
                        z0 = box.ZStart;
                        x1 = box.XEnd + torsoXOffset;
                        y1 = box.YEnd;
                        z1 = box.ZEnd;

                        break;
                    case 14:
                        xOffset = (double)tongueLengthControl.Value - box.getLength();
                        yOffset = ((double)tongueWidthControl.Value - box.getWidth()) / 2;

                        x0 = box.XStart + torsoXOffset;
                        y0 = box.YStart - yOffset;
                        z0 = box.ZStart;
                        x1 = box.XEnd + torsoXOffset + xOffset;
                        y1 = box.YEnd + yOffset;
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
            int counter = 0;
            double whiteOfTheEyeOffset = 0;

            foreach (Cylinder c in cylinders)
            {
                double radiusOffset = 0;

                double x = c.Center.X;
                double y = c.Center.Y;
                double z = c.Center.Z;
                double radius = c.Radius;
                double height = c.Height;
                double vertexCount = c.VertexCount;

                if (counter >=0 && counter <= 1)
                {
                    radiusOffset = (double)irisSizeControl.Value - radius;
                    whiteOfTheEyeOffset = radiusOffset;
                }
                else if (counter >= 2 && counter <= 3)
                {
                    radiusOffset = whiteOfTheEyeOffset;
                }
                else if (counter >= 4 && counter <= 5)
                {
                    radiusOffset = (double)pupilSizeControl.Value - radius;
                }

                Cylinder tmpC = new Cylinder(new Vertex(x - torsoXOffset, y, z),
                    radius + radiusOffset, height, vertexCount, c.Color);

                drawTriangles(tmpC.BottomBase, transformMatrix, zBuffer);
                drawTriangles(tmpC.TopBase, transformMatrix, zBuffer);
                drawTriangles(tmpC.Surface, transformMatrix, zBuffer);

                counter++;
            }
        }

        private void drawHalfCylinders(Matrix4 transformMatrix, double[] zBuffer)
        {
            int counter = 0;
            foreach (HalfCylinder hc in halfCylinders)
            {
                double heigthOffset = 0;

                double x = hc.Center.X;
                double y = hc.Center.Y;
                double z = hc.Center.Z;
                double radius = hc.Radius;
                double height = hc.Height;
                double vertexCount = hc.VertexCount;

                if (counter >= 0 && counter <= 1)
                {
                    heigthOffset = (double)earsWidthControl.Value - height;
                    x = x - heigthOffset;
                }

                HalfCylinder tmpHc = new HalfCylinder(new Vertex(x + torsoXOffset, y, z),
                   radius, height + heigthOffset, vertexCount, hc.Color);

                drawTriangles(tmpHc.BottomBase, transformMatrix, zBuffer);
                drawTriangles(tmpHc.TopBase, transformMatrix, zBuffer);
                drawTriangles(tmpHc.Surface, transformMatrix, zBuffer);

                counter++;
            }
        }

        private void drawBands(Matrix4 transform, double[] zBuffer)
        {
            foreach (Box b in bands)
            {
                double xOffset = ((double)bandsWidthControl.Value - b.getLength()) / 2;

                double x0 = b.XStart - xOffset;
                double y0 = b.YStart;
                double z0 = b.ZStart;
                double x1 = b.XEnd + xOffset;
                double y1 = b.YEnd;
                double z1 = b.ZEnd;

                Box box = new Box(new Vertex(x0, y0, z0),
                    new Vertex(x1, y1, z1), b.Color);

                drawTriangles(box.BottomEdge, transform, zBuffer);
                drawTriangles(box.TopEdge, transform, zBuffer);
                drawTriangles(box.LeftEdge, transform, zBuffer);
                drawTriangles(box.RightEdge, transform, zBuffer);
                drawTriangles(box.NearEdge, transform, zBuffer);
                drawTriangles(box.DistantEdge, transform, zBuffer);
            }
        }

        private void drawTeeth(Matrix4 transform, double[] zBuffer)
        {
            int counter = 0;

            foreach (Box b in teeth)
            {
                double yOffset = ((double)teethWidthControl.Value - b.getWidth()) / 2;

                double x0 = 0;
                double y0 = 0;
                double z0 = 0;
                double x1 = 0;
                double y1 = 0;
                double z1 = 0;

                x0 = b.XStart + torsoXOffset;
                y0 = b.YStart < b.YEnd ? b.YStart - yOffset : b.YStart + yOffset;
                z0 = b.ZStart;
                x1 = b.XEnd + torsoXOffset;
                y1 = b.YStart < b.YEnd ? b.YEnd + yOffset : b.YEnd - yOffset;
                z1 = b.ZEnd;

                Box box = new Box(new Vertex(x0, y0, z0),
                    new Vertex(x1, y1, z1), b.Color);

                drawTriangles(box.BottomEdge, transform, zBuffer);
                drawTriangles(box.TopEdge, transform, zBuffer);
                drawTriangles(box.LeftEdge, transform, zBuffer);
                drawTriangles(box.RightEdge, transform, zBuffer);
                drawTriangles(box.NearEdge, transform, zBuffer);
                drawTriangles(box.DistantEdge, transform, zBuffer);

                counter++;
            }
        }

        private void render(Graphics g, Matrix4 transformMatrix, double[] zBuffer)
        {
            drawBoxes(transformMatrix, zBuffer);
            drawCylinders(transformMatrix, zBuffer);
            drawHalfCylinders(transformMatrix, zBuffer);
            drawBands(transformMatrix, zBuffer);
            drawTeeth(transformMatrix, zBuffer);

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
