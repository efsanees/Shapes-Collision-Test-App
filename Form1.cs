using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EfsaneProje.Form1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using GrapeCity.DataVisualization;
namespace EfsaneProje
{
    public partial class Form1 : Form
    {
        Graphics graphics;

        public class GeometrikSekil
        {

        }

        List<GeometrikSekil> sekiller = new List<GeometrikSekil>();

        public class Point : GeometrikSekil
        {
            int x, y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public int X => x;
            public int Y => y;
        }

        public class Point3d : Point
        {
            int z;

            public Point3d(int x, int y, int z) : base(x, y)
            {
                this.z = z;
            }
            public int Z => z;
        }

        public class Dortgen : GeometrikSekil
        {
            Point m;
            int Genislik;
            int Yukseklik;
            public Dortgen(Point p, int genislik, int yukseklik)
            {
                m = p;
                Genislik = genislik;
                Yukseklik = yukseklik;
            }
            public Point p => m;
            public int genislik => Genislik;
            public int yukseklik => Yukseklik;
        }

        public class Cember : GeometrikSekil
        {
            Point m;
            int r;

            public Cember(Point m, int r)
            {
                this.m = m;
                this.r = r;
            }
            public Point M => m;
            public int R => r;
        }

        public class Silindir : GeometrikSekil
        {
            Point3d m; int r; int h;
            public Silindir(Point3d m, int r, int h)
            {
                this.m = m;
                this.r = r;
                this.h = h;
            }
            public Point3d M => m;
            public int R => r;
            public int H => h;
        }
        public class Kure : GeometrikSekil
        {
            Point3d m; int r;
            public Kure(Point3d m, int r)
            {
                this.m = m;
                this.r = r;
            }
            public Point3d M => m;
            public int R => r;
        }
        public class DikdortgenPrizma : GeometrikSekil
        {
            Point3d m;
            int yukseklik;
            int genislik;
            int derinlik;
            public DikdortgenPrizma(Point3d m, int yukseklik, int genislik, int derinlik)
            {
                this.m = m;
                this.yukseklik = yukseklik;
                this.genislik = genislik;
                this.derinlik = derinlik;
            }
            public Point3d M => m;
            public int Yukseklik => yukseklik;
            public int Genislik => genislik;
            public int Derinlik => derinlik;
        }
        public class Yuzey : GeometrikSekil
        {
            Point p;
            int yukseklik;
            int genislik;
            public Yuzey(Point p, int yukseklik, int genislik)
            {
                this.p = p;
                this.yukseklik = yukseklik;
                this.genislik = genislik;
            }
            public int Yukseklik => yukseklik;
            public Point point => p;
            public int Genislik => genislik;
        }

        public class CarpismaDenetimi
        {
            public bool NoktaDortgen(Point p, Dortgen d)
            {
                if (p.X <= d.p.X + d.genislik / 2 && p.Y <= d.p.Y + d.yukseklik / 2) return true;
                else return false;
            }

            public bool NoktaCember(Point p, Cember c)
            {
                if (Math.Pow(p.X - c.M.X, 2) + Math.Pow(p.Y - c.M.Y, 2) <= Math.Pow(c.R, 2)) return true;
                else return false;
            }

            public bool DortgenDortgen(Dortgen d1, Dortgen d2)
            {
                int x1 = d1.p.X + d1.genislik / 2;
                int y1 = d1.p.Y + d1.yukseklik / 2;
                int x2 = d2.p.X - d2.genislik / 2;
                int y2 = d2.p.Y - d2.yukseklik / 2;
                if (Math.Abs(x1 - x2) <= (d1.genislik / 2 + d2.genislik / 2) && Math.Abs(y1 - y2) <= (d1.yukseklik / 2 + d2.yukseklik / 2)) return true;
                else return false;
            }

            public bool DortgenCember(Dortgen d, Cember c)
            {
                if (Math.Pow(c.M.X - d.p.X, 2) + Math.Pow(c.M.Y - d.p.Y, 2) <= c.R + d.genislik / 2 && Math.Pow(c.M.X - d.p.X, 2) + Math.Pow(c.M.Y - d.p.Y, 2) <= c.R + d.yukseklik / 2)
                    return true;
                else return false;
            }

            public bool CemberCember(Cember c1, Cember c2)
            {
                float d = (float)Math.Sqrt(Math.Pow(c1.M.X - c2.M.X, 2) + Math.Pow(c1.M.Y - c2.M.Y, 2));
                if (d <= c1.R + c2.R) return true;
                else return false;
            }

            public bool NoktaKure(Point3d p, Kure k)
            {
                if (Math.Pow(p.X - k.M.X, 2) <= Math.Pow(k.R, 2) && Math.Pow(p.Y - k.M.Y, 2) <= Math.Pow(k.R, 2) && Math.Pow(p.Z - k.M.Z, 2) <= Math.Pow(k.R, 2)) return true;
                else return false;
            }

            public bool NoktaDPrizma(Point3d p, DikdortgenPrizma dPrizma)
            {
                int solSinirX = dPrizma.M.X - dPrizma.Genislik / 2;
                int sagSinirX = dPrizma.M.X + dPrizma.Genislik / 2;
                int altSinirY = dPrizma.M.Y - dPrizma.Yukseklik / 2;
                int ustSinirY = dPrizma.M.Y + dPrizma.Yukseklik / 2;
                int onSinirZ = dPrizma.M.Z - dPrizma.Derinlik / 2;
                int arkaSinirZ = dPrizma.M.Z + dPrizma.Derinlik / 2;

                if ((p.X > solSinirX && p.X < sagSinirX) &&
                    (p.Y > altSinirY && p.Y < ustSinirY) &&
                    (p.Z > onSinirZ && p.Z < arkaSinirZ))
                {
                    return true;
                }

                return false;
            }

            public bool NoktaSilindir(Point3d p, Silindir s)
            {
                if (Math.Pow(p.X - s.M.X, 2) + Math.Pow(p.Y - s.M.Y, 2) <= Math.Pow(s.R, 2) && s.M.Z - s.H / 2 <= p.Z && p.Z <= s.M.Z + s.H / 2) return true;
                else return false;
            }

            public bool SilindirSilindir(Silindir s1, Silindir s2)
            {
                if (Math.Abs(s1.M.X - s2.M.X) <= s1.R + s2.R && Math.Abs(s1.M.Z - s2.M.Z) <= s1.R + s2.R && Math.Abs(s1.M.Y - s2.M.Y) <= (s1.H + s2.H) / 2) return true;
                else return false;
            }

            public bool KureKure(Kure k1, Kure k2)
            {
                if (Math.Pow(k1.M.X - k2.M.X, 2) <= Math.Pow(k1.R + k2.R, 2) && Math.Pow(k1.M.Y - k2.M.Y, 2) <= Math.Pow(k1.R + k2.R, 2)
                    && Math.Pow(k1.M.Z - k2.M.Z, 2) <= Math.Pow(k1.R + k2.R, 2)) return true;
                else return false;
            }

            public bool KureSilindir(Kure kure, Silindir silindir)
            {
                if (Math.Abs(kure.M.X - silindir.M.X) <= kure.R + silindir.R && Math.Abs(kure.M.Z - silindir.M.Z) <= kure.R + silindir.R
                    && Math.Abs(kure.M.Y - silindir.M.Y) <= kure.R + silindir.H / 2) return true;
                else return false;
            }

            public bool YuzeyKure(Kure kure, Yuzey yuzey)
            {
                double distanceX = Math.Abs(kure.M.X - yuzey.point.X);
                double distanceY = Math.Abs(kure.M.Y - yuzey.point.Y);

                if (distanceX <= yuzey.Genislik / 2 && distanceY <= yuzey.Yukseklik / 2)
                {
                    if (distanceX <= kure.R || distanceY <= kure.R)
                    {
                        return true;
                    }
                    else
                    {
                        double distanceToCorner = Math.Sqrt(Math.Pow(distanceX - yuzey.Genislik / 2, 2) + Math.Pow(distanceY - yuzey.Yukseklik / 2, 2));
                        if (distanceToCorner <= kure.R)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            public bool YuzeyDikdortgenPrizma(Yuzey yuzey, DikdortgenPrizma dPrizma)
            {
                int prizmaSol = dPrizma.M.X - dPrizma.Genislik / 2;
                int prizmaSag = dPrizma.M.X + dPrizma.Genislik / 2;
                int prizmaAlt = dPrizma.M.Y - dPrizma.Yukseklik / 2;
                int prizmaUst = dPrizma.M.Y + dPrizma.Yukseklik / 2;
                int prizmaOn = dPrizma.Derinlik / 2;
                int prizmaArka = -dPrizma.Derinlik / 2;

                int yuzeySol = yuzey.point.X - yuzey.Genislik / 2;
                int yuzeySag = yuzey.point.X + yuzey.Genislik / 2;
                int yuzeyAlt = yuzey.point.Y - yuzey.Yukseklik / 2;
                int yuzeyUst = yuzey.point.Y + yuzey.Yukseklik / 2;

                if (prizmaSag >= yuzeySol && prizmaSol <= yuzeySag && prizmaUst >= yuzeyAlt && prizmaAlt <= yuzeyUst) return true;
                else return false;
            }

            public bool YuzeySilindir(Yuzey yuzey, Silindir silindir)
            {
                int yuzeySol = yuzey.point.X - yuzey.Genislik / 2;
                int yuzeySag = yuzey.point.X + yuzey.Genislik / 2;
                int yuzeyAlt = yuzey.point.Y - yuzey.Yukseklik / 2;
                int yuzeyUst = yuzey.point.Y + yuzey.Yukseklik / 2;

                int silindirSag = silindir.M.X + silindir.R;
                int silindirSol = silindir.M.X - silindir.R;
                int silindirUst = silindir.M.Y + silindir.H / 2;
                int silindirAlt = silindir.M.Y - silindir.H / 2;

                if (silindirSag >= yuzeySol && silindirSol <= yuzeySag && silindirUst >= yuzeyAlt && silindirAlt <= yuzeyUst) { return true; }
                else return false;
            }

            public bool KureDikdortgenPrizma(Kure kure, DikdortgenPrizma dPrizma)
            {
                if (Math.Abs(kure.M.X - dPrizma.M.X) <= kure.R / 2 + dPrizma.Genislik / 2 && Math.Abs(kure.M.Y - dPrizma.M.Y) <= kure.R / 2 + dPrizma.Yukseklik / 2
                    && Math.Abs(kure.M.Z - dPrizma.M.Z) <= kure.R / 2 + dPrizma.Derinlik / 2) return true;
                else return false;
            }

            public bool DikdortgenPrizmaDikdortgenPrizma(DikdortgenPrizma dikdortgenPrizma1, DikdortgenPrizma dikdortgenPrizma2)
            {
                if (Math.Pow(dikdortgenPrizma1.M.X - dikdortgenPrizma2.M.X, 2) <= Math.Pow(dikdortgenPrizma1.Genislik / 2 + dikdortgenPrizma2.Genislik / 2, 2)
                    && Math.Pow(dikdortgenPrizma1.M.Y - dikdortgenPrizma2.M.Y, 2) <= Math.Pow(dikdortgenPrizma1.Yukseklik / 2 + dikdortgenPrizma2.Yukseklik / 2, 2)
                    && Math.Pow(dikdortgenPrizma1.M.Z - dikdortgenPrizma2.M.Z, 2) <= Math.Pow(dikdortgenPrizma1.Derinlik / 2 + dikdortgenPrizma2.Derinlik / 2, 2)) { return true; }
                else return false;
            }
        }

        public Form1()
        {
            InitializeComponent();
            sekiller = new List<GeometrikSekil>();
            this.SuspendLayout();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            graphics = this.CreateGraphics();

        }

        CarpismaDenetimi carpismaVarMi = new CarpismaDenetimi();
        Point point1 = new Point(350, 350);
        Dortgen dortgen1 = new Dortgen(new Point(300, 300), 100, 315);
        Point point2 = new Point(400, 250);
        Cember cember1 = new Cember(new Point(300, 350), 40);
        Dortgen dortgen2 = new Dortgen(new Point(300, 400), 100, 50);
        Dortgen dortgen3 = new Dortgen(new Point(350, 450), 100, 50);
        Dortgen dortgen4 = new Dortgen(new Point(400, 300), 140, 50);
        Cember cember2 = new Cember(new Point(300, 219), 40);
        Cember cember3 = new Cember(new Point(250, 255), 45);
        Cember cember4 = new Cember(new Point(400, 344), 42);
        Point3d point3 = new Point3d(216, 216, 316);
        Kure kure1 = new Kure(new Point3d(226, 276, 226), 28);
        Point3d point4 = new Point3d(219, 226, 400);
        DikdortgenPrizma dPrizma1 = new DikdortgenPrizma(new Point3d(224, 255, 229), 100, 200, 130);
        Point3d point5 = new Point3d(218, 218, 318);
        Silindir silindir1 = new Silindir(new Point3d(254, 218, 254), 100, 20);
        Silindir silindir2 = new Silindir(new Point3d(215, 216, 227), 100, 20);
        Silindir silindir3 = new Silindir(new Point3d(210, 215, 212), 100, 20);
        Kure kure2 = new Kure(new Point3d(326, 300, 320), 20);
        Kure kure3 = new Kure(new Point3d(226, 229, 212), 25);
        Kure kure4 = new Kure(new Point3d(250, 250, 213), 20);
        Silindir silindir4 = new Silindir(new Point3d(210, 215, 212), 100, 20);
        Yuzey yuzey1 = new Yuzey(new Point(305, 305), 110, 60);
        Kure kure5 = new Kure(new Point3d(316, 318, 310), 30);
        Yuzey yuzey2 = new Yuzey(new Point(316, 314), 121, 125);
        DikdortgenPrizma dPrizma2 = new DikdortgenPrizma(new Point3d(228, 128, 228), 115, 110, 120);
        Yuzey yuzey3 = new Yuzey(new Point(215, 215), 125, 90);
        Silindir silindir5 = new Silindir(new Point3d(424, 324, 224), 112, 38);
        Kure kure6 = new Kure(new Point3d(216, 218, 210), 50);
        DikdortgenPrizma dPrizma3 = new DikdortgenPrizma(new Point3d(318, 315, 317), 125, 100, 130);
        DikdortgenPrizma dPrizma4 = new DikdortgenPrizma(new Point3d(314, 310, 312), 112, 130, 105);
        DikdortgenPrizma dPrizma5 = new DikdortgenPrizma(new Point3d(310, 322, 226), 119, 100, 106);

        public void sekilEkle()
        {
            sekiller.Add(dortgen1);
            sekiller.Add(dortgen2);
            sekiller.Add(dortgen3);
            sekiller.Add(dortgen4);
            sekiller.Add(cember1);
            sekiller.Add(cember2);
            sekiller.Add(cember3);
            sekiller.Add(cember4);
            sekiller.Add(kure1);
            sekiller.Add(kure2);
            sekiller.Add(kure3);
            sekiller.Add(kure4);
            sekiller.Add(kure5);
            sekiller.Add(kure6);
            sekiller.Add(dPrizma1);
            sekiller.Add(dPrizma2);
            sekiller.Add(dPrizma3);
            sekiller.Add(dPrizma4);
            sekiller.Add(dPrizma5);
            sekiller.Add(silindir1);
            sekiller.Add(silindir2);
            sekiller.Add(silindir3);
            sekiller.Add(silindir4);
            sekiller.Add(silindir5);
            sekiller.Add(yuzey1);
            sekiller.Add(yuzey2);
            sekiller.Add(yuzey3);
        }

        Pen pen = new Pen(Color.Black, 3);
        Pen penRed = new Pen(Color.Red, 3);
        Pen penBlue = new Pen(Color.Blue, 3);
        Brush brush = new SolidBrush(Color.Black);
        Brush brushBlue = new SolidBrush(Color.Blue);
        Brush brushRed = new SolidBrush(Color.Red);
        private void button1_Click(object sender, EventArgs e)
        {
            graphics.Clear(this.BackColor);

            if ((comboBox1.SelectedIndex == 0 && comboBox2.SelectedIndex == 3) || (comboBox1.SelectedIndex == 3 && comboBox2.SelectedIndex == 0))
            {
                if (carpismaVarMi.NoktaDortgen(point1, dortgen1))
                    label4.Text = "Çarpışma VAR";
                else label4.Text = "Çarpışma YOK";
                label4.Visible = true;
                graphics.DrawRectangle(penRed, 300, 200, 100, 315);
                graphics.FillEllipse(brushBlue, 300, 250, 8, 8);

            }

            if (comboBox1.SelectedIndex == 0 && comboBox2.SelectedIndex == 0)
            {
                if (carpismaVarMi.DortgenDortgen(dortgen2, dortgen3))
                {
                    label4.Text = "Çarpışma VAR";
                }
                else
                {
                    label4.Text = "Çarpışma YOK";
                }
                label4.Visible = true;
                graphics.DrawRectangle(pen, 300, 200, 100, 50);
                graphics.DrawRectangle(penRed, 250, 200, 100, 50);
            }

            if ((comboBox1.SelectedIndex == 3 && comboBox2.SelectedIndex == 6) || (comboBox1.SelectedIndex == 6 && comboBox2.SelectedIndex == 3))
            {
                if (carpismaVarMi.NoktaCember(point2, cember1))
                    label4.Text = "Çarpışma VAR";
                else label4.Text = "Çarpışma YOK";
                label4.Visible = true;
                graphics.FillEllipse(brushBlue, 400, 250, 8, 8);
                graphics.DrawEllipse(pen, 300, 350, 40, 40);
            }

            if ((comboBox1.SelectedIndex == 0 && comboBox2.SelectedIndex == 6) || (comboBox1.SelectedIndex == 6 && comboBox2.SelectedIndex == 0))
            {
                if (carpismaVarMi.DortgenCember(dortgen4, cember2))
                    label4.Text = "Çarpışma VAR";
                else label4.Text = "Çarpışma YOK";
                label4.Visible = true;
                graphics.DrawRectangle(pen, 400, 200, 100, 50);
                graphics.DrawEllipse(pen, 300, 219, 8, 8);

            }

            if (comboBox1.SelectedIndex == 6 && comboBox2.SelectedIndex == 6)
            {
                if (carpismaVarMi.CemberCember(cember3, cember4))
                    label4.Text = "Çarpışma VAR";
                else label4.Text = "Çarpışma YOK";
                label4.Visible = true;
                graphics.DrawEllipse(pen, 250, 255, 8, 8);
                graphics.DrawEllipse(pen, 400, 244, 22, 22);
            }

            if ((comboBox1.SelectedIndex == 3 && comboBox2.SelectedIndex == 1) || (comboBox1.SelectedIndex == 1 && comboBox2.SelectedIndex == 3))
            {
                if (carpismaVarMi.NoktaKure(point3, kure1))
                    label4.Text = "Çarpışma VAR";
                else label4.Text = "Çarpışma YOK";
                label4.Visible = true;
                graphics.FillEllipse(brushBlue, 216, 216, 8, 8);
                graphics.FillEllipse(brush, 226, 276, 28, 28);
            }

            if ((comboBox1.SelectedIndex == 3 && comboBox2.SelectedIndex == 4) || (comboBox1.SelectedIndex == 4 && comboBox2.SelectedIndex == 3))
            {
                if (carpismaVarMi.NoktaDPrizma(point4, dPrizma1))
                    label4.Text = "Çarpışma VAR";
                else label4.Text = "Çarpışma YOK";
                label4.Visible = true;
                graphics.FillEllipse(brushBlue, 219, 226, 8, 8);
                graphics.FillRectangle(brush, 224, 255, 100, 200);
            }

            if ((comboBox1.SelectedIndex == 3 && comboBox2.SelectedIndex == 5) || (comboBox1.SelectedIndex == 5 && comboBox2.SelectedIndex == 3))
            {
                if (carpismaVarMi.NoktaSilindir(point5, silindir1))
                    label4.Text = "Çarpışma VAR";
                else label4.Text = "Çarpışma YOK";
                label4.Visible = true;
                graphics.FillEllipse(brushBlue, 218, 218, 8, 8);
                graphics.DrawEllipse(pen, 234, 98, 40, 40);
                graphics.DrawEllipse(pen, 234, 198, 40, 40);



                graphics.DrawLine(pen, 234, 218, 234, 118);
                graphics.DrawLine(pen, 274, 218, 274, 118);


                graphics.DrawArc(pen, 234, 98, 40, 40, 0, 180);
                graphics.DrawLine(pen, 234, 118, 274, 118);
            }

            if (comboBox1.SelectedIndex == 5 && comboBox2.SelectedIndex == 5)
            {
                if (carpismaVarMi.SilindirSilindir(silindir2, silindir3))
                    label4.Text = "Çarpışma VAR";
                else label4.Text = "Çarpışma YOK";
                label4.Visible = true;

                graphics.DrawEllipse(pen, 195, 196, 40, 40);

                graphics.DrawEllipse(pen, 195, 96, 40, 40);

                graphics.DrawLine(pen, 195, 216, 195, 116);
                graphics.DrawLine(pen, 235, 216, 235, 116);

                graphics.DrawArc(pen, 195, 96, 40, 40, 0, 180);
                graphics.DrawLine(pen, 195, 116, 235, 116);
                graphics.DrawEllipse(pen, 195, 195, 40, 40);

                graphics.DrawEllipse(pen, 190, 195, 40, 40);
                graphics.DrawEllipse(pen, 190, 95, 40, 40);

                graphics.DrawLine(pen, 190, 215, 190, 115);
                graphics.DrawLine(pen, 230, 215, 230, 115);

                graphics.DrawArc(pen, 190, 95, 40, 40, 0, 180);
                graphics.DrawLine(pen, 190, 115, 230, 115);

            }

            if ((comboBox1.SelectedIndex == 1 && comboBox2.SelectedIndex == 5) || (comboBox1.SelectedIndex == 5 && comboBox2.SelectedIndex == 1))
            {
                if (carpismaVarMi.KureSilindir(kure4, silindir4))
                    label4.Text = "Çarpışma VAR";
                else label4.Text = "Çarpışma YOK";
                label4.Visible = true;

                graphics.DrawEllipse(pen, 195, 196, 40, 40);

                graphics.DrawEllipse(pen, 195, 96, 40, 40);

                graphics.DrawLine(pen, 195, 216, 195, 116);
                graphics.DrawLine(pen, 235, 216, 235, 116);

                graphics.DrawArc(pen, 195, 96, 40, 40, 0, 180);
                graphics.DrawLine(pen, 195, 116, 235, 116);
                graphics.FillEllipse(brushBlue, 250, 250, 20, 20);

            }

            if (comboBox1.SelectedIndex == 1 && comboBox2.SelectedIndex == 1)
            {
                if (carpismaVarMi.KureKure(kure2, kure3))
                    label4.Text = "Çarpışma VAR";
                else label4.Text = "Çarpışma YOK";
                label4.Visible = true;
                graphics.FillEllipse(brushBlue, 326, 300, 20, 20);
                graphics.FillEllipse(brushRed, 226, 229, 25, 25);
            }

            if ((comboBox1.SelectedIndex == 2 && comboBox2.SelectedIndex == 1) || (comboBox1.SelectedIndex == 1 && comboBox2.SelectedIndex == 2))
            {
                if (carpismaVarMi.YuzeyKure(kure5, yuzey1))
                    label4.Text = "Çarpışma VAR";
                else label4.Text = "Çarpışma YOK";
                label4.Visible = true;
                graphics.FillRectangle(brushRed, 305, 305, 100, 60);
                graphics.FillEllipse(brushBlue, 316, 318, 30, 30);
            }

            if ((comboBox1.SelectedIndex == 2 && comboBox2.SelectedIndex == 4) || (comboBox1.SelectedIndex == 4 && comboBox2.SelectedIndex == 2))
            {
                if (carpismaVarMi.YuzeyDikdortgenPrizma(yuzey2, dPrizma2))
                    label4.Text = "Çarpışma VAR";
                else label4.Text = "Çarpışma YOK";
                label4.Visible = true;
                graphics.FillRectangle(brush, 316, 314, 121, 125);
                graphics.FillRectangle(brushRed, 228, 128, 115, 110);

            }

            if ((comboBox1.SelectedIndex == 5 && comboBox2.SelectedIndex == 2) || (comboBox1.SelectedIndex == 2 && comboBox2.SelectedIndex == 5))
            {
                if (carpismaVarMi.YuzeySilindir(yuzey3, silindir5))
                    label4.Text = "Çarpışma VAR";
                else label4.Text = "Çarpışma YOK";
                label4.Visible = true;
                int centerX = 424;
                int centerY = 324;
                int centerZ = 224;
                int radius = 38;
                int height = 112;

                graphics.DrawEllipse(pen, centerX - radius, centerY - radius, 2 * radius, 2 * radius);

                graphics.DrawEllipse(pen, centerX - radius, centerY - radius - height, 2 * radius, 2 * radius);

                graphics.DrawLine(pen, centerX - radius, centerY, centerX - radius, centerY - height);
                graphics.DrawLine(pen, centerX + radius, centerY, centerX + radius, centerY - height);

                graphics.DrawArc(pen, centerX - radius, centerY - height - radius, 2 * radius, 2 * radius, 0, 180);
                graphics.DrawLine(pen, centerX - radius, centerY - height, centerX + radius, centerY - height);

                graphics.FillRectangle(brushRed, 215, 215, 125, 90);
            }

            if ((comboBox1.SelectedIndex == 4 && comboBox2.SelectedIndex == 1) || (comboBox1.SelectedIndex == 1 && comboBox2.SelectedIndex == 4))
            {
                if (carpismaVarMi.KureDikdortgenPrizma(kure6, dPrizma3))
                    label4.Text = "Çarpışma VAR";
                else label4.Text = "Çarpışma YOK";
                label4.Visible = true;
                graphics.FillEllipse(brushRed, 216, 218, 50, 50);
                graphics.FillRectangle(brush, 318, 315, 125, 100);
            }

            if (comboBox1.SelectedIndex == 4 && comboBox2.SelectedIndex == 4)
            {
                if (carpismaVarMi.DikdortgenPrizmaDikdortgenPrizma(dPrizma4, dPrizma5))
                    label4.Text = "Çarpışma VAR";
                else label4.Text = "Çarpışma YOK";
                label4.Visible = true;
                graphics.FillRectangle(brushRed, 314, 310, 112, 130);
                graphics.FillRectangle(brushBlue, 310, 322, 119, 100);
            }

        }
    }
}