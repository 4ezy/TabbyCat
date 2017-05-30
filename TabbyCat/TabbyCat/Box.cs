using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TabbyCat
{
    struct Box
    {
        List<Triangle> bottomEdge;
        List<Triangle> topEdge;
        List<Triangle> leftEdge;
        List<Triangle> rightEdge;
        List<Triangle> nearEdge;
        List<Triangle> distantEdge;

        double xStart;
        double yStart;
        double zStart;
        double xEnd;
        double yEnd;
        double zEnd;

        Color color;

        internal List<Triangle> BottomEdge
        {
            get
            {
                return bottomEdge;
            }

            set
            {
                bottomEdge = value;
            }
        }

        internal List<Triangle> TopEdge
        {
            get
            {
                return topEdge;
            }

            set
            {
                topEdge = value;
            }
        }

        internal List<Triangle> LeftEdge
        {
            get
            {
                return leftEdge;
            }

            set
            {
                leftEdge = value;
            }
        }

        internal List<Triangle> RightEdge
        {
            get
            {
                return rightEdge;
            }

            set
            {
                rightEdge = value;
            }
        }

        internal List<Triangle> NearEdge
        {
            get
            {
                return nearEdge;
            }

            set
            {
                nearEdge = value;
            }
        }

        internal List<Triangle> DistantEdge
        {
            get
            {
                return distantEdge;
            }

            set
            {
                distantEdge = value;
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public double XStart
        {
            get
            {
                return xStart;
            }

            set
            {
                xStart = value;
            }
        }

        public double YStart
        {
            get
            {
                return yStart;
            }

            set
            {
                yStart = value;
            }
        }

        public double ZStart
        {
            get
            {
                return zStart;
            }

            set
            {
                zStart = value;
            }
        }

        public double XEnd
        {
            get
            {
                return xEnd;
            }

            set
            {
                xEnd = value;
            }
        }

        public double YEnd
        {
            get
            {
                return yEnd;
            }

            set
            {
                yEnd = value;
            }
        }

        public double ZEnd
        {
            get
            {
                return zEnd;
            }

            set
            {
                zEnd = value;
            }
        }

        public void setBottomEdge(double x1, double z1, double x2, double z2, double y1, double stepX, double stepZ, Color color)
        {
            bottomEdge.Clear();

            for (double i = x1; i != x2; i += stepX)
            {
                for (double j = z1; j != z2; j += stepZ)
                {
                    this.bottomEdge.Add(new Triangle(
                        new Vertex(i, y1, j),
                        new Vertex(i, y1, j + stepZ),
                        new Vertex(i + stepX, y1, j),
                        color
                    ));
                }
            }

            for (double i = x2; i != x1; i -= stepX)
            {
                for (double j = z2; j != z1; j -= stepZ)
                {
                    this.bottomEdge.Add(new Triangle(
                        new Vertex(i, y1, j),
                        new Vertex(i, y1, j - stepZ),
                        new Vertex(i - stepX, y1, j),
                        color
                    ));
                }
            }
        }

        public void setTopEdge(double x1, double z1, double x2, double z2, double y2, double stepX, double stepZ, Color color)
        {
            topEdge.Clear();

            for (double i = x1; i != x2; i += stepX)
            {
                for (double j = z1; j != z2; j += stepZ)
                {
                    this.topEdge.Add(new Triangle(
                        new Vertex(i, y2, j),
                        new Vertex(i, y2, j + stepZ),
                        new Vertex(i + stepX, y2, j),
                        color
                    ));
                }
            }

            for (double i = x2; i != x1; i -= stepX)
            {
                for (double j = z2; j != z1; j -= stepZ)
                {
                    this.topEdge.Add(new Triangle(
                        new Vertex(i, y2, j),
                        new Vertex(i, y2, j - stepZ),
                        new Vertex(i - stepX, y2, j),
                        color
                    ));
                }
            }
        }

        public void setLeftEdge(double y1, double z1, double y2, double z2, double x1, double stepY, double stepZ, Color color)
        {
            leftEdge.Clear();

            for (double i = y1; i != y2; i += stepY)
            {
                for (double j = z1; j != z2; j += stepZ)
                {
                    this.leftEdge.Add(new Triangle(
                        new Vertex(x1, i, j),
                        new Vertex(x1, i, j + stepZ),
                        new Vertex(x1, i + stepY, j),
                        color
                    ));
                }
            }

            for (double i = y2; i != y1; i -= stepY)
            {
                for (double j = z2; j != z1; j -= stepZ)
                {
                    this.leftEdge.Add(new Triangle(
                        new Vertex(x1, i, j),
                        new Vertex(x1, i, j - stepZ),
                        new Vertex(x1, i - stepY, j),
                        color
                    ));
                }
            }
        }

        public void setRightEdge(double y1, double z1, double y2, double z2, double x2, double stepY, double stepZ, Color color)
        {
            rightEdge.Clear();

            for (double i = y1; i != y2; i += stepY)
            {
                for (double j = z1; j != z2; j += stepZ)
                {
                    this.rightEdge.Add(new Triangle(
                        new Vertex(x2, i, j),
                        new Vertex(x2, i, j + stepZ),
                        new Vertex(x2, i + stepY, j),
                        color
                    ));
                }
            }

            for (double i = y2; i != y1; i -= stepY)
            {
                for (double j = z2; j != z1; j -= stepZ)
                {
                    this.rightEdge.Add(new Triangle(
                        new Vertex(x2, i, j),
                        new Vertex(x2, i, j - stepZ),
                        new Vertex(x2, i - stepY, j),
                        color
                    ));
                }
            }
        }

        public void setNearEdge(double x1, double y1, double x2, double y2, double z1, double stepX, double stepY, Color color)
        {
            nearEdge.Clear();

            for (double i = x1; i != x2; i += stepX)
            {
                for (double j = y1; j != y2; j += stepY)
                {
                    this.nearEdge.Add(new Triangle(
                        new Vertex(i, j, z1),
                        new Vertex(i, j + stepY, z1),
                        new Vertex(i + stepX, j, z1),
                        color
                    ));
                }
            }

            for (double i = x2; i != x1; i -= stepX)
            {
                for (double j = y2; j != y1; j -= stepY)
                {
                    this.nearEdge.Add(new Triangle(
                        new Vertex(i, j, z1),
                        new Vertex(i, j - stepY, z1),
                        new Vertex(i - stepX, j, z1),
                        color
                    ));
                }
            }
        }

        public void setDistantEdge(double x1, double y1, double x2, double y2, double z2, double stepX, double stepY, Color color)
        {
            distantEdge.Clear();

            for (double i = x1; i != x2; i += stepX)
            {
                for (double j = y1; j != y2; j += stepY)
                {
                    this.distantEdge.Add(new Triangle(
                        new Vertex(i, j, z2),
                        new Vertex(i, j + stepY, z2),
                        new Vertex(i + stepX, j, z2),
                        color
                    ));
                }
            }

            for (double i = x2; i != x1; i -= stepX)
            {
                for (double j = y2; j != y1; j -= stepY)
                {
                    this.distantEdge.Add(new Triangle(
                        new Vertex(i, j, z2),
                        new Vertex(i, j - stepY, z2),
                        new Vertex(i - stepX, j, z2),
                        color
                    ));
                }
            }
        }

        public Box(Vertex v1, Vertex v2, Color color)
        {
            bottomEdge = new List<Triangle>();
            topEdge = new List<Triangle>();
            leftEdge = new List<Triangle>();
            rightEdge = new List<Triangle>();
            nearEdge = new List<Triangle>();
            distantEdge = new List<Triangle>();


            // x
            if (v1.X < 0 && v2.X < 0)
            {
                if (Math.Abs(v1.X) < Math.Abs(v2.X))
                {
                    xStart = v1.X;
                    xEnd = v2.X;
                }
                else if (Math.Abs(v1.X) > Math.Abs(v2.X))
                {
                    xStart = v2.X;
                    xEnd = v1.X;
                }
                else
                {
                    xStart = v1.X;
                    xEnd = v1.X;
                }
            }
            else
            {
                if (v1.X < v2.X)
                {
                    xStart = v1.X;
                    xEnd = v2.X;
                }
                else if (v1.X > v2.X)
                {
                    xStart = v2.X;
                    xEnd = v1.X;
                }
                else
                {
                    xStart = v1.X;
                    xEnd = v1.X;
                }
            }

            // y
            if (v1.Y < 0 && v2.Y < 0)
            {
                if (Math.Abs(v1.Y) < Math.Abs(v2.Y))
                {
                    yStart = v1.Y;
                    yEnd = v2.Y;
                }
                else if (Math.Abs(v1.Y) > Math.Abs(v2.Y))
                {
                    yStart = v2.Y;
                    yEnd = v1.Y;
                }
                else
                {
                    yStart = v1.Y;
                    yEnd = v1.Y;
                }
            }
            else
            {
                if (v1.Y < v2.Y)
                {
                    yStart = v1.Y;
                    yEnd = v2.Y;
                }
                else if (v1.Y > v2.Y)
                {
                    yStart = v2.Y;
                    yEnd = v1.Y;
                }
                else
                {
                    yStart = v1.Y;
                    yEnd = v1.Y;
                }
            }

            // z
            if (v1.Z < 0 && v2.Z < 0)
            {
                if (Math.Abs(v1.Z) < Math.Abs(v2.Z))
                {
                    zStart = v1.Z;
                    zEnd = v2.Z;
                }
                else if (Math.Abs(v1.Z) > Math.Abs(v2.Z))
                {
                    zStart = v2.Z;
                    zEnd = v1.Z;
                }
                else
                {
                    zStart = v1.Z;
                    zEnd = v1.Z;
                }
            }
            else
            {
                if (v1.Z < v2.Z)
                {
                    zStart = v1.Z;
                    zEnd = v2.Z;
                }
                else if (v1.Z > v2.Z)
                {
                    zStart = v2.Z;
                    zEnd = v1.Z;
                }
                else
                {
                    zStart = v1.Z;
                    zEnd = v1.Z;
                }
            }

            this.color = color;

            double stepX = xEnd - xStart;
            double stepY = yEnd - yStart;
            double stepZ = zEnd - zStart;

            setBottomEdge(xStart, zStart, xEnd, zEnd, yStart, stepX, stepZ, color);

            setLeftEdge(yStart, zStart, yEnd, zEnd, xStart, stepY, stepZ, color);

            setRightEdge(yStart, zStart, yEnd, zEnd, xEnd, stepY, stepZ, color);

            setDistantEdge(xStart, yStart, xEnd, yEnd, zEnd, stepX, stepY, color);

            setNearEdge(xStart, yStart, xEnd, yEnd, zStart, stepX, stepY, color);

            setTopEdge(xStart, zStart, xEnd, zEnd, yEnd, stepX, stepZ, color);
        }

        // длина измеряется по оси x
        public double getLength()
        {
            double length = this.xStart < this.xEnd ?
                Math.Abs(this.xEnd - this.xStart) :
                Math.Abs(this.xStart - this.xEnd);

            return length;
        }

        // ширина измеряется по оси y
        public double getWidth()
        {
            double width = this.yStart < this.yEnd ?
                Math.Abs(this.yEnd - this.yStart) :
                Math.Abs(this.yStart - this.yEnd);

            return width;
        }

        // высота измеряется по оси z
        public double getHeight()
        {
            double height = this.zStart < this.zEnd ?
                Math.Abs(this.zEnd - this.zStart) :
                Math.Abs(this.zStart - this.zEnd);

            return height;
        }
    }
}