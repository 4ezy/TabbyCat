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

        double height;

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

        public Cylinder(Vertex center, double radius, double height, double vertexCount, Color color)
        {
            this.bottomBase = new List<Triangle>();
            this.surface = new List<Triangle>();
            this.topBase = new List<Triangle>();
            this.color = color;
            this.center = center;
            this.height = height;

            bottomBase = getBase(center, radius, vertexCount, color);
            topBase = getBase(new Vertex(center.X, center.Y, center.Z + height), radius, vertexCount, color);
            surface = getSurface(bottomBase, topBase, color);
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
