using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TabbyCat
{
    public partial class Form1 : Form
    {
        Bitmap renderArea;

        public Form1()
        {
            InitializeComponent();
        }

        private double degreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private List<Triangle> tetrahedronDrafting()
        {
            List<Triangle> tris = new List<Triangle>();

            tris.Add(new Triangle(new Vertex(100, 100, 100),
                new Vertex(-100, -100, 100),
                new Vertex(-100, 100, -100),
                Color.White));

            tris.Add(new Triangle(new Vertex(100, 100, 100),
                new Vertex(-100, -100, 100),
                new Vertex(100, -100, -100),
                Color.Red));

            tris.Add(new Triangle(new Vertex(-100, 100, -100),
                new Vertex(100, -100, -100),
                new Vertex(100, 100, 100),
                Color.Green));

            tris.Add(new Triangle(new Vertex(-100, 100, -100),
                new Vertex(100, -100, -100),
                new Vertex(-100, -100, 100),
                Color.Blue));

            return tris;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            List<Triangle> tris = tetrahedronDrafting();

            renderArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = renderArea;

            Graphics g;
            g = Graphics.FromImage(renderArea);

            g.FillRectangle(Brushes.Black, new RectangleF(0, 0, renderArea.Width, renderArea.Height));

            double horizontal = degreeToRadian(trackBar1.Value);

            Matrix3 horizTansform = new Matrix3(new double[] {Math.Cos(horizontal), 0, -Math.Sin(horizontal),
                                                     0, 1, 0,
                                                     Math.Sin(horizontal), 0, Math.Cos(horizontal)});

            double vertical = degreeToRadian(trackBar2.Value);
            Matrix3 vertTransform = new Matrix3(new double[] {1, 0, 0,
                                                   0, Math.Cos(vertical), Math.Sin(vertical),
                                                   0, -Math.Sin(vertical), Math.Cos(vertical)});

            Matrix3 transform = horizTansform.multiply(vertTransform);

            g.TranslateTransform(renderArea.Width / 2, renderArea.Height / 2);

            foreach (Triangle t in tris)
            {
                GraphicsPath path = new GraphicsPath();

                Vertex v1 = transform.transform(t.V1);
                Vertex v2 = transform.transform(t.V2);
                Vertex v3 = transform.transform(t.V3);

                path.AddLine((float)v1.X, (float)v1.Y, (float)v2.X, (float)v2.Y);
                path.AddLine((float)v2.X, (float)v2.Y, (float)v3.X, (float)v3.Y);
                path.AddLine((float)v3.X, (float)v3.Y, (float)v1.X, (float)v1.Y);

                g.DrawPath(new Pen(t.Color, 2), path);
                path.Dispose();
            }
        }
    }
}
