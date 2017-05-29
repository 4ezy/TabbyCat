using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbyCat
{
    struct Cylinder
    {
        List<Triangle> bottomBase;
        List<Triangle> topBase;
        List<Triangle> surface;

        Vertex center;

        Color color;

        double radius;
        double height;
        double vertexCount;

        internal List<Triangle> BottomBase
        {
            get
            {
                return bottomBase;
            }

            set
            {
                bottomBase = value;
            }
        }

        internal List<Triangle> TopBase
        {
            get
            {
                return topBase;
            }

            set
            {
                topBase = value;
            }
        }

        internal List<Triangle> Surface
        {
            get
            {
                return surface;
            }

            set
            {
                surface = value;
            }
        }

        internal Vertex Center
        {
            get
            {
                return center;
            }

            set
            {
                center = value;
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

        public double Radius
        {
            get
            {
                return radius;
            }

            set
            {
                radius = value;
            }
        }

        public double Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
            }
        }

        public double VertexCount
        {
            get
            {
                return vertexCount;
            }

            set
            {
                vertexCount = value;
            }
        }

        public Cylinder(Vertex center, double radius, double height, double vertexCount, Color color)
        {
            double x = center.Z;
            double y = center.Y;
            double z = -center.X;

            this.bottomBase = new List<Triangle>();
            this.surface = new List<Triangle>();
            this.topBase = new List<Triangle>();
            this.color = color;
            this.center = center;
            this.radius = radius;
            this.height = height;
            this.vertexCount = vertexCount;

            this.bottomBase = getBase(new Vertex(x, y, z), radius, vertexCount, color);
            this.topBase = getBase(new Vertex(x, y, z + height), radius, vertexCount, color);
            this.surface = getSurface(bottomBase, topBase, color);

            rotateY(90);
        }

        public void rotateY(int degrees)
        {
            RotationTransformation rt = new RotationTransformation();
            rt.setAngles(0, (decimal)degreeToRadian(degrees), 0);

            Matrix4 transformMatrix = rt.OyMatrix.multiply(rt.OxMatrix);
            transformMatrix = transformMatrix.multiply(rt.OzMatrix);

            bottomBase = elementRotation(bottomBase, transformMatrix);
            topBase = elementRotation(topBase, transformMatrix);
            surface = elementRotation(surface, transformMatrix);
        }

        private List<Triangle> elementRotation(List<Triangle> element, Matrix4 transformMatrix)
        {
            for (int i = 0; i < element.Count; i++)
            {
                Vertex v1 = transformMatrix.transform(element[i].V1);
                Vertex v2 = transformMatrix.transform(element[i].V2);
                Vertex v3 = transformMatrix.transform(element[i].V3);

                element.RemoveAt(i);

                element.Insert(i, new Triangle(v1, v2, v3, color));
            }

            return element;
        }

        private List<Triangle> getBase(Vertex center, double radius, double vertexCount, Color color)
        {
            List<Vertex> verteces = new List<Vertex>();
            List<Triangle> triangles = new List<Triangle>();

            double angle = Math.PI * 2 / vertexCount;

            for (int i = 0; i < vertexCount; i++)
            {
                Vertex v = new Vertex(center.X + radius * Math.Cos(Math.PI - angle * i), center.Y + radius * Math.Sin(Math.PI - angle * i), center.Z);

                verteces.Add(v);
            }

            for (int i = 0; i < verteces.Count - 1; i++)
            {
                triangles.Add(new Triangle(center, verteces[i], verteces[i + 1], color));
            }

            triangles.Add(new Triangle(center, verteces[verteces.Count - 1], verteces[0], color));

            return triangles;
        }

        private List<Triangle> getSurface(List<Triangle> bottomBase, List<Triangle> topBase, Color color)
        {
            List<Triangle> triangles = new List<Triangle>();

            for (int i = 0; i < bottomBase.Count; i++)
            {
                triangles.Add(new Triangle(bottomBase[i].V2, topBase[i].V2, topBase[i].V3, color));
            }

            for (int i = 0; i < topBase.Count; i++)
            {
                triangles.Add(new Triangle(topBase[i].V3, bottomBase[i].V2, bottomBase[i].V3, color));
            }

            return triangles;
        }

        private double degreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
