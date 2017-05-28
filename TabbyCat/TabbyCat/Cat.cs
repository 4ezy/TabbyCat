using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbyCat
{
    class Cat
    {
        List<Box> boxes = new List<Box>();
        List<Cylinder> cylinders = new List<Cylinder>();
        List<HalfCylinder> halfCylinders = new List<HalfCylinder>();
        List<Box> bands = new List<Box>();
        List<Box> teeth = new List<Box>();

        decimal xOffset;
        decimal yOffset;
        decimal zOffset;
        decimal scaleOffset;
        decimal xAngle;
        decimal yAngle;
        decimal zAngle;

        bool isChecked;
        string name;

        public Cat()
        {
            boxes = boxesDrafting(Color.Orange);
            cylinders = cylindersDrafting(Color.White);
            halfCylinders = halfCylindersDrafting(Color.DarkGray);
            bands = bandsDrafting(Color.Brown);
            teeth = teethDrafting(Color.White);

            xOffset = 0;
            yOffset = 0;
            zOffset = 0;
            scaleOffset = 1;
            xAngle = 0;
            yAngle = 0;
            zAngle = 0;

            isChecked = false;
        }

        internal List<Box> Boxes
        {
            get
            {
                return boxes;
            }

            set
            {
                boxes = value;
            }
        }

        internal List<Cylinder> Cylinders
        {
            get
            {
                return cylinders;
            }

            set
            {
                cylinders = value;
            }
        }

        internal List<HalfCylinder> HalfCylinders
        {
            get
            {
                return halfCylinders;
            }

            set
            {
                halfCylinders = value;
            }
        }

        internal List<Box> Bands
        {
            get
            {
                return bands;
            }

            set
            {
                bands = value;
            }
        }

        internal List<Box> Teeth
        {
            get
            {
                return teeth;
            }

            set
            {
                teeth = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }

            set
            {
                isChecked = value;
            }
        }

        public decimal XOffset
        {
            get
            {
                return xOffset;
            }

            set
            {
                xOffset = value;
            }
        }

        public decimal YOffset
        {
            get
            {
                return yOffset;
            }

            set
            {
                yOffset = value;
            }
        }

        public decimal ZOffset
        {
            get
            {
                return zOffset;
            }

            set
            {
                zOffset = value;
            }
        }

        public decimal ScaleOffset
        {
            get
            {
                return scaleOffset;
            }

            set
            {
                scaleOffset = value;
            }
        }

        public decimal XAngle
        {
            get
            {
                return xAngle;
            }

            set
            {
                xAngle = value;
            }
        }

        public decimal YAngle
        {
            get
            {
                return yAngle;
            }

            set
            {
                yAngle = value;
            }
        }

        public decimal ZAngle
        {
            get
            {
                return zAngle;
            }

            set
            {
                zAngle = value;
            }
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
            List<HalfCylinder> halfCylinders = new List<HalfCylinder>();

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
            boxes.Add(new Box(new Vertex(60, 10, 60), new Vertex(65, -20, 61), color));
            boxes.Add(new Box(new Vertex(45, 10, 60), new Vertex(50, -20, 61), color));

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
    }
}
