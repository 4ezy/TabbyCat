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

        public void setBottomEdge(int x1, int z1, int x2, int z2, int y1, int stepX, int stepZ, Color color)
        {
            for (int i = x1; i != x2; i += stepX)
            {
                for (int j = z1; j != z2; j += stepZ)
                {
                    this.bottomEdge.Add(new Triangle(
                        new Vertex(i, y1, j, 1),
                        new Vertex(i, y1, j + stepZ, 1),
                        new Vertex(i + stepX, y1, j, 1),
                        color
                    ));
                }
            }

            for (int i = x2; i != x1; i -= stepX)
            {
                for (int j = z2; j != z1; j -= stepZ)
                {
                    this.bottomEdge.Add(new Triangle(
                        new Vertex(i, y1, j, 1),
                        new Vertex(i, y1, j - stepZ, 1),
                        new Vertex(i - stepX, y1, j, 1),
                        color
                    ));
                }
            }
        }

        public void setTopEdge(int x1, int z1, int x2, int z2, int y2, int stepX, int stepZ, Color color)
        {
            for (int i = x1; i != x2; i += stepX)
            {
                for (int j = z1; j != z2; j += stepZ)
                {
                    this.topEdge.Add(new Triangle(
                        new Vertex(i, y2, j, 1),
                        new Vertex(i, y2, j + stepZ, 1),
                        new Vertex(i + stepX, y2, j, 1),
                        color
                    ));
                }
            }

            for (int i = x2; i != x1; i -= stepX)
            {
                for (int j = z2; j != z1; j -= stepZ)
                {
                    this.topEdge.Add(new Triangle(
                        new Vertex(i, y2, j, 1),
                        new Vertex(i, y2, j - stepZ, 1),
                        new Vertex(i - stepX, y2, j, 1),
                        color
                    ));
                }
            }
        }

        public void setLeftEdge(int y1, int z1, int y2, int z2, int x1, int stepY, int stepZ, Color color)
        {
            for (int i = y1; i != y2; i += stepY)
            {
                for (int j = z1; j != z2; j += stepZ)
                {
                    this.leftEdge.Add(new Triangle(
                        new Vertex(x1, i, j, 1),
                        new Vertex(x1, i, j + stepZ, 1),
                        new Vertex(x1, i + stepY, j, 1),
                        color
                    ));
                }
            }

            for (int i = y2; i != y1; i -= stepY)
            {
                for (int j = z2; j != z1; j -= stepZ)
                {
                    this.leftEdge.Add(new Triangle(
                        new Vertex(x1, i, j, 1),
                        new Vertex(x1, i, j - stepZ, 1),
                        new Vertex(x1, i - stepY, j, 1),
                        color
                    ));
                }
            }
        }

        public void setRightEdge(int y1, int z1, int y2, int z2, int x2, int stepY, int stepZ, Color color)
        {
            for (int i = y1; i != y2; i += stepY)
            {
                for (int j = z1; j != z2; j += stepZ)
                {
                    this.rightEdge.Add(new Triangle(
                        new Vertex(x2, i, j, 1),
                        new Vertex(x2, i, j + stepZ, 1),
                        new Vertex(x2, i + stepY, j, 1),
                        color
                    ));
                }
            }

            for (int i = y2; i != y1; i -= stepY)
            {
                for (int j = z2; j != z1; j -= stepZ)
                {
                    this.rightEdge.Add(new Triangle(
                        new Vertex(x2, i, j, 1),
                        new Vertex(x2, i, j - stepZ, 1),
                        new Vertex(x2, i - stepY, j, 1),
                        color
                    ));
                }
            }
        }

        public void setNearEdge(int x1, int y1, int x2, int y2, int z1, int stepX, int stepY, Color color)
        {
            for (int i = x1; i != x2; i += stepX)
            {
                for (int j = y1; j != y2; j += stepY)
                {
                    this.nearEdge.Add(new Triangle(
                        new Vertex(i, j, z1, 1),
                        new Vertex(i, j + stepY, z1, 1),
                        new Vertex(i + stepX, j, z1, 1),
                        color
                    ));
                }
            }

            for (int i = x2; i != x1; i -= stepX)
            {
                for (int j = y2; j != y1; j -= stepY)
                {
                    this.nearEdge.Add(new Triangle(
                        new Vertex(i, j, z1, 1),
                        new Vertex(i, j - stepY, z1, 1),
                        new Vertex(i - stepX, j, z1, 1),
                        color
                    ));
                }
            }
        }

        public void setDistantEdge(int x1, int y1, int x2, int y2, int z2, int stepX, int stepY, Color color)
        {
            for (int i = x1; i != x2; i += stepX)
            {
                for (int j = y1; j != y2; j += stepY)
                {
                    this.distantEdge.Add(new Triangle(
                        new Vertex(i, j, z2, 1),
                        new Vertex(i, j + stepY, z2, 1),
                        new Vertex(i + stepX, j, z2, 1),
                        color
                    ));
                }
            }

            for (int i = x2; i != x1; i -= stepX)
            {
                for (int j = y2; j != y1; j -= stepY)
                {
                    this.distantEdge.Add(new Triangle(
                        new Vertex(i, j, z2, 1),
                        new Vertex(i, j - stepY, z2, 1),
                        new Vertex(i - stepX, j, z2, 1),
                        color
                    ));
                }
            }
        }

        public Box(double xStart, double yStart, double zStart, double xEnd, double yEnd, double zEnd, Color color)
        {
            bottomEdge = new List<Triangle>();
            topEdge = new List<Triangle>();
            leftEdge = new List<Triangle>();
            rightEdge = new List<Triangle>();
            nearEdge = new List<Triangle>();
            distantEdge = new List<Triangle>();

            int x1 = (int)(xStart);
            int y1 = (int)(yStart);
            int z1 = (int)(zStart);
            int x2 = (int)(xEnd);
            int y2 = (int)(yEnd);
            int z2 = (int)(zEnd);

            int stepX = x1 < x2 ? Math.Abs((x2 - x1)) : -Math.Abs((x1 - x2));
            int stepY = y1 < y2 ? Math.Abs((y2 - y1)) : -Math.Abs((y1 - y2));
            int stepZ = z1 < z2 ? Math.Abs((z2 - z1)) : -Math.Abs((z1 - z2));

            setBottomEdge(x1, z1, x2, z2, y1, stepX, stepZ, color);

            setLeftEdge(y1, z1, y2, z2, x1, stepY, stepZ, color);

            setRightEdge(y1, z1, y2, z2, x2, stepY, stepZ, color);

            setDistantEdge(x1, y1, x2, y2, z2, stepX, stepY, color);

            setNearEdge(x1, y1, x2, y2, z1, stepX, stepY, color);

            setTopEdge(x1, z1, x2, z2, y2, stepX, stepZ, color);
        }
    }
}