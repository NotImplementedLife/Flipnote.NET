using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DitheringTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var g = Graphics.FromImage(Bitmap);
            g.Clear(Color.White);

            Pixels = new int[Bitmap.Width * Bitmap.Height];

        }

        Bitmap Bitmap = new Bitmap(60, 60);
        Graphics g;
        int Scale = 1;
        int DColor = 0;

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {            
            var bmp = IsDithering ? DitheredBmp : Bitmap;
            Canvas.Width = Scale * bmp.Width;
            Canvas.Height = Scale * bmp.Height;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.DrawImage(bmp, 0, 0, bmp.Width * Scale, bmp.Height * Scale);            
        }

        private void ZoomButton_Click(object sender, EventArgs e)
        {
            Scale = 3 - Scale;
            Canvas.Width = Scale * Bitmap.Width;
            Canvas.Height = Scale * Bitmap.Height;
            Canvas.Invalidate();
        }

        private void Color0Button_Click(object sender, EventArgs e) => DColor = 0;
        private void Color1Button_Click(object sender, EventArgs e) => DColor = 1;
        private void Color2Button_Click(object sender, EventArgs e) => DColor = 2;


        bool msdown = false;
        Point point;
        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            msdown = true;
            point = e.Location;
            PutPx(point.X / Scale, point.Y / Scale);
            Canvas.Invalidate();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!msdown) return;
            line(point.X / Scale, point.Y / Scale, e.X / Scale, e.Y / Scale);
            Canvas.Invalidate();
            point = e.Location;
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            msdown = false;
        }

        public void line(int x, int y, int x2, int y2)
        {
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                PutPx(x, y);
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }

        void PutPx(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Bitmap.Width || y >= Bitmap.Height) return;
            Bitmap.SetPixel(x, y, Colors[DColor]);
            Pixels[y * Bitmap.Width + x] = DColor;
        }

        Color[] Colors = { Color.White, Color.Black, Color.Red };

        int[] Pixels;        

        private void DitherButton_Click(object sender, EventArgs e)
        {
            var scX = ScaleX.Value / 10.0f;
            var scY = ScaleY.Value / 10.0f;

            Points.Clear();

            for (int y = 0; y < Bitmap.Height; y++)
            {
                for (int x = 0; x < Bitmap.Width; x++)
                {                    

                    //Points.Add(x * scX, y * scY, new C012(Pixels[y * Bitmap.Width + x]));
                    //Points.Add((x + 1) * scX, y * scY, new C012(Pixels[y * Bitmap.Width + x]));
                    //Points.Add(x * scX, (y + 1) * scY, new C012(Pixels[y * Bitmap.Width + x]));
                    Points.Add((x+0.5f) * scX, (y+0.5f) * scY, new C012(Pixels[y * Bitmap.Width + x]));
                }
            }            
            var newW = (int)(scX * Bitmap.Width) + 1;
            var newH = (int)(scY * Bitmap.Height) + 1;

            var px2 = new C012[newW * newH];            

            for (int y = 0; y < newH; y++) 
            {
                for (int x = 0; x < newW; x++)
                {
                    px2[y * newW + x] = Points.GetValue(x, y, ScaleX.Value, ScaleY.Value);
                    //Debug.WriteLine(px2[y * newW + x]);
                }
            }

            for (int y = 0; y < newH; y++) 
            {
                for (int x = 0; x < newW; x++) 
                {
                    var oldPx = px2[y * newW + x];
                    var newPx = px2[y * newW + x] = oldPx.GetClosestColor();
                    var err = oldPx - newPx;

                    int[] dx = { 1, -1, 0, 1 };
                    int[] dy = { 0, 1, 1, 1 };
                    int[] dd = { 7, 3, 5, 1 };

                    for (int i = 0; i < 4; i++)
                    {
                        int x2 = x + dx[i];
                        int y2 = y + dy[i];
                        if (x2 < 0 || y2 < 0 || x2 >= newW || y2 >= newH) continue;
                        px2[y2 * newW + x2] += err * dd[i] / 16;
                    }                    
                }
            }

            DitheredBmp = new Bitmap(newW, newH);
            for(int y=0;y<newH;y++)
            {
                for (int x = 0; x < newW; x++) 
                {
                    var ix = px2[y * newW + x].ToIndex();
                    DitheredBmp.SetPixel(x, y, Colors[ix]);
                }
            }

            IsDithering = true;
            Canvas.Invalidate();
        }

        bool IsDithering = false;
        Bitmap DitheredBmp = new Bitmap(10, 10);

        PointsCollection Points = new PointsCollection();      

        private void Scale_Scroll(object sender, EventArgs e)
        {
            DitherButton_Click(null, null);
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            IsDithering = false;
            ScaleX.Value = ScaleY.Value = 10;
            Canvas.Invalidate();
        }
    }
}
