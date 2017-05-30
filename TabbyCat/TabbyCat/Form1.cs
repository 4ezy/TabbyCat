using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

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

            parallelRadioButton.Checked = true;
        }

        private void setControls(Cat cat)
        {
            torsoLengthControl.Value = (decimal)cat.TorsoLength;
            torsoWidthControl.Value = (decimal)cat.TorsoWidth;
            tailLengthControl.Value = (decimal)cat.TailLength;
            tailWidthControl.Value = (decimal)cat.TailWidth;
            pawsHeightControl.Value = (decimal)cat.PawsHeight;
            pawsWidthControl.Value = (decimal)cat.PawsWidth;
            irisSizeControl.Value = (decimal)cat.IrisSize;
            pupilSizeControl.Value = (decimal)cat.PupilSize;
            earsDepthControl.Value = (decimal)cat.EarsWidth;
            tongueLengthControl.Value = (decimal)cat.TongueLength;
            tongueWidthControl.Value = (decimal)cat.TongueWidth;
            teethWidthControl.Value = (decimal)cat.TeethWidth;
            bandsDepthControl.Value = (decimal)cat.BandsWidth;
            bandsNumberControl.Value = (decimal)cat.BandsNumber;
            teethNumberControl.Value = (decimal)cat.TeethNumber;

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
            torsoLengthControl.Value = 0;
            torsoWidthControl.Value = 0;
            tailLengthControl.Value = 0;
            tailWidthControl.Value = 0;
            pawsHeightControl.Value = 0;
            pawsWidthControl.Value = 0;
            irisSizeControl.Value = 0;
            pupilSizeControl.Value = 0;
            earsDepthControl.Value = 0;
            tongueLengthControl.Value = 0;
            tongueWidthControl.Value = 0;
            teethWidthControl.Value = 0;
            bandsDepthControl.Value = 0;
            bandsNumberControl.Value = 0;
            teethNumberControl.Value = 0;

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

                if (centralRadioButton.Checked)
                {
                    double d = 1;
                    double a = (1000 + d) / (1000 - d);
                    double b = -2 * 1000 * d / (1000 - d);
                    d = -renderArea.Width / 2 * Math.Tan(RotationTransformation.degreeToRadian(45));
                    double z = v1.Z == 0 ? -0.1 : v1.Z;
                    v1.X = d * v1.X / z;
                    v1.Y = d * v1.Y / z;
                    v1.Z = (a * (v1.Z) + b) / z;

                    z = v2.Z == 0 ? -0.1 : v2.Z;
                    v2.X = d * v2.X / z;
                    v2.Y = d * v2.Y / z;
                    v2.Z = (a * (v2.Z) + b) / z;

                    z = v3.Z == 0 ? -0.1 : v3.Z;
                    v3.X = d * v3.X / z;
                    v3.Y = d * v3.Y / z;
                    v3.Z = (a * (v3.Z) + b) / z;

                    d = 1;
                    v1.X += renderArea.Width / 2 + v1.X * d / (v1.Z + d);
                    v1.Y += renderArea.Height / 2 + v1.Y * d / (v1.Z + d);
                    v2.X += renderArea.Width / 2 + v2.X * d / (v2.Z + d);
                    v2.Y += renderArea.Height / 2 + v2.Y * d / (v2.Z + d);
                    v3.X += renderArea.Width / 2 + v3.X * d / (v3.Z + d);
                    v3.Y += renderArea.Height / 2 + v3.Y * d / (v3.Z + d);
                }
                else
                {
                    v1.X += renderArea.Width / 2;
                    v1.Y += renderArea.Height / 2;
                    v2.X += renderArea.Width / 2;
                    v2.Y += renderArea.Height / 2;
                    v3.X += renderArea.Width / 2;
                    v3.Y += renderArea.Height / 2;
                }

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
                        for (int i = (teeth.Count - 1); i < cats[catCounter].TeethNumber - 1; i++)
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
                double yOffset = (cats[catCounter].TeethWidth - b.getWidth()) / 2;

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

            if (torsoLengthControl.Value < (decimal)cat.DefaultHeadLength * 2 + 5 || torsoLengthControl.Value > (decimal)cat.DefaultHeadLength * 10 + 10)
            {
                str = string.Format("Длина тела должна лежать в диапазоне от {0} до {1}", cat.DefaultHeadLength * 2 + 5, cat.DefaultHeadLength * 10 + 10);
                torsoLengthControl.Value = (decimal)cat.TorsoLength;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                cat.TorsoLength = (double)torsoLengthControl.Value;
            }

            if (torsoWidthControl.Value < (decimal)cat.DefaultHeadWidth || torsoWidthControl.Value > (decimal)cat.DefaultHeadWidth * 3)
            {
                str = string.Format("Ширина тела должна лежать в диапазоне от {0} до {1}", cat.DefaultHeadWidth, cat.DefaultHeadWidth * 3);
                torsoWidthControl.Value = (decimal)cat.TorsoWidth;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                cat.TorsoWidth = (double)torsoWidthControl.Value;
            }

            if (tailLengthControl.Value < (decimal)Math.Round(cat.DefaultTailLength / 2) || tailLengthControl.Value > (decimal)cat.DefaultTailLength * 2)
            {
                str = string.Format("Длина хвоста должна лежать в диапазоне от {0} до {1}",
                    (decimal)Math.Round(cat.DefaultTailLength / 2), (decimal)cat.DefaultTailLength * 2);
                tailLengthControl.Value = (decimal)cat.TailLength;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                cat.TailLength = (double)tailLengthControl.Value;
            }

            if (tailWidthControl.Value < (decimal)Math.Round(cat.DefaultTailWidth / 2) || tailWidthControl.Value > (decimal)cat.DefaultTailWidth * 2)
            {
                str = string.Format("Ширина хвоста должна лежать в диапазоне от {0} до {1}",
                    (decimal)Math.Round(cat.DefaultTailWidth / 2), (decimal)cat.DefaultTailWidth * 2);
                tailWidthControl.Value = (decimal)cat.TailWidth;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                cat.TailWidth = (double)tailWidthControl.Value;
            }

            if (pawsHeightControl.Value < (decimal)cat.DefaultPawsHeight / 2 || pawsHeightControl.Value > (decimal)cat.DefaultPawsHeight * 2)
            {
                str = string.Format("Длина лап должна лежать в диапазоне от {0} до {1}",
                    (decimal)cat.DefaultPawsHeight / 2, (decimal)cat.DefaultPawsHeight * 2);
                pawsHeightControl.Value = (decimal)cat.PawsHeight;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                cat.PawsHeight = (double)pawsHeightControl.Value;
            }

            if (pawsWidthControl.Value < (decimal)cat.DefaultPawsWidth - 2 || pawsWidthControl.Value > (decimal)cat.DefaultPawsWidth * 2)
            {
                str = string.Format("Ширина лап должна лежать в диапазоне от {0} до {1}",
                    (decimal)cat.DefaultPawsWidth - 2, (decimal)cat.DefaultPawsWidth * 2);
                pawsWidthControl.Value = (decimal)cat.PawsWidth;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                cat.PawsWidth = (double)pawsWidthControl.Value;
            }

            if (irisSizeControl.Value < (decimal)cat.DefaultIrisSize - 2 || irisSizeControl.Value > (decimal)cat.DefaultIrisSize + 2)
            {
                str = string.Format("Радиус глаз должен лежать в диапазоне от {0} до {1}",
                    (decimal)cat.DefaultIrisSize - 2, (decimal)cat.DefaultIrisSize + 2);
                irisSizeControl.Value = (decimal)cat.IrisSize;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                cat.IrisSize = (double)irisSizeControl.Value;
            }

            if (pupilSizeControl.Value < (decimal)cat.DefaultPupilSize - 2 || pupilSizeControl.Value > (decimal)cat.DefaultPupilSize + 2)
            {
                str = string.Format("Радиус зрачков должен лежать в диапазоне от {0} до {1}",
                    (decimal)cat.DefaultPupilSize - 2, (decimal)cat.DefaultPupilSize + 2);
                pupilSizeControl.Value = (decimal)cat.PupilSize;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated1 = true;
                return;
            }
            else
            {
                cat.PupilSize = (double)pupilSizeControl.Value;
            }

            isUpdated1 = true;
        }

        private void checkControlValues2(Cat cat)
        {
            string str;

            if (earsDepthControl.Value < Math.Round((decimal)cat.DefaultEarsWidth / 2) || earsDepthControl.Value > (decimal)cat.DefaultEarsWidth * 2)
            {
                str = string.Format("Ширина ушей должна лежать в диапазоне от {0} до {1}",
                    Math.Round((decimal)cat.DefaultEarsWidth / 2), (decimal)cat.DefaultEarsWidth * 2);
                earsDepthControl.Value = (decimal)cat.EarsWidth;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated2 = true;
                return;
            }
            else
            {
                cat.EarsWidth = (double)earsDepthControl.Value;
            }

            if (tongueLengthControl.Value < (decimal)cat.DefaultTongueLength || tongueLengthControl.Value > (decimal)cat.DefaultTongueLength + 5)
            {
                str = string.Format("Длина языка должна лежать в диапазоне от {0} до {1}",
                    (decimal)cat.DefaultTongueLength, (decimal)cat.DefaultTongueLength + 5);
                tongueLengthControl.Value = (decimal)cat.TongueLength;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated2 = true;
                return;
            }
            else
            {
                cat.TongueLength = (double)tongueLengthControl.Value;
            }

            if (tongueWidthControl.Value < (decimal)cat.DefaultTongueWidth / 2 || tongueWidthControl.Value > (decimal)cat.DefaultTongueWidth + 5)
            {
                str = string.Format("Ширина языка должна лежать в диапазоне от {0} до {1}",
                    (decimal)cat.DefaultTongueWidth / 2, (decimal)cat.DefaultTongueWidth + 5);
                tongueWidthControl.Value = (decimal)cat.TongueWidth;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated2 = true;
                return;
            }
            else
            {
                cat.TongueWidth = (double)tongueWidthControl.Value;
            }

            if (teethWidthControl.Value < (decimal)cat.DefaultTeethWidth - 1 || teethWidthControl.Value > (decimal)cat.DefaultTeethWidth + 1)
            {
                str = string.Format("Ширина зубов должна лежать в диапазоне от {0} до {1}",
                    (decimal)cat.DefaultTeethWidth - 1, (decimal)cat.DefaultTeethWidth + 1);
                teethWidthControl.Value = (decimal)cat.TeethWidth;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated2 = true;
                return;
            }
            else
            {
                cat.TeethWidth = (double)teethWidthControl.Value;
            }

            if (bandsDepthControl.Value < Math.Round((decimal)cat.DefaultBandsWidth / 2) || bandsDepthControl.Value > (decimal)cat.DefaultBandsWidth * 2)
            {
                str = string.Format("Толщина полос должна лежать в диапазоне от {0} до {1}",
                    Math.Round((decimal)cat.DefaultBandsWidth / 2), (decimal)cat.DefaultBandsWidth * 2);
                bandsDepthControl.Value = (decimal)cat.BandsWidth;
                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated2 = true;
                return;
            }
            else
            {
                cat.BandsWidth = (double)bandsDepthControl.Value;
            }

            isUpdated2 = true;
        }

        private void checkControlValues3(Cat cat)
        {
            string str;

            if (bandsNumberControl.Value >= 0)
            {
                if (cat.Bands.Count == 0)
                {
                    double x = cat.FirstBandXStart;

                    for (int i = (cat.Bands.Count); i < (int)bandsNumberControl.Value - 1; i++)
                    {
                        if (x - Cat.BandsXOffset < cat.TorsoXMin + 10)
                        {
                            str = string.Format("Количество полос должно быть в диапазоне от {0} до {1}",
                                0, i + 1);
                            bandsNumberControl.Value = (decimal)cat.BandsNumber;
                            MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            isUpdated3 = true;
                            return;
                        }
                        else
                        {
                            x -= Cat.BandsXOffset;
                        }
                    }

                    cat.BandsNumber = (double)bandsNumberControl.Value;
                }
                else
                {
                    double x = cat.LastBandXMin;

                    for (int i = (cat.Bands.Count - 1); i < (int)bandsNumberControl.Value - 1; i++)
                    {
                        if (x - Cat.BandsXOffset < cat.TorsoXMin + 10)
                        {
                            str = string.Format("Количество полос должно быть в диапазоне от {0} до {1}",
                                0, i + 1);
                            bandsNumberControl.Value = (decimal)cat.BandsNumber;
                            MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            isUpdated3 = true;
                            return;
                        }
                        else
                        {
                            x -= Cat.BandsXOffset;
                        }
                    }

                    cat.BandsNumber = (double)bandsNumberControl.Value;
                }
            }
            else
            {
                bandsNumberControl.Value = (decimal)cat.BandsNumber;
                MessageBox.Show("Количество полос не может быть меньше 0", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdated3 = true;
                return;
            }

            if (teethNumberControl.Value >= 0)
            {
                double teethYMin = 0;
                double teethYMax = 0;

                if (cat.Teeth.Count == 0)
                {
                    if (teethNumberControl.Value <= 4)
                    {
                        cat.TeethNumber = (double)teethNumberControl.Value;
                    }
                    else
                    {
                        str = string.Format("Количество зубов должно быть в диапазоне от {0} до {1}",
                                0, 4);
                        teethNumberControl.Value = (decimal)cat.TeethNumber;
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

                    for (int i = (cat.Teeth.Count - 1); i < (int)teethNumberControl.Value - 1; i++)
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
                                teethNumberControl.Value = (decimal)cat.TeethNumber;
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
                                teethNumberControl.Value = (decimal)cat.TeethNumber;
                                MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                isUpdated3 = true;
                                return;
                            }
                        }
                    }

                    cat.TeethNumber = (double)teethNumberControl.Value;
                }
            }
            else
            {
                teethNumberControl.Value = (decimal)cat.TeethNumber;
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
                if (catsListBox.SelectedIndex == lastSelectedListBoxIndex)
                {
                    if (isUpdated1 == false)
                        checkControlValues1(cats[catsListBox.SelectedIndex]);

                    if (isUpdated2 == false)
                        checkControlValues2(cats[catsListBox.SelectedIndex]);

                    if (isUpdated3 == false)
                        checkControlValues3(cats[catsListBox.SelectedIndex]);

                    cats[catsListBox.SelectedIndex].XOffset = xOffsetControl.Value;
                    cats[catsListBox.SelectedIndex].YOffset = yOffsetControl.Value;
                    cats[catsListBox.SelectedIndex].ZOffset = zOffsetControl.Value;
                    cats[catsListBox.SelectedIndex].XAngle = xAngleControl.Value;
                    cats[catsListBox.SelectedIndex].YAngle = yAngleControl.Value;
                    cats[catsListBox.SelectedIndex].ZAngle = zAngleControl.Value;
                    cats[catsListBox.SelectedIndex].ScaleOffset = scaleControl.Value;
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
                viewRtTransform.setAngles(cameraXAngleControl.Value, cameraYAngleControl.Value, cameraZAngleControl.Value);

                if (centralRadioButton.Checked)
                {
                    viewTrTransform.setOffsets(cameraXPositionControl.Value, cameraYPositionControl.Value, cameraZPositionControl.Value + 300);
                }
                else
                {
                    viewTrTransform.setOffsets(cameraXPositionControl.Value, cameraYPositionControl.Value, cameraZPositionControl.Value);
                }

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
            }
            else
            {
                if (catsListBox.Items.Count > 1)
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

        private void serializeObjects()
        {
            TextWriter writer = new StreamWriter(("cat.xml"));

            XmlSerializer ser = new XmlSerializer(typeof(List<Cat>));

            ser.Serialize(writer, cats);

            writer.Close();
        }

        private void deserialzeObjects()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Cat>));
            FileStream fs = new FileStream("cat.xml", FileMode.Open);
            XmlReader reader = XmlReader.Create(fs);
            cats.Clear();
            catsListBox.Items.Clear();
            cats = (List<Cat>)serializer.Deserialize(reader);
            fs.Close();
            
            foreach (Cat cat in cats)
            {
                catsListBox.Items.Add(cat.Name);
            }

            if (cats.Count != 0)
            {
                catsListBox.SelectedItem = catsListBox.Items[0];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serializeObjects();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            deserialzeObjects();
        }
    }
}
