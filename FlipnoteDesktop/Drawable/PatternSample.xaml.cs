using FlipnoteDesktop.Environment.Canvas;
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
    /// Interaction logic for PatternSample.xaml
    /// </summary>
    public partial class PatternSample : UserControl
    {
        public PatternSample()
        {
            InitializeComponent();
            Image.Source = new WriteableBitmap(12, 12, 96, 96, PixelFormats.Indexed2,
                new BitmapPalette(new List<Color> { Colors.White, Colors.Black }));           
        }

        public static DependencyProperty PatternNameProperty = DependencyProperty.Register("PatternName", typeof(string), typeof(PatternSample),
            new PropertyMetadata("Default", new PropertyChangedCallback(PatternNamePropertyChanged)));

        public string PatternName
        {
            get => (string)GetValue(PatternNameProperty);
            set => SetValue(PatternNameProperty, value);
        }

        public Pattern Pattern
        {
            get => typeof(Patterns).GetField(PatternName).GetValue(null) as Pattern;
        }

        private static void PatternNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var o = d as PatternSample;
            Pattern p = Patterns.Mono;
            try
            {
                if (e.NewValue is string)
                {
                    p = typeof(Patterns).GetField((string)e.NewValue).GetValue(null) as Pattern;                    
                }
            }
            finally
            {
                for (int x = 0; x < 12; x++)
                    for (int y = 0; y < 12; y++)
                        if (p.GetPixelAt(x, y))
                        {                            
                            o.SetImagePixel(x, y, 1);
                        }
            }
            (o.Image.Source as WriteableBitmap).WritePixels(new Int32Rect(0, 0, 12, 12), o.pixels, 3, 0);
        }

        byte[] pixels = new byte[12 * 3];
        private void SetImagePixel(int x, int y, int val)
        {
            int b = 12 * y + x;
            int p = 3 - b % 4;
            b /= 4;
            pixels[b] &= (byte)(~(0b11 << (2 * p)));
            pixels[b] |= (byte)(val << (2 * p));
        }
    }
}
