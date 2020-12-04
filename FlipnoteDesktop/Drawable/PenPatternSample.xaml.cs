using FlipnoteDesktop.Environment.Canvas;
using FlipnoteDesktop.Environment.Canvas.DrawingTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlipnoteDesktop.Drawable
{
    /// <summary>
    /// Interaction logic for PenPatternSample.xaml
    /// </summary>
    public partial class PenPatternSample : UserControl
    {
        public PenPatternSample()
        {
            InitializeComponent();
            Image.Source = new WriteableBitmap(12, 12, 96, 96, PixelFormats.Indexed2,
                new BitmapPalette(new List<Color> { Colors.White, Colors.Black }));
        }

        public bool IsPen;

        public static DependencyProperty PatternNameProperty = DependencyProperty.Register("PatternName", typeof(string), typeof(PenPatternSample),
            new PropertyMetadata("Default", new PropertyChangedCallback(PatternNamePropertyChanged)));

        public string PatternName
        {
            get => (string)GetValue(PatternNameProperty);
            set => SetValue(PatternNameProperty, value);
        }

        public Pattern Pattern
        {
            get => typeof(PenPatterns).GetField(PatternName).GetValue(null) as Pattern;
        }

        private static void PatternNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var o = d as PenPatternSample;
            Pattern p = PenPatterns.Mono;
            try
            {
                if (e.NewValue is string)
                {
                    p = typeof(PenPatterns).GetField((string)e.NewValue).GetValue(null) as Pattern;
                }
            }
            finally
            {
                int x0 = 2, y0 = 0;
                for (int t = 3; t < 10; t++)
                {
                    double a = (t * 0.25 - 1.3);
                    int y = (int)(11 * a * a * a) + 6;
                    var pts = LineTool.GetLinePixels(x0, y0, t, y);
                    if (o.Pattern.ContinuousDraw)
                    {
                        int lstX = x0, lstY = y0;
                        for (int i = 1; i < pts.Count; i++)
                        {
                            var dx = pts[i].X - x0;
                            var dy = pts[i].Y - y0;
                            if (dx == 0 || dy == 0)
                            {
                                o.PutPoint(pts[i].X, pts[i].Y);
                            }
                            else
                            {
                                o.PutPoint(x0 + dx, y0);
                                o.PutPoint(pts[i].X, pts[i].Y);
                            }
                            x0 = pts[i].X;
                            y0 = pts[i].Y;
                        }
                    }
                    else
                    {
                        for (int i = 1; i < pts.Count; i++)
                            o.PutPoint(pts[i].X, pts[i].Y);
                    }
                    x0 = t;
                    y0 = y;
                }
            }
            (o.Image.Source as WriteableBitmap).WritePixels(new Int32Rect(0, 0, 12, 12), o.pixels, 3, 0);
        }

        void PutPoint(int x, int y)
        {
            int szX = Pattern.Cols;
            int szY = Pattern.Rows;
            for (int _x = 0; _x < szX; _x++)
                for (int _y = 0; _y < szY; _y++)
                {
                    if (Pattern.GetPixelAt(_x, _y))
                    {
                        SetImagePixel(Math.Max(0, x - szX / 2) + _x, Math.Max(0, y - szY / 2) + _y, 1);
                    }
                }            
        }

        byte[] pixels = new byte[12 * 3];
        private void SetImagePixel(int x, int y, int val)
        {
            if (x < 0 || x > 11 || y < 0 || y > 11) return;
            int b = 12 * y + x;
            int p = 3 - b % 4;
            b /= 4;
            pixels[b] &= (byte)(~(0b11 << (2 * p)));
            pixels[b] |= (byte)(val << (2 * p));
        }
    }
}
