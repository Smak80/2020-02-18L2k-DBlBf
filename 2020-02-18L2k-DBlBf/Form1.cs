using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2020_02_18L2k_DBlBf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Image img = null;
        private BufferedGraphicsContext bgc = BufferedGraphicsManager.Current;
        private BufferedGraphics bg;
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            var b = new SolidBrush(Color.Purple);
            if (img == null)
            {
                img = new Bitmap(
                    panel1.Width, panel1.Height,
                    panel1.CreateGraphics()
                    );
                var gimg = Graphics.FromImage(img);
                bg = bgc.Allocate(panel1.CreateGraphics(), 
                    new Rectangle(0, 0, panel1.Width, panel1.Height)
                    );
                gimg.Clear(Color.White);
                gimg.FillPie(b, 
                    new Rectangle(30,30,300,300), 
                    0, 
                    -105 
                );
            }
            e.Graphics.DrawImage(img, 0, 0);
        }

        private bool paint = false;

        private bool paintRect = false;
        
        private Point startPoint;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                paint = true;
            } else if (e.Button == MouseButtons.Right)
            {
                paintRect = true;
            }
            startPoint = new Point(e.X, e.Y);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                paint = false;
                panel1.CreateGraphics().DrawImage(img, 0, 0);
            } else if (e.Button == MouseButtons.Right)
            {
                paintRect = false;
                var b = new SolidBrush(Color.GreenYellow);
                var gimg = Graphics.FromImage(img);
                var currentPoint = new Point(e.X, e.Y);
                var sz = new Size(currentPoint.X-startPoint.X, 
                    currentPoint.Y-startPoint.Y);
                gimg.FillRectangle(b, new Rectangle(startPoint, sz));
                panel1.CreateGraphics().DrawImage(img, 0, 0);
                img.Save("kartinka.jpg", ImageFormat.Jpeg);
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (img == null) return;
            var p = new Pen(Color.Blue);
            var g = panel1.CreateGraphics();
            var bgg = bg.Graphics;
            var currentPoint = new Point(e.X, e.Y);

            if (paint)
            {
                g.DrawLine(p, startPoint, currentPoint);
                startPoint = currentPoint;
            } else if (paintRect)
            {
                var sz = new Size(currentPoint.X-startPoint.X, 
                    currentPoint.Y-startPoint.Y);
                bgg.DrawImage(img, 0, 0);
                bgg.DrawRectangle(p, new Rectangle(startPoint, sz));    
                bg.Render();
            }
        }
    }
}
