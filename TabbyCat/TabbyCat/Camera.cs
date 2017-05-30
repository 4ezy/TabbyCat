using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbyCat
{
    class Camera
    {
        Vertex observerPoint;
        Vertex observingPoint;

        double near;
        double far;
        double width;
        double height;
        double fov;

        internal Vertex ObserverPoint
        {
            get
            {
                return observerPoint;
            }

            set
            {
                observerPoint = value;
            }
        }

        internal Vertex ObservingPoint
        {
            get
            {
                return observingPoint;
            }

            set
            {
                observingPoint = value;
            }
        }

        public double Near
        {
            get
            {
                return near;
            }

            set
            {
                near = value;
            }
        }

        public double Far
        {
            get
            {
                return far;
            }

            set
            {
                far = value;
            }
        }

        public double Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
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

        public double Fov
        {
            get
            {
                return fov;
            }

            set
            {
                fov = value;
            }
        }

        public Camera(Vertex observerPoint, Vertex observingPoint, double width, double height)
        {
            this.observerPoint = observerPoint;
            this.observingPoint = observingPoint;
            this.Near = 0.1;
            this.Far = (observerPoint - observingPoint).Length();
            this.Width = width;
            this.Height = Height;
            this.Fov = 67;
        }
    }
}
