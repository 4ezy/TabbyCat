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

        List<Cat> cats;

        TranslationTransformation trTransform;
        RotationTransformation rtTransform;
        ScaleTransformation scTransform;

        CameraRotationTransformation viewRtTransform;
        CameraTranslationTransformation viewTrTransform;

        int changingIndex = 0;
        int catNumber = 0;

        double torsoXOffset = 0;
        double torsoYOffset = 0;

        double pawsHeight;
        double pawsWidth;
        double tailLength;
        double tailWidth;
        double torsoXMin;

        double lastBandXMin;
        double firstBandXStart;
        double teethFirstYMin;
        double teethFirstYMax;

        decimal headLength;
        decimal headWidth;
        
        const int bandsXOffset = 15;
        const int teethYOffset = 6;

        bool isUpdated1 = false;
        bool isUpdated2 = false;
        bool isUpdated3 = false;

        public tabbyCatRenderForm()
        {
            InitializeComponent();

            cats = new List<Cat>();

            trTransform = new TranslationTransformation();

            rtTransform = new RotationTransformation(
                (double)xAngleControl.Value,
                (double)yAngleControl.Value,
                (double)zAngleControl.Value
            );

            scTransform = new ScaleTransformation();

            viewTrTransform = new CameraTranslationTransformation();

            viewRtTransform = new CameraRotationTransformation();

            radioButton1.Checked = true;
        }

        private void setControls(Cat cat)
        {
            headLength = (decimal)cat.Boxes[9].getLength();
            headWidth = (decimal)cat.Boxes[9].getWidth();

            torsoLengthControl.Value = (decimal)cat.Boxes[0].getLength();
            numericUpDown1.Value = torsoLengthControl.Value;
            torsoWidthControl.Value = (decimal)cat.Boxes[0].getWidth();
            numericUpDown2.Value = torsoWidthControl.Value;

            tailLengthControl.Value = (decimal)cat.Boxes[10].getLength() + (decimal)cat.Boxes[12].getLength();
            numericUpDown3.Value = tailLengthControl.Value;
            tailWidthControl.Value = (decimal)cat.Boxes[10].getWidth();
            numericUpDown4.Value = tailWidthControl.Value;
            tailLength = (double)tailLengthControl.Value;
            tailWidth = (double)tailWidthControl.Value;

            pawsHeightControl.Value = (decimal)cat.Boxes[1].getHeight();
            numericUpDown5.Value = pawsHeightControl.Value;
            pawsWidthControl.Value = (decimal)cat.Boxes[1].getWidth();
            numericUpDown6.Value = pawsWidthControl.Value;
            pawsHeight = (double)pawsHeightControl.Value;
            pawsWidth = (double)pawsWidthControl.Value;

            irisSizeControl.Value = (decimal)cat.Cylinders[0].Radius;
            numericUpDown7.Value = irisSizeControl.Value;
            pupilSizeControl.Value = (decimal)cat.Cylinders[4].Radius;
            numericUpDown8.Value = pupilSizeControl.Value;

            earsWidthControl.Value = (decimal)cat.HalfCylinders[0].Height;
            numericUpDown9.Value = earsWidthControl.Value;

            tongueLengthControl.Value = (decimal)cat.Boxes[14].getLength();
            numericUpDown10.Value = tongueLengthControl.Value;
            tongueWidthControl.Value = (decimal)cat.Boxes[14].getWidth();
            numericUpDown11.Value = tongueWidthControl.Value;

            teethWidthControl.Value = (decimal)cat.Teeth[0].getWidth();
            numericUpDown12.Value = teethWidthControl.Value;

            bandsWidthControl.Value = (decimal)cat.Bands[0].getLength();
            numericUpDown13.Value = bandsWidthControl.Value;

            bandsNumberControl.Value = cat.Bands.Count;
            numericUpDown14.Value = bandsNumberControl.Value;
            teethNumberControl.Value = cat.Teeth.Count;
            numericUpDown15.Value = teethNumberControl.Value;
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

        private void drawTriangles(List<Triangle> tris, Matrix4 transform, double[] zBuffer)
        {
            foreach (Triangle t in tris)
            {
                Vertex v1 = transform.transform(t.V1);
                Vertex v2 = transform.transform(t.V2);
                Vertex v3 = transform.transform(t.V3);

                //if (radioButton2.Checked)
                //{
                //    double f = 10;
                //    double n = 2;
                //    double d = 8;

                //    double a = (f + n) / (f - n);
                //    double b = (-2 * f * n) / (f - n);

                //    v1.X = v1.X * (d / v1.Z);
                //    v1.Y = v1.Y * (d / v1.Z);
                //    v1.Z = (a * v1.Z + b) / v1.Z;

                //    v2.X = v2.X * (d / v2.Z);
                //    v2.Y = v2.Y * (d / v2.Z);
                //    v2.Z = (a * v2.Z + b) / v2.Z;

                //    v3.X = v3.X * (d / v3.Z);
                //    v3.Y = v3.Y * (d / v3.Z);
                //    v3.Z = (a * v3.Z + b) / v3.Z;
                //}

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
                }
            }
        }

        private void drawBoxes(List<Box> boxes, Matrix4 transform, double[] zBuffer)
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

                        torsoXMin = x0;

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

        private void drawCylinders(List<Cylinder> cylinders, Matrix4 transformMatrix, double[] zBuffer)
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

        private void drawHalfCylinders(List<HalfCylinder> halfCylinders, Matrix4 transformMatrix, double[] zBuffer)
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

        private void drawBands(List<Box> bands, Matrix4 transform, double[] zBuffer)
        {
            if (bands.Count != (int)bandsNumberControl.Value)
            {
                if (bands.Count > 0)
                {
                    if ((int)bandsNumberControl.Value < bands.Count)
                    {
                        for (int i = (bands.Count - 1); i >= (int)bandsNumberControl.Value; i--)
                        {
                            bands.RemoveAt(i);
                        }
                    }
                    else if ((int)bandsNumberControl.Value > bands.Count)
                    {
                        for (int i = (bands.Count - 1); i < (int)bandsNumberControl.Value - 1; i++)
                        {
                            bands.Add(new Box(new Vertex(bands[i].XStart - bandsXOffset, bands[i].YStart, bands[i].ZStart),
                                new Vertex(bands[i].XEnd - bandsXOffset, bands[i].YEnd, bands[i].ZEnd), bands[i].Color));
                        }
                    }
                }
                else if (bands.Count == 0)
                {
                    bands.Add(new Box(new Vertex(60, 10, 60), new Vertex(65, -20, 61), Color.Brown));
                }
            }

            int counter = 0;

            foreach (Box b in bands)
            {
                double xOffset = ((double)bandsWidthControl.Value - b.getLength()) / 2;

                double x0 = b.XStart + torsoXOffset - xOffset;
                double y0 = b.YStart;
                double z0 = b.ZStart;
                double x1 = b.XEnd + torsoXOffset + xOffset;
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

                if (counter == 0)
                    firstBandXStart = box.XStart;

                lastBandXMin = box.XStart;
                counter++;
            }
        }

        private void drawTeeth(List<Box> teeth, Matrix4 transform, double[] zBuffer)
        {
            if (teeth.Count != (int)teethNumberControl.Value)
            {
                if (teeth.Count != 0)
                {
                    if ((int)teethNumberControl.Value < teeth.Count)
                    {
                        for (int i = (teeth.Count - 1); i >= (int)teethNumberControl.Value; i--)
                        {
                            teeth.RemoveAt(i);
                        }
                    }
                    else
                    {
                        for (int i = (teeth.Count - 1); i < (int)teethNumberControl.Value - 1; i++)
                        {
                            if (i == 0)
                            {
                                teeth.Add(new Box(new Vertex(teeth[i].XStart, teeth[i].YStart - teethYOffset, teeth[i].ZStart),
                                    new Vertex(teeth[i].XEnd, teeth[i].YEnd - teethYOffset, teeth[i].ZEnd), teeth[i].Color));
                            }
                            else if (i % 2 != 0)
                            {
                                teeth.Add(new Box(new Vertex(teeth[i - 1].XStart, teeth[i - 1].YStart + teethYOffset, teeth[i - 1].ZStart),
                                    new Vertex(teeth[i - 1].XEnd, teeth[i - 1].YEnd + teethYOffset, teeth[i - 1].ZEnd), teeth[i - 1].Color));
                            }
                            else
                            {
                                teeth.Add(new Box(new Vertex(teeth[i - 1].XStart, teeth[i - 1].YStart - teethYOffset, teeth[i - 1].ZStart),
                                    new Vertex(teeth[i - 1].XEnd, teeth[i - 1].YEnd - teethYOffset, teeth[i - 1].ZEnd), teeth[i - 1].Color));
                            }
                        }
                    }
                }
                else
                {
                    teeth.Add(new Box(new Vertex(96, 0, 50), new Vertex(97, -4, 47), Color.White));
                }
            }

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

                if (counter == 0)
                {
                    teethFirstYMin = box.YStart;
                    teethFirstYMax = box.YEnd;
                }
            }
        }

        private void render(Cat cat, Graphics g, Matrix4 transformMatrix, double[] zBuffer)
        {
            drawBoxes(cat.Boxes,transformMatrix, zBuffer);
            drawCylinders(cat.Cylinders, transformMatrix, zBuffer);
            drawHalfCylinders(cat.HalfCylinders, transformMatrix, zBuffer);
            drawBands(cat.Bands, transformMatrix, zBuffer);
            drawTeeth(cat.Teeth, transformMatrix, zBuffer);

            g.DrawImage(renderArea, 0, 0);
        }

        private void checkControlValues1(Cat cat)
        {
            string str;

            if (numericUpDown1.Value < headLength * 2 + 5 || numericUpDown1.Value > headLength * 10 + 10)
            {
                str = string.Format("Длина тела должна лежать в диапазоне от {0} до {1}", headLength * 2 + 5, headLength * 10 + 10);
                numericUpDown1.Value = torsoLengthControl.Value;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                torsoLengthControl.Value = numericUpDown1.Value;
            }

            if (numericUpDown2.Value < headWidth || numericUpDown2.Value > headWidth * 5)
            {
                str = string.Format("Ширина тела должна лежать в диапазоне от {0} до {1}", headWidth, headWidth * 5);
                numericUpDown2.Value = torsoWidthControl.Value;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                torsoWidthControl.Value = numericUpDown2.Value;
            }

            if (numericUpDown3.Value < (decimal)Math.Round(tailLength / 2) || numericUpDown3.Value > (decimal)tailLength * 2)
            {
                str = string.Format("Длина хвоста должна лежать в диапазоне от {0} до {1}",
                    (decimal)Math.Round(tailLength / 2), (decimal)tailLength * 2);
                numericUpDown3.Value = tailLengthControl.Value;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                tailLengthControl.Value = numericUpDown3.Value;
            }

            if (numericUpDown4.Value < (decimal)Math.Round(tailWidth / 2) || numericUpDown4.Value > (decimal)tailWidth * 2)
            {
                str = string.Format("Ширина хвоста должна лежать в диапазоне от {0} до {1}",
                    (decimal)Math.Round(tailWidth / 2), (decimal)tailWidth * 2);
                numericUpDown4.Value = tailWidthControl.Value;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                tailWidthControl.Value = numericUpDown4.Value;
            }

            if (numericUpDown5.Value < (decimal)pawsHeight / 2 || numericUpDown5.Value > (decimal)pawsHeight * 2)
            {
                str = string.Format("Длина лап должна лежать в диапазоне от {0} до {1}",
                    (decimal)pawsHeight / 2, (decimal)pawsHeight * 2);
                numericUpDown5.Value = pawsHeightControl.Value;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                pawsHeightControl.Value = numericUpDown5.Value;
            }

            if (numericUpDown6.Value < (decimal)pawsWidth - 2 || numericUpDown6.Value > (decimal)pawsWidth * 2)
            {
                str = string.Format("Ширина лап должна лежать в диапазоне от {0} до {1}",
                    (decimal)pawsWidth - 2, (decimal)pawsWidth * 2);
                numericUpDown6.Value = pawsWidthControl.Value;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                pawsWidthControl.Value = numericUpDown6.Value;
            }

            if (numericUpDown7.Value < (decimal)cat.Cylinders[0].Radius - 2 || numericUpDown7.Value > (decimal)cat.Cylinders[0].Radius + 2)
            {
                str = string.Format("Радиус глаз должен лежать в диапазоне от {0} до {1}",
                    (decimal)cat.Cylinders[0].Radius - 2, (decimal)cat.Cylinders[0].Radius + 2);
                numericUpDown7.Value = irisSizeControl.Value;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                irisSizeControl.Value = numericUpDown7.Value;
            }

            if (numericUpDown8.Value < (decimal)cat.Cylinders[4].Radius - 2 || numericUpDown8.Value > (decimal)cat.Cylinders[4].Radius + 2)
            {
                str = string.Format("Радиус зрачков должен лежать в диапазоне от {0} до {1}",
                    (decimal)cat.Cylinders[4].Radius - 2, (decimal)cat.Cylinders[4].Radius + 2);
                numericUpDown8.Value = pupilSizeControl.Value;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                pupilSizeControl.Value = numericUpDown8.Value;
            }

            isUpdated1 = true;
        }

        private void checkControlValues2(Cat cat)
        {
            string str;

            if (numericUpDown9.Value < Math.Round((decimal)cat.HalfCylinders[0].Height / 2) || numericUpDown9.Value > (decimal)cat.HalfCylinders[0].Height * 2)
            {
                str = string.Format("Ширина ушей должна лежать в диапазоне от {0} до {1}",
                    Math.Round((decimal)cat.HalfCylinders[0].Height / 2), (decimal)cat.HalfCylinders[0].Height * 2);
                numericUpDown9.Value = earsWidthControl.Value;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated2 = true;
                return;
            }
            else
            {
                earsWidthControl.Value = numericUpDown9.Value;
            }

            if (numericUpDown10.Value < (decimal)cat.Boxes[14].getLength() || numericUpDown10.Value > (decimal)cat.Boxes[14].getLength() + 5)
            {
                str = string.Format("Длина языка должна лежать в диапазоне от {0} до {1}",
                    (decimal)cat.Boxes[14].getLength(), (decimal)cat.Boxes[14].getLength() + 5);
                numericUpDown10.Value = tongueLengthControl.Value;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated2 = true;
                return;
            }
            else
            {
                tongueLengthControl.Value = numericUpDown10.Value;
            }

            if (numericUpDown11.Value < (decimal)cat.Boxes[14].getWidth() / 2 || numericUpDown11.Value > (decimal)cat.Boxes[14].getWidth() + 5)
            {
                str = string.Format("Ширина языка должна лежать в диапазоне от {0} до {1}",
                    (decimal)cat.Boxes[14].getWidth() / 2, (decimal)cat.Boxes[14].getWidth() + 5);
                numericUpDown11.Value = tongueWidthControl.Value;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated2 = true;
                return;
            }
            else
            {
                tongueWidthControl.Value = numericUpDown11.Value;
            }

            if (numericUpDown12.Value < (decimal)cat.Teeth[0].getWidth() - 1 || numericUpDown12.Value > (decimal)cat.Teeth[0].getWidth() + 1)
            {
                str = string.Format("Ширина зубов должна лежать в диапазоне от {0} до {1}",
                    (decimal)cat.Teeth[0].getWidth() - 1, (decimal)cat.Teeth[0].getWidth() + 1);
                numericUpDown12.Value = teethWidthControl.Value;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated2 = true;
                return;
            }
            else
            {
                teethWidthControl.Value = numericUpDown12.Value;
            }

            if (numericUpDown13.Value < Math.Round((decimal)cat.Bands[0].getLength() / 2) || numericUpDown13.Value > (decimal)cat.Bands[0].getLength() * 2)
            {
                str = string.Format("Толщина полос должна лежать в диапазоне от {0} до {1}",
                    Math.Round((decimal)cat.Bands[0].getLength() / 2), (decimal)cat.Bands[0].getLength() * 2);
                numericUpDown13.Value = bandsWidthControl.Value;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated2 = true;
                return;
            }
            else
            {
                bandsWidthControl.Value = numericUpDown13.Value;
            }

            isUpdated2 = true;
        }

        private void checkControlValues3(Cat cat)
        {
            string str;

            if (numericUpDown14.Value >= 0)
            {
                if (cat.Bands.Count == 0)
                {
                    double x = firstBandXStart;

                    for (int i = (cat.Bands.Count); i < (int)numericUpDown14.Value - 1; i++)
                    {
                        if (x - bandsXOffset < torsoXMin + 10)
                        {
                            str = string.Format("Количество полос должно быть в диапазоне от {0} до {1}",
                                0, i + 1);
                            numericUpDown14.Value = bandsNumberControl.Value;
                            MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            isUpdated3 = true;
                            return;
                        }
                        else
                        {
                            x -= bandsXOffset;
                        }
                    }

                    bandsNumberControl.Value = numericUpDown14.Value;
                }
                else
                {
                    double x = lastBandXMin;

                    for (int i = (cat.Bands.Count - 1); i < (int)numericUpDown14.Value - 1; i++)
                    {
                        if (x - bandsXOffset < torsoXMin + 10)
                        {
                            str = string.Format("Количество полос должно быть в диапазоне от {0} до {1}",
                                0, i + 1);
                            numericUpDown14.Value = bandsNumberControl.Value;
                            MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            isUpdated3 = true;
                            return;
                        }
                        else
                        {
                            x -= bandsXOffset;
                        }
                    }

                    bandsNumberControl.Value = numericUpDown14.Value;
                }
            }
            else
            {
                numericUpDown14.Value = bandsNumberControl.Value;
                MessageBox.Show("Количество полос не может быть меньше 0", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated3 = true;
                return;
            }

            if (numericUpDown15.Value >= 0)
            {
                double teethYMin = 0;
                double teethYMax = 0;

                if (cat.Teeth.Count == 0)
                {
                    if (numericUpDown15.Value <= 4)
                    {
                        teethNumberControl.Value = numericUpDown15.Value;
                    }
                    else
                    {
                        str = string.Format("Количество зубов должно быть в диапазоне от {0} до {1}",
                                0, 4);
                        numericUpDown15.Value = teethNumberControl.Value;
                        MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isUpdated3 = true;
                        return;
                    }
                }
                else
                {
                    if (cat.Teeth.Count == 1)
                    {
                        teethYMax = teethFirstYMax;
                        teethYMin = teethFirstYMin;
                    }
                    else
                    {
                        if ((cat.Teeth.Count - 1) % 2 == 0)
                        {
                            teethYMax = cat.Teeth[cat.Teeth.Count - 1].YEnd;
                            teethYMin = cat.Teeth[cat.Teeth.Count - 2].YEnd;
                        }
                        else
                        {
                            teethYMax = cat.Teeth[cat.Teeth.Count - 2].YEnd;
                            teethYMin = cat.Teeth[cat.Teeth.Count - 1].YEnd;
                        }
                    }

                    for (int i = (cat.Teeth.Count - 1); i < (int)numericUpDown15.Value - 1; i++)
                    {
                        if (i % 2 != 0)
                        {
                            if (teethYMax + teethYOffset < cat.Boxes[13].YEnd - 2)
                            {
                                teethYMax += teethYOffset;
                            }
                            else
                            {
                                str = string.Format("Количество зубов должно быть в диапазоне от {0} до {1}",
                                0, i + 1);
                                numericUpDown15.Value = teethNumberControl.Value;
                                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                isUpdated3 = true;
                                return;
                            }
                        }
                        else
                        {
                            if (teethYMin - teethYOffset > cat.Boxes[13].YStart + 2)
                            {
                                teethYMin -= teethYOffset;
                            }
                            else
                            {
                                str = string.Format("Количество зубов должно быть в диапазоне от {0} до {1}",
                                0, i + 1);
                                numericUpDown15.Value = teethNumberControl.Value;
                                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                isUpdated3 = true;
                                return;
                            }
                        }
                    }

                    teethNumberControl.Value = numericUpDown15.Value;
                }
            }
            else
            {
                numericUpDown15.Value = teethNumberControl.Value;
                MessageBox.Show("Количество зубов не может быть меньше 0", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated3 = true;
                return;
            }

            isUpdated3 = true;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            renderArea = new Bitmap(renderPictureBox.Size.Width, renderPictureBox.Size.Height);
            renderPictureBox.Image = renderArea;

            Graphics g;
            g = Graphics.FromImage(renderArea);

            // рисование фона
            g.FillRectangle(Brushes.Black, new RectangleF(0, 0, renderArea.Width, renderArea.Height));

            List<Matrix4> matrices = new List<Matrix4>();

            for (int i = 0; i < cats.Count; i++)
            {
                if (catsListBox.SelectedItem.ToString() == cats[i].Name)
                {
                    if (isUpdated1 == false)
                        checkControlValues1(cats[i]);

                    if (isUpdated2 == false)
                        checkControlValues2(cats[i]);

                    if (isUpdated3 == false)
                        checkControlValues3(cats[i]);

                    if (changingIndex == i)
                    {
                        cats[i].XOffset = xOffsetControl.Value;
                        cats[i].YOffset = yOffsetControl.Value;
                        cats[i].ZOffset = zOffsetControl.Value;
                        cats[i].XAngle = xAngleControl.Value;
                        cats[i].YAngle = yAngleControl.Value;
                        cats[i].ZAngle = zAngleControl.Value;
                        cats[i].ScaleOffset = scaleControl.Value;
                    }
                }
            }

            foreach (Cat cat in cats)
            {
                trTransform.setOffsets(cat.XOffset, cat.YOffset, cat.ZOffset);
                scTransform.ScaleOffset = (double)cat.ScaleOffset;
                rtTransform.setAngles(cat.XAngle, cat.YAngle, cat.ZAngle);

                viewTrTransform.setOffsets(cameraXPositionControl.Value, cameraYPositionControl.Value, cameraZPositionControl.Value);
                viewRtTransform.setAngles(cameraXAngleControl.Value, cameraYAngleControl.Value, cameraZAngleControl.Value);

                Matrix4 transformMatrix = rtTransform.OxMatrix.multiply(rtTransform.OyMatrix);
                transformMatrix = transformMatrix.multiply(rtTransform.OzMatrix);
                transformMatrix = transformMatrix.multiply(scTransform.Matrix);
                transformMatrix = transformMatrix.multiply(trTransform.Matrix);
                transformMatrix = transformMatrix.multiply(viewTrTransform.Matrix);

                Matrix4 viewMatrix = viewTrTransform.Matrix.multiply(viewRtTransform.OyMatrix);
                viewMatrix = viewMatrix.multiply(viewRtTransform.OxMatrix);
                viewMatrix = viewMatrix.multiply(viewRtTransform.OzMatrix);

                transformMatrix = transformMatrix.multiply(viewMatrix);

                matrices.Add(transformMatrix);
            }

            //if (radioButton2.Checked)
            //{
            //    double d = -10;

            //    double[] matrix =
            //    {
            //        1, 0, 0, 0,
            //        0, 1, 0, 0,
            //        0, 0, 1, 1 / d,
            //        0, 0, 0, 0
            //    };

            //    double f = 100;
            //    double n = 1;

            //    double[] matrix =
            //    {
            //        1, 0, 0, 0,
            //        0, 1, 0, 0,
            //        0, 0, (f + n) / (f - n), 1,
            //        0, 0, (-2 * f * n) / (f - n), 0
            //    };

            //    double ar = renderArea.Width / renderArea.Height;
            //    double zNear = 2;
            //    double zFar = 10;
            //    double zRange = zNear - zFar;
            //    double tanHalfFOV = Math.Tan(RotationTransformation.degreeToRadian(90 / 2.0));

            //    double[] matrix =
            //    {
            //            1.0 / (tanHalfFOV * ar), 0.0, 0.0, 0.0,
            //            0.0, 1.0 / tanHalfFOV, 0.0, 0.0,
            //            0.0, 0.0, (-zNear - zFar) / zRange, 2.0 * zFar * zNear / zRange,
            //            0.0, 0.0, 1.0, 0.0
            //        };

            //    Matrix4 perspective = new Matrix4(matrix);

            //    transformMatrix = transformMatrix.multiply(perspective);
            //}

            // инициализация заполнение z-буфера
            double[] zBuffer = new double[renderArea.Width * renderArea.Height];

            for (int q = 0; q < zBuffer.Length; q++)
            {
                zBuffer[q] = double.NegativeInfinity;
            }

            for (int i = 0; i < cats.Count; i++)
            {
                render(cats[i], g, matrices[i], zBuffer);
            }
        }

        private void confrimQualityButton1_Click(object sender, EventArgs e)
        {
            isUpdated1 = !isUpdated1;   
        }

        private void confrimQualityButton2_Click(object sender, EventArgs e)
        {
            isUpdated2 = !isUpdated2;
        }

        private void quantityAcceptButton_Click(object sender, EventArgs e)
        {
            isUpdated3 = !isUpdated3;
        }

        private void addCatButton_Click(object sender, EventArgs e)
        {
            cats.Add(new Cat());
            cats[cats.Count - 1].Name = "Cat" + catNumber;
            catNumber++;
            catsListBox.Items.Add(cats[cats.Count - 1].Name);
            catsListBox.SelectedItem = catsListBox.Items[cats.Count - 1];
            setControls(cats[cats.Count - 1]);

            xOffsetControl.Value = cats[catsListBox.SelectedIndex].XOffset;
            yOffsetControl.Value = cats[catsListBox.SelectedIndex].YOffset;
            zOffsetControl.Value = cats[catsListBox.SelectedIndex].ZOffset;

            scaleControl.Value = cats[catsListBox.SelectedIndex].ScaleOffset;

            xAngleControl.Value = cats[catsListBox.SelectedIndex].XAngle;
            yAngleControl.Value = cats[catsListBox.SelectedIndex].YAngle;
            zAngleControl.Value = cats[catsListBox.SelectedIndex].ZAngle;
        }

        private void catsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //setControls(cats[catsListBox.SelectedIndex]);

            try
            {
                xOffsetControl.Value = cats[catsListBox.SelectedIndex].XOffset;
                yOffsetControl.Value = cats[catsListBox.SelectedIndex].YOffset;
                zOffsetControl.Value = cats[catsListBox.SelectedIndex].ZOffset;

                scaleControl.Value = cats[catsListBox.SelectedIndex].ScaleOffset;

                xAngleControl.Value = cats[catsListBox.SelectedIndex].XAngle;
                yAngleControl.Value = cats[catsListBox.SelectedIndex].YAngle;
                zAngleControl.Value = cats[catsListBox.SelectedIndex].ZAngle;

                changingIndex = catsListBox.SelectedIndex;
            }
            catch (ArgumentOutOfRangeException)
            {
                xOffsetControl.Value = 0;
                yOffsetControl.Value = 0;
                zOffsetControl.Value = 0;

                scaleControl.Value = 1;

                xAngleControl.Value = 0;
                yAngleControl.Value = 0;
                zAngleControl.Value = 0;

                changingIndex = 0;
            }
        }

        private void deleteCatButton_Click(object sender, EventArgs e)
        {
            if (catsListBox.SelectedIndex > 0)
            {
                catsListBox.SelectedIndex = catsListBox.SelectedIndex - 1;
                catsListBox.Items.RemoveAt(catsListBox.SelectedIndex + 1);
                cats.RemoveAt(catsListBox.SelectedIndex + 1);
            }
            if (catsListBox.SelectedIndex == 0)
            {
                if (catsListBox.Items.Count > 0)
                {
                    catsListBox.SelectedIndex = 1;
                    catsListBox.Items.RemoveAt(0);
                    cats.RemoveAt(0);
                }
                else
                {
                    catsListBox.ClearSelected();
                    catsListBox.Items.RemoveAt(0);
                    cats.RemoveAt(0);
                }
            }
        }
    }
}
