using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TabbyCat
{
    [Serializable]
    public class Cat
    {
        string name;

        List<Box> boxes = new List<Box>();
        List<Cylinder> cylinders = new List<Cylinder>();
        List<HalfCylinder> halfCylinders = new List<HalfCylinder>();
        List<Box> bands = new List<Box>();
        List<Box> teeth = new List<Box>();

        const int bandsXOffset = 15;
        const int teethYOffset = 6;

        // матрицы
        decimal xOffset;
        decimal yOffset;
        decimal zOffset;
        decimal scaleOffset;
        decimal xAngle;
        decimal yAngle;
        decimal zAngle;

        double torsoXOffset;
        double torsoYOffset;

        double defaultPawsHeight;
        double defaultPawsWidth;
        double defaultTailLength;
        double defaultTailWidth;
        double defaultHeadLength;
        double defaultHeadWidth;
        double defaultTorsoLength;
        double defaultTorsoWidth;
        double defaultIrisSize;
        double defaultPupilSize;
        double defaultEarsWidth;
        double defaultTongueLength;
        double defaultTongueWidth;
        double defaultBandsWidth;
        double defaultTeethWidth;

        double bandsNumber;
        double teethNumber;

        double torsoLength;
        double torsoWidth;
        double pawsHeight;
        double pawsWidth;
        double tailLength;
        double tailWidth;
        double irisSize;
        double pupilSize;
        double earsWidth;
        double tongueLength;
        double tongueWidth;
        double bandsWidth;
        double teethWidth;

        double torsoXMin;
        double lastBandXMin;
        double firstBandXStart;
        double teethFirstYMax;
        double teethFirstYMin;

        public Cat()
        {
            boxes = boxesDrafting(Color.Orange);
            cylinders = cylindersDrafting(Color.White);
            halfCylinders = halfCylindersDrafting(Color.DarkGray);
            bands = bandsDrafting(Color.Brown);
            teeth = teethDrafting(Color.White);

            XOffset = 0;
            YOffset = 0;
            ZOffset = 0;
            ScaleOffset = 1;
            XAngle = 0;
            YAngle = 0;
            ZAngle = 0;

            DefaultHeadLength = Boxes[9].getLength();
            DefaultHeadWidth = Boxes[9].getWidth();

            TorsoLength = Boxes[0].getLength();
            TorsoWidth = Boxes[0].getWidth();
            DefaultTorsoLength = TorsoLength;
            DefaultTorsoWidth = TorsoWidth;

            TailLength = Boxes[10].getLength() + Boxes[12].getLength();
            TailWidth = Boxes[10].getWidth();
            DefaultTailLength = TailLength;
            DefaultTailWidth = TailWidth;

            PawsHeight = Boxes[1].getHeight();
            PawsWidth = Boxes[1].getWidth();
            DefaultPawsHeight = PawsHeight;
            DefaultPawsWidth = PawsWidth;

            IrisSize = Cylinders[0].Radius;
            DefaultIrisSize = IrisSize;

            PupilSize = Cylinders[4].Radius;
            DefaultPupilSize = PupilSize;

            EarsWidth = HalfCylinders[0].Height;
            DefaultEarsWidth = EarsWidth;

            TongueLength = Boxes[14].getLength();
            TongueWidth = Boxes[14].getWidth();
            DefaultTongueLength = TongueLength;
            DefaultTongueWidth = TongueWidth;

            TeethWidth = Teeth[0].getWidth();
            DefaultTeethWidth = TeethWidth;

            BandsWidth = Bands[0].getLength();
            DefaultBandsWidth = BandsWidth;

            BandsNumber = Bands.Count;
            TeethNumber = Teeth.Count;

            TorsoXOffset = 0;
            TorsoYOffset = 0;
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

        public double TorsoLength
        {
            get
            {
                return torsoLength;
            }

            set
            {
                torsoLength = value;
            }
        }

        public double TorsoWidth
        {
            get
            {
                return torsoWidth;
            }

            set
            {
                torsoWidth = value;
            }
        }

        public double PawsHeight
        {
            get
            {
                return pawsHeight;
            }

            set
            {
                pawsHeight = value;
            }
        }

        public double PawsWidth
        {
            get
            {
                return pawsWidth;
            }

            set
            {
                pawsWidth = value;
            }
        }

        public double TailLength
        {
            get
            {
                return tailLength;
            }

            set
            {
                tailLength = value;
            }
        }

        public double TailWidth
        {
            get
            {
                return tailWidth;
            }

            set
            {
                tailWidth = value;
            }
        }

        public double IrisSize
        {
            get
            {
                return irisSize;
            }

            set
            {
                irisSize = value;
            }
        }

        public double PupilSize
        {
            get
            {
                return pupilSize;
            }

            set
            {
                pupilSize = value;
            }
        }

        public double EarsWidth
        {
            get
            {
                return earsWidth;
            }

            set
            {
                earsWidth = value;
            }
        }

        public double TongueLength
        {
            get
            {
                return tongueLength;
            }

            set
            {
                tongueLength = value;
            }
        }

        public double TongueWidth
        {
            get
            {
                return tongueWidth;
            }

            set
            {
                tongueWidth = value;
            }
        }

        public double BandsWidth
        {
            get
            {
                return bandsWidth;
            }

            set
            {
                bandsWidth = value;
            }
        }

        public double TeethWidth
        {
            get
            {
                return teethWidth;
            }

            set
            {
                teethWidth = value;
            }
        }

        [XmlIgnore]
        public double TorsoXMin
        {
            get
            {
                return torsoXMin;
            }

            set
            {
                torsoXMin = value;
            }
        }

        public double BandsNumber
        {
            get
            {
                return bandsNumber;
            }

            set
            {
                bandsNumber = value;
            }
        }

        public double TeethNumber
        {
            get
            {
                return teethNumber;
            }

            set
            {
                teethNumber = value;
            }
        }

        [XmlIgnore]
        public double DefaultPawsHeight
        {
            get
            {
                return defaultPawsHeight;
            }

            set
            {
                defaultPawsHeight = value;
            }
        }

        [XmlIgnore]
        public double DefaultPawsWidth
        {
            get
            {
                return defaultPawsWidth;
            }

            set
            {
                defaultPawsWidth = value;
            }
        }

        [XmlIgnore]
        public double DefaultTailLength
        {
            get
            {
                return defaultTailLength;
            }

            set
            {
                defaultTailLength = value;
            }
        }

        [XmlIgnore]
        public double DefaultTailWidth
        {
            get
            {
                return defaultTailWidth;
            }

            set
            {
                defaultTailWidth = value;
            }
        }

        [XmlIgnore]
        public double DefaultHeadLength
        {
            get
            {
                return defaultHeadLength;
            }

            set
            {
                defaultHeadLength = value;
            }
        }

        [XmlIgnore]
        public double DefaultHeadWidth
        {
            get
            {
                return defaultHeadWidth;
            }

            set
            {
                defaultHeadWidth = value;
            }
        }

        [XmlIgnore]
        public double DefaultTorsoLength
        {
            get
            {
                return defaultTorsoLength;
            }

            set
            {
                defaultTorsoLength = value;
            }
        }

        [XmlIgnore]
        public double DefaultTorsoWidth
        {
            get
            {
                return defaultTorsoWidth;
            }

            set
            {
                defaultTorsoWidth = value;
            }
        }

        [XmlIgnore]
        public double DefaultIrisSize
        {
            get
            {
                return defaultIrisSize;
            }

            set
            {
                defaultIrisSize = value;
            }
        }

        [XmlIgnore]
        public double DefaultPupilSize
        {
            get
            {
                return defaultPupilSize;
            }

            set
            {
                defaultPupilSize = value;
            }
        }

        [XmlIgnore]
        public double DefaultEarsWidth
        {
            get
            {
                return defaultEarsWidth;
            }

            set
            {
                defaultEarsWidth = value;
            }
        }

        [XmlIgnore]
        public double DefaultTongueLength
        {
            get
            {
                return defaultTongueLength;
            }

            set
            {
                defaultTongueLength = value;
            }
        }

        [XmlIgnore]
        public double DefaultTongueWidth
        {
            get
            {
                return defaultTongueWidth;
            }

            set
            {
                defaultTongueWidth = value;
            }
        }

        [XmlIgnore]
        public double DefaultBandsWidth
        {
            get
            {
                return defaultBandsWidth;
            }

            set
            {
                defaultBandsWidth = value;
            }
        }

        [XmlIgnore]
        public double DefaultTeethWidth
        {
            get
            {
                return defaultTeethWidth;
            }

            set
            {
                defaultTeethWidth = value;
            }
        }

        [XmlIgnore]
        public double LastBandXMin
        {
            get
            {
                return lastBandXMin;
            }

            set
            {
                lastBandXMin = value;
            }
        }

        [XmlIgnore]
        public double FirstBandXStart
        {
            get
            {
                return firstBandXStart;
            }

            set
            {
                firstBandXStart = value;
            }
        }

        [XmlIgnore]
        public static int BandsXOffset
        {
            get
            {
                return bandsXOffset;
            }
        }

        [XmlIgnore]
        public static int TeethYOffset
        {
            get
            {
                return teethYOffset;
            }
        }

        [XmlIgnore]
        public double TeethFirstYMax
        {
            get
            {
                return teethFirstYMax;
            }

            set
            {
                teethFirstYMax = value;
            }
        }

        [XmlIgnore]
        public double TeethFirstYMin
        {
            get
            {
                return teethFirstYMin;
            }

            set
            {
                teethFirstYMin = value;
            }
        }

        [XmlIgnore]
        public double TorsoXOffset
        {
            get
            {
                return torsoXOffset;
            }

            set
            {
                torsoXOffset = value;
            }
        }

        [XmlIgnore]
        public double TorsoYOffset
        {
            get
            {
                return torsoYOffset;
            }

            set
            {
                torsoYOffset = value;
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
