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
        List<Cat> cats;
        List<Matrix4> matrices;
        Bitmap renderArea;
        TranslationTransformation trTransform;
        RotationTransformation rtTransform;
        ScaleTransformation scTransform;
        CameraRotationTransformation viewRtTransform;
        CameraTranslationTransformation viewTrTransform;

        int lastSelectedListBoxIndex = 0;
        int catNameNumber = 0;
        int catCounter = 0;

        //double torsoXOffset = 0;
        //double torsoYOffset = 0;

        //double pawsHeight;
        //double pawsWidth;
        //double tailLength;
        //double tailWidth;
        //double torsoXMin;

        //double lastBandXMin;
        //double firstBandXStart;
        //double teethFirstYMin;
        //double teethFirstYMax;

        //decimal headLength;
        //decimal headWidth;

        //const int bandsXOffset = 15;
        //const int teethYOffset = 6;

        bool isUpdated1 = false;
        bool isUpdated2 = false;
        bool isUpdated3 = false;

        public tabbyCatRenderForm()
        {
            InitializeComponent();

            cats = new List<Cat>();

            matrices = new List<Matrix4>();

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
            numericUpDown1.Value = (decimal)cat.TorsoLength;
            numericUpDown2.Value = (decimal)cat.TorsoWidth;
            numericUpDown3.Value = (decimal)cat.TailLength;
            numericUpDown4.Value = (decimal)cat.TailWidth;
            numericUpDown5.Value = (decimal)cat.PawsHeight;
            numericUpDown6.Value = (decimal)cat.PawsWidth;
            numericUpDown7.Value = (decimal)cat.IrisSize;
            numericUpDown8.Value = (decimal)cat.PupilSize;
            numericUpDown9.Value = (decimal)cat.EarsWidth;
            numericUpDown10.Value = (decimal)cat.TongueLength;
            numericUpDown11.Value = (decimal)cat.TongueWidth;
            numericUpDown12.Value = (decimal)cat.TeethWidth;
            numericUpDown13.Value = (decimal)cat.BandsWidth;
            numericUpDown14.Value = (decimal)cat.BandsNumber;
            numericUpDown15.Value = (decimal)cat.TeethNumber;

            xOffsetControl.Value = cat.XOffset;
            yOffsetControl.Value = cat.YOffset;
            zOffsetControl.Value = cat.ZOffset;
            scaleControl.Value = cat.ScaleOffset;
            xAngleControl.Value = cat.XAngle;
            yAngleControl.Value = cat.YAngle;
            zAngleControl.Value = cat.ZAngle;
        }

        private void clearControls()
        {
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numericUpDown6.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown8.Value = 0;
            numericUpDown9.Value = 0;
            numericUpDown10.Value = 0;
            numericUpDown11.Value = 0;
            numericUpDown12.Value = 0;
            numericUpDown13.Value = 0;
            numericUpDown14.Value = 0;
            numericUpDown15.Value = 0;

            xOffsetControl.Value = 0;
            yOffsetControl.Value = 0;
            zOffsetControl.Value = 0;
            scaleControl.Value = 1;
            xAngleControl.Value = 0;
            yAngleControl.Value = 0;
            zAngleControl.Value = 0;
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
                        cats[catCounter].TorsoXOffset = (cats[catCounter].TorsoLength - box.getLength()) / 2;
                        cats[catCounter].TorsoYOffset = (cats[catCounter].TorsoWidth - box.getWidth()) / 2;

                        x0 = box.XStart - cats[catCounter].TorsoXOffset;
                        y0 = box.YStart - cats[catCounter].TorsoYOffset;
                        z0 = box.ZStart;
                        x1 = box.XEnd + cats[catCounter].TorsoXOffset;
                        y1 = box.YEnd + cats[catCounter].TorsoYOffset;
                        z1 = box.ZEnd;

                        cats[catCounter].TorsoXMin = x0;

                        break;
                    // конечности
                    case 1:
                        yOffset = (cats[catCounter].PawsWidth - cats[catCounter].DefaultPawsWidth) / 2;
                        zOffset = (cats[catCounter].PawsHeight - cats[catCounter].DefaultPawsHeight) / 2;

                        x0 = box.XStart - cats[catCounter].TorsoXOffset;
                        y0 = box.YStart + cats[catCounter].TorsoYOffset - yOffset;
                        z0 = box.ZStart - zOffset;
                        x1 = box.XEnd - cats[catCounter].TorsoXOffset;
                        y1 = box.YEnd + cats[catCounter].TorsoYOffset + yOffset;
                        z1 = box.ZEnd;

                        break;
                    case 2:
                        yOffset = (cats[catCounter].PawsWidth - cats[catCounter].DefaultPawsWidth) / 2;
                        zOffset = (cats[catCounter].PawsHeight - cats[catCounter].DefaultPawsHeight) / 2;

                        x0 = box.XStart - cats[catCounter].TorsoXOffset;
                        y0 = box.YStart - cats[catCounter].TorsoYOffset + yOffset;
                        z0 = box.ZStart - zOffset;
                        x1 = box.XEnd - cats[catCounter].TorsoXOffset;
                        y1 = box.YEnd - cats[catCounter].TorsoYOffset - yOffset;
                        z1 = box.ZEnd;

                        break;
                    case 3:
                        yOffset = (cats[catCounter].PawsWidth - cats[catCounter].DefaultPawsWidth) / 2;
                        zOffset = (cats[catCounter].PawsHeight - cats[catCounter].DefaultPawsHeight) / 2;

                        x0 = box.XStart + cats[catCounter].TorsoXOffset;
                        y0 = box.YStart + cats[catCounter].TorsoYOffset - yOffset;
                        z0 = box.ZStart - zOffset;
                        x1 = box.XEnd + cats[catCounter].TorsoXOffset;
                        y1 = box.YEnd + cats[catCounter].TorsoYOffset + yOffset;
                        z1 = box.ZEnd;

                        break;
                    case 4:
                        yOffset = (cats[catCounter].PawsWidth - cats[catCounter].DefaultPawsWidth) / 2;
                        zOffset = (cats[catCounter].PawsHeight - cats[catCounter].DefaultPawsHeight) / 2;

                        x0 = box.XStart + cats[catCounter].TorsoXOffset;
                        y0 = box.YStart - cats[catCounter].TorsoYOffset + yOffset;
                        z0 = box.ZStart - zOffset;
                        x1 = box.XEnd + cats[catCounter].TorsoXOffset;
                        y1 = box.YEnd - cats[catCounter].TorsoYOffset - yOffset;
                        z1 = box.ZEnd;

                        break;
                    // лапы
                    case 5:
                        yOffset = (cats[catCounter].PawsWidth - cats[catCounter].DefaultPawsWidth) / 2;
                        zOffset = (cats[catCounter].PawsHeight - cats[catCounter].DefaultPawsHeight) / 2;

                        x0 = box.XStart - cats[catCounter].TorsoXOffset;
                        y0 = box.YStart + cats[catCounter].TorsoYOffset - yOffset;
                        z0 = box.ZStart - zOffset;
                        x1 = box.XEnd - cats[catCounter].TorsoXOffset;
                        y1 = box.YEnd + cats[catCounter].TorsoYOffset + yOffset;
                        z1 = box.ZEnd - zOffset;

                        break;
                    case 6:
                        yOffset = (cats[catCounter].PawsWidth - cats[catCounter].DefaultPawsWidth) / 2;
                        zOffset = (cats[catCounter].PawsHeight - cats[catCounter].DefaultPawsHeight) / 2;

                        x0 = box.XStart - cats[catCounter].TorsoXOffset;
                        y0 = box.YStart - cats[catCounter].TorsoYOffset + yOffset;
                        z0 = box.ZStart - zOffset;
                        x1 = box.XEnd - cats[catCounter].TorsoXOffset;
                        y1 = box.YEnd - cats[catCounter].TorsoYOffset - yOffset;
                        z1 = box.ZEnd - zOffset;

                        break;
                    case 7:
                        yOffset = (cats[catCounter].PawsWidth - cats[catCounter].DefaultPawsWidth) / 2;
                        zOffset = (cats[catCounter].PawsHeight - cats[catCounter].DefaultPawsHeight) / 2;

                        x0 = box.XStart + cats[catCounter].TorsoXOffset;
                        y0 = box.YStart + cats[catCounter].TorsoYOffset - yOffset;
                        z0 = box.ZStart - zOffset;
                        x1 = box.XEnd + cats[catCounter].TorsoXOffset;
                        y1 = box.YEnd + cats[catCounter].TorsoYOffset + yOffset;
                        z1 = box.ZEnd - zOffset;

                        break;
                    case 8:
                        yOffset = (cats[catCounter].PawsWidth - cats[catCounter].DefaultPawsWidth) / 2;
                        zOffset = (cats[catCounter].PawsHeight - cats[catCounter].DefaultPawsHeight) / 2;

                        x0 = box.XStart + cats[catCounter].TorsoXOffset;
                        y0 = box.YStart - cats[catCounter].TorsoYOffset + yOffset;
                        z0 = box.ZStart - zOffset;
                        x1 = box.XEnd + cats[catCounter].TorsoXOffset;
                        y1 = box.YEnd - cats[catCounter].TorsoYOffset - yOffset;
                        z1 = box.ZEnd - zOffset;

                        break;
                    // голова
                    case 9:
                        x0 = box.XStart + cats[catCounter].TorsoXOffset;
                        y0 = box.YStart;
                        z0 = box.ZStart;
                        x1 = box.XEnd + cats[catCounter].TorsoXOffset;
                        y1 = box.YEnd;
                        z1 = box.ZEnd;

                        break;
                    // хвост
                    case 10:
                        xOffset = (cats[catCounter].TailLength - cats[catCounter].DefaultTailLength) / 2;
                        yOffset = (cats[catCounter].TailWidth - cats[catCounter].DefaultTailWidth) / 2;

                        x0 = box.XStart - cats[catCounter].TorsoXOffset;
                        y0 = box.YStart + yOffset;
                        z0 = box.ZStart;
                        x1 = box.XEnd - cats[catCounter].TorsoXOffset - xOffset;
                        y1 = box.YEnd - yOffset;
                        z1 = box.ZEnd;

                        break;
                    case 11:
                        xOffset = (cats[catCounter].TailLength - cats[catCounter].DefaultTailLength) / 2;
                        yOffset = (cats[catCounter].TailWidth - cats[catCounter].DefaultTailWidth) / 2;

                        x0 = box.XStart - cats[catCounter].TorsoXOffset - xOffset;
                        y0 = box.YStart + yOffset;
                        z0 = box.ZStart;
                        x1 = box.XEnd - cats[catCounter].TorsoXOffset - xOffset;
                        y1 = box.YEnd - yOffset;
                        z1 = box.ZEnd;

                        break;
                    case 12:
                        xOffset = (cats[catCounter].TailLength - cats[catCounter].DefaultTailLength) / 2;
                        yOffset = (cats[catCounter].TailWidth - cats[catCounter].DefaultTailWidth) / 2;

                        x0 = box.XStart - cats[catCounter].TorsoXOffset - xOffset;
                        y0 = box.YStart + yOffset;
                        z0 = box.ZStart;
                        x1 = box.XEnd - cats[catCounter].TorsoXOffset - xOffset * 2;
                        y1 = box.YEnd - yOffset;
                        z1 = box.ZEnd;

                        break;
                    case 13:
                        x0 = box.XStart + cats[catCounter].TorsoXOffset;
                        y0 = box.YStart;
                        z0 = box.ZStart;
                        x1 = box.XEnd + cats[catCounter].TorsoXOffset;
                        y1 = box.YEnd;
                        z1 = box.ZEnd;

                        break;
                    case 14:
                        xOffset = (cats[catCounter].TongueLength - box.getLength());
                        yOffset = (cats[catCounter].TongueWidth - box.getWidth()) / 2;

                        x0 = box.XStart + cats[catCounter].TorsoXOffset;
                        y0 = box.YStart - yOffset;
                        z0 = box.ZStart;
                        x1 = box.XEnd + cats[catCounter].TorsoXOffset + xOffset;
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

                if (counter >= 0 && counter <= 1)
                {
                    radiusOffset = cats[catCounter].IrisSize - radius;
                    whiteOfTheEyeOffset = radiusOffset;
                }
                else if (counter >= 2 && counter <= 3)
                {
                    radiusOffset = whiteOfTheEyeOffset;
                }
                else if (counter >= 4 && counter <= 5)
                {
                    radiusOffset = cats[catCounter].PupilSize - radius;
                }

                Cylinder tmpC = new Cylinder(new Vertex(x - cats[catCounter].TorsoXOffset, y, z),
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
                    heigthOffset = cats[catCounter].EarsWidth - height;
                    x = x - heigthOffset;
                }

                HalfCylinder tmpHc = new HalfCylinder(new Vertex(x + cats[catCounter].TorsoXOffset, y, z),
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
                    if (cats[catCounter].BandsNumber < bands.Count)
                    {
                        for (int i = (bands.Count - 1); i >= cats[catCounter].BandsNumber; i--)
                        {
                            bands.RemoveAt(i);
                        }
                    }
                    else if (cats[catCounter].BandsNumber > bands.Count)
                    {
                        for (int i = (bands.Count - 1); i < cats[catCounter].BandsNumber - 1; i++)
                        {
                            bands.Add(new Box(new Vertex(bands[i].XStart - Cat.BandsXOffset, bands[i].YStart, bands[i].ZStart),
                                new Vertex(bands[i].XEnd - Cat.BandsXOffset, bands[i].YEnd, bands[i].ZEnd), bands[i].Color));
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
                double xOffset = (cats[catCounter].BandsWidth - b.getLength()) / 2;

                double x0 = b.XStart + cats[catCounter].TorsoXOffset - xOffset;
                double y0 = b.YStart;
                double z0 = b.ZStart;
                double x1 = b.XEnd + cats[catCounter].TorsoXOffset + xOffset;
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
                    cats[catCounter].FirstBandXStart = box.XStart;

                cats[catCounter].LastBandXMin = box.XStart;
                counter++;
            }
        }

        private void drawTeeth(List<Box> teeth, Matrix4 transform, double[] zBuffer)
        {
            if (teeth.Count != cats[catCounter].TeethNumber)
            {
                if (teeth.Count != 0)
                {
                    if (cats[catCounter].TeethNumber < teeth.Count)
                    {
                        for (int i = (teeth.Count - 1); i >= cats[catCounter].TeethNumber; i--)
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
                                teeth.Add(new Box(new Vertex(teeth[i].XStart, teeth[i].YStart - Cat.TeethYOffset, teeth[i].ZStart),
                                    new Vertex(teeth[i].XEnd, teeth[i].YEnd - Cat.TeethYOffset, teeth[i].ZEnd), teeth[i].Color));
                            }
                            else if (i % 2 != 0)
                            {
                                teeth.Add(new Box(new Vertex(teeth[i - 1].XStart, teeth[i - 1].YStart + Cat.TeethYOffset, teeth[i - 1].ZStart),
                                    new Vertex(teeth[i - 1].XEnd, teeth[i - 1].YEnd + Cat.TeethYOffset, teeth[i - 1].ZEnd), teeth[i - 1].Color));
                            }
                            else
                            {
                                teeth.Add(new Box(new Vertex(teeth[i - 1].XStart, teeth[i - 1].YStart - Cat.TeethYOffset, teeth[i - 1].ZStart),
                                    new Vertex(teeth[i - 1].XEnd, teeth[i - 1].YEnd - Cat.TeethYOffset, teeth[i - 1].ZEnd), teeth[i - 1].Color));
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

                x0 = b.XStart + cats[catCounter].TorsoXOffset;
                y0 = b.YStart < b.YEnd ? b.YStart - yOffset : b.YStart + yOffset;
                z0 = b.ZStart;
                x1 = b.XEnd + cats[catCounter].TorsoXOffset;
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
                    cats[catCounter].TeethFirstYMin = box.YStart;
                    cats[catCounter].TeethFirstYMax = box.YEnd;
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

            if (numericUpDown1.Value < (decimal)cat.DefaultHeadLength * 2 + 5 || numericUpDown1.Value > (decimal)cat.DefaultHeadLength * 10 + 10)
            {
                str = string.Format("Длина тела должна лежать в диапазоне от {0} до {1}", cat.DefaultHeadLength * 2 + 5, cat.DefaultHeadLength * 10 + 10);
                numericUpDown1.Value = (decimal)cat.TorsoLength;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                cat.TorsoLength = (double)numericUpDown1.Value;
            }

            if (numericUpDown2.Value < (decimal)cat.DefaultHeadWidth || numericUpDown2.Value > (decimal)cat.DefaultHeadWidth * 5)
            {
                str = string.Format("Ширина тела должна лежать в диапазоне от {0} до {1}", cat.DefaultHeadWidth, cat.DefaultHeadWidth * 5);
                numericUpDown2.Value = (decimal)cat.TorsoWidth;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                cat.TorsoWidth = (double)numericUpDown2.Value;
            }

            if (numericUpDown3.Value < (decimal)Math.Round(cat.DefaultTailLength / 2) || numericUpDown3.Value > (decimal)cat.DefaultTailLength * 2)
            {
                str = string.Format("Длина хвоста должна лежать в диапазоне от {0} до {1}",
                    (decimal)Math.Round(cat.DefaultTailLength / 2), (decimal)cat.DefaultTailLength * 2);
                numericUpDown3.Value = (decimal)cat.TailLength;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                cat.TailLength = (double)numericUpDown3.Value;
            }

            if (numericUpDown4.Value < (decimal)Math.Round(cat.DefaultTailWidth / 2) || numericUpDown4.Value > (decimal)cat.DefaultTailWidth * 2)
            {
                str = string.Format("Ширина хвоста должна лежать в диапазоне от {0} до {1}",
                    (decimal)Math.Round(cat.DefaultTailWidth / 2), (decimal)cat.DefaultTailWidth * 2);
                numericUpDown4.Value = (decimal)cat.TailWidth;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                cat.TailWidth = (double)numericUpDown4.Value;
            }

            if (numericUpDown5.Value < (decimal)cat.DefaultPawsHeight / 2 || numericUpDown5.Value > (decimal)cat.DefaultPawsHeight * 2)
            {
                str = string.Format("Длина лап должна лежать в диапазоне от {0} до {1}",
                    (decimal)cat.DefaultPawsHeight / 2, (decimal)cat.DefaultPawsHeight * 2);
                numericUpDown5.Value = (decimal)cat.PawsHeight;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                cat.PawsHeight = (double)numericUpDown5.Value;
            }

            if (numericUpDown6.Value < (decimal)cat.DefaultPawsWidth - 2 || numericUpDown6.Value > (decimal)cat.DefaultPawsWidth * 2)
            {
                str = string.Format("Ширина лап должна лежать в диапазоне от {0} до {1}",
                    (decimal)cat.DefaultPawsWidth - 2, (decimal)cat.DefaultPawsWidth * 2);
                numericUpDown6.Value = (decimal)cat.PawsWidth;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                cat.PawsWidth = (double)numericUpDown6.Value;
            }

            if (numericUpDown7.Value < (decimal)cat.DefaultIrisSize - 2 || numericUpDown7.Value > (decimal)cat.DefaultIrisSize + 2)
            {
                str = string.Format("Радиус глаз должен лежать в диапазоне от {0} до {1}",
                    (decimal)cat.DefaultIrisSize - 2, (decimal)cat.DefaultIrisSize + 2);
                numericUpDown7.Value = (decimal)cat.IrisSize;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                cat.IrisSize = (double)numericUpDown7.Value;
            }

            if (numericUpDown8.Value < (decimal)cat.DefaultPupilSize - 2 || numericUpDown8.Value > (decimal)cat.DefaultPupilSize + 2)
            {
                str = string.Format("Радиус зрачков должен лежать в диапазоне от {0} до {1}",
                    (decimal)cat.DefaultPupilSize - 2, (decimal)cat.DefaultPupilSize + 2);
                numericUpDown8.Value = (decimal)cat.PupilSize;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                cat.PupilSize = (double)numericUpDown8.Value;
            }

            isUpdated1 = true;
        }

        private void checkControlValues2(Cat cat)
        {
            string str;

            if (numericUpDown9.Value < Math.Round((decimal)cat.DefaultEarsWidth / 2) || numericUpDown9.Value > (decimal)cat.DefaultEarsWidth * 2)
            {
                str = string.Format("Ширина ушей должна лежать в диапазоне от {0} до {1}",
                    Math.Round((decimal)cat.DefaultEarsWidth / 2), (decimal)cat.DefaultEarsWidth * 2);
                numericUpDown9.Value = (decimal)cat.EarsWidth;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated2 = true;
                return;
            }
            else
            {
                cat.EarsWidth = (double)numericUpDown9.Value;
            }

            if (numericUpDown10.Value < (decimal)cat.DefaultTongueLength || numericUpDown10.Value > (decimal)cat.DefaultTongueLength + 5)
            {
                str = string.Format("Длина языка должна лежать в диапазоне от {0} до {1}",
                    (decimal)cat.DefaultTongueLength, (decimal)cat.DefaultTongueLength + 5);
                numericUpDown10.Value = (decimal)cat.TongueLength;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated2 = true;
                return;
            }
            else
            {
                cat.TongueLength = (double)numericUpDown10.Value;
            }

            if (numericUpDown11.Value < (decimal)cat.DefaultTongueWidth / 2 || numericUpDown11.Value > (decimal)cat.DefaultTongueWidth + 5)
            {
                str = string.Format("Ширина языка должна лежать в диапазоне от {0} до {1}",
                    (decimal)cat.DefaultTongueWidth / 2, (decimal)cat.DefaultTongueWidth + 5);
                numericUpDown11.Value = (decimal)cat.TongueWidth;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated2 = true;
                return;
            }
            else
            {
                cat.TongueWidth = (double)numericUpDown11.Value;
            }

            if (numericUpDown12.Value < (decimal)cat.DefaultTeethWidth - 1 || numericUpDown12.Value > (decimal)cat.DefaultTeethWidth + 1)
            {
                str = string.Format("Ширина зубов должна лежать в диапазоне от {0} до {1}",
                    (decimal)cat.DefaultTeethWidth - 1, (decimal)cat.DefaultTeethWidth + 1);
                numericUpDown12.Value = (decimal)cat.TeethWidth;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated2 = true;
                return;
            }
            else
            {
                cat.TeethWidth = (double)numericUpDown12.Value;
            }

            if (numericUpDown13.Value < Math.Round((decimal)cat.DefaultBandsWidth / 2) || numericUpDown13.Value > (decimal)cat.DefaultBandsWidth * 2)
            {
                str = string.Format("Толщина полос должна лежать в диапазоне от {0} до {1}",
                    Math.Round((decimal)cat.DefaultBandsWidth / 2), (decimal)cat.DefaultBandsWidth * 2);
                numericUpDown13.Value = (decimal)cat.BandsWidth;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated2 = true;
                return;
            }
            else
            {
                cat.BandsWidth = (double)numericUpDown13.Value;
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
                    double x = cat.FirstBandXStart;

                    for (int i = (cat.Bands.Count); i < (int)numericUpDown14.Value - 1; i++)
                    {
                        if (x - Cat.BandsXOffset < cat.TorsoXMin + 10)
                        {
                            str = string.Format("Количество полос должно быть в диапазоне от {0} до {1}",
                                0, i + 1);
                            numericUpDown14.Value = (decimal)cat.BandsNumber;
                            MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            isUpdated3 = true;
                            return;
                        }
                        else
                        {
                            x -= Cat.BandsXOffset;
                        }
                    }

                    cat.BandsNumber = (double)numericUpDown14.Value;
                }
                else
                {
                    double x = cat.LastBandXMin;

                    for (int i = (cat.Bands.Count - 1); i < (int)numericUpDown14.Value - 1; i++)
                    {
                        if (x - Cat.BandsXOffset < cat.TorsoXMin + 10)
                        {
                            str = string.Format("Количество полос должно быть в диапазоне от {0} до {1}",
                                0, i + 1);
                            numericUpDown14.Value = (decimal)cat.BandsNumber;
                            MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            isUpdated3 = true;
                            return;
                        }
                        else
                        {
                            x -= Cat.BandsXOffset;
                        }
                    }

                    cat.BandsNumber = (double)numericUpDown14.Value;
                }
            }
            else
            {
                numericUpDown14.Value = (decimal)cat.BandsNumber;
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
                        cat.TeethNumber = (double)numericUpDown15.Value;
                    }
                    else
                    {
                        str = string.Format("Количество зубов должно быть в диапазоне от {0} до {1}",
                                0, 4);
                        numericUpDown15.Value = (decimal)cat.TeethNumber;
                        MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isUpdated3 = true;
                        return;
                    }
                }
                else
                {
                    if (cat.Teeth.Count == 1)
                    {
                        teethYMax = cat.TeethFirstYMax;
                        teethYMin = cat.TeethFirstYMin;
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
                            if (teethYMax + Cat.TeethYOffset < cat.Boxes[13].YEnd - 2)
                            {
                                teethYMax += Cat.TeethYOffset;
                            }
                            else
                            {
                                str = string.Format("Количество зубов должно быть в диапазоне от {0} до {1}",
                                0, i + 1);
                                numericUpDown15.Value = (decimal)cat.TeethNumber;
                                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                isUpdated3 = true;
                                return;
                            }
                        }
                        else
                        {
                            if (teethYMin - Cat.TeethYOffset > cat.Boxes[13].YStart + 2)
                            {
                                teethYMin -= Cat.TeethYOffset;
                            }
                            else
                            {
                                str = string.Format("Количество зубов должно быть в диапазоне от {0} до {1}",
                                0, i + 1);
                                numericUpDown15.Value = (decimal)cat.TeethNumber;
                                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                isUpdated3 = true;
                                return;
                            }
                        }
                    }

                    cat.TeethNumber = (double)numericUpDown15.Value;
                }
            }
            else
            {
                numericUpDown15.Value = (decimal)cat.TeethNumber;
                MessageBox.Show("Количество зубов не может быть меньше 0", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated3 = true;
                return;
            }

            isUpdated3 = true;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < cats.Count; i++)
            {
                if (catsListBox.SelectedItem.ToString() == cats[i].Name)
                {
                    // устанавливаем количественные и качественные параметры
                    if (isUpdated1 == false)
                        checkControlValues1(cats[i]);

                    if (isUpdated2 == false)
                        checkControlValues2(cats[i]);

                    if (isUpdated3 == false)
                        checkControlValues3(cats[i]);

                    // устанавливаем сдвиги
                    if (lastSelectedListBoxIndex == i)
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

            renderArea = new Bitmap(renderPictureBox.Size.Width, renderPictureBox.Size.Height);
            renderPictureBox.Image = renderArea;

            Graphics g;
            g = Graphics.FromImage(renderArea);

            // рисование фона
            g.FillRectangle(Brushes.Black, new RectangleF(0, 0, renderArea.Width, renderArea.Height));

            matrices.Clear();
            // генерируем матрицу для каждого объекта
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
                catCounter = i;
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
            cats[cats.Count - 1].Name = "Cat" + catNameNumber;
            catNameNumber++;
            catsListBox.Items.Add(cats[cats.Count - 1].Name);
            catsListBox.SelectedItem = catsListBox.Items[cats.Count - 1];
        }

        private void catsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                setControls(cats[catsListBox.SelectedIndex]);

                lastSelectedListBoxIndex = catsListBox.SelectedIndex;
            }
            catch (ArgumentOutOfRangeException)
            {
                clearControls();

                lastSelectedListBoxIndex = 0;
            }
        }

        private void deleteCatButton_Click(object sender, EventArgs e)
        {
            if (catsListBox.SelectedIndex > 0)
            {
                catsListBox.SelectedIndex = catsListBox.SelectedIndex - 1;
                catsListBox.Items.RemoveAt(catsListBox.SelectedIndex + 1);
                cats.RemoveAt(catsListBox.SelectedIndex + 1);
                setControls(cats[catsListBox.SelectedIndex]);
            }
            if (catsListBox.SelectedIndex == 0)
            {
                if (catsListBox.Items.Count > 0)
                {
                    catsListBox.SelectedIndex = 1;
                    catsListBox.Items.RemoveAt(0);
                    cats.RemoveAt(0);
                    setControls(cats[catsListBox.SelectedIndex]);
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
