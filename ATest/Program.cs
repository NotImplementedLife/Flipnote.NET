using PPMLib.Rendering;
using PPMLib.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using static PPMLib.Utils.FlipnoteVisualSourceResizer;

namespace ATest
{
    internal class Program
    {

        private static long Benchmark(FlipnoteVisualSource source, bool dithering, RescaleMethod method, int w, int h)
        {            
            
            var s = Stopwatch.StartNew();
            var resized = new FlipnoteVisualSource(source, w, h, dithering, method);
            s.Stop();            
            return s.ElapsedMilliseconds;
        }

        private static void BenchmarkS(bool dithering, RescaleMethod method)
        {
            var ms = new List<long>();
            var msp128px = new List<double>();
            var source = Load(@"D:\Users\NotImpLife\Projects\PPMLib\ATest\cat.png");

            Console.WriteLine($"Rescale Dithering = {dithering}, Method = {method}");

            for (int w = 200; w < 400; w += 5) 
            {
                Debug.WriteLine($"{w}");
                for (int h = 200; h < 400; h += 5) 
                {
                    var t = Benchmark(source, dithering, method, w, h);
                    ms.Add(t);
                    msp128px.Add(128.0 * t / (w * h));
                }
            }

            Console.WriteLine($"Milliseconds Max = {ms.Max()}, Min={ms.Min()}, Avg={ms.Average()}");
            Console.WriteLine($"Ms/128px Max = {msp128px.Max()}, Min={msp128px.Min()}, Avg={msp128px.Average()}");
            Console.WriteLine();
        }

        private static void BenchmarkColor()
        {
            var rand = new Random();
            var arr = Enumerable.Range(0, 1000000).Select(_ => (byte)(rand.Next() % 3)).ToArray();

            var sw = Stopwatch.StartNew();
            var colors = FlipnoteVisualSourceResizer.ToColorVectors(arr);
            sw.Stop();
            Console.WriteLine($"ToColorVectors : {sw.ElapsedMilliseconds}");

        }


        static long MX = 0;
        static void Main(string[] args)
        {
            BenchmarkColor();
            BenchmarkColor();
            BenchmarkColor();
            BenchmarkColor();
            BenchmarkColor();

            BenchmarkS(false, RescaleMethod.NearestNeighbor);
            BenchmarkS(true, RescaleMethod.NearestNeighbor);
            BenchmarkS(false, RescaleMethod.Bilinear);
            BenchmarkS(true, RescaleMethod.Bilinear);
            Console.ReadLine();
            return;

            Plot();            
            var s = Load(@"D:\Users\NotImpLife\Projects\PPMLib\ATest\cat.png");
            int w = 300;
            int h = 270;

            Save(s, "res6.png");

            var m = RescaleMethod.Bilinear;

            var originalColors = ToColorVectors(s.Data);
            var newColors = Resize(originalColors, s.Width, s.Height, w, h, m);
            MX = newColors.Max(_ => 1L * _.X * _.X + 1L * _.Y * _.Y);
            Debug.WriteLine(MX);

            Save(newColors, w, h, "res.png");

            File.WriteAllText("data.txt",
                string.Join("\n", newColors                
                .Select(c => $"{c.X} {c.Y}| {Math.Atan2(c.Y, c.X) * 180 / Math.PI} | {PFromAngle(c)}")
                .Distinct()));

            s = new FlipnoteVisualSource(s, w, h, false, m);
            Save(s, "res5.png");

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        static double a0 = 7 * Math.PI / 6;
        static double a1 = 11 * Math.PI / 6;
        static double a2 = Math.PI / 2;

        public static double AngleDifference(double angle1, double angle2)
        {
            double diff = (angle2 - angle1 + Math.PI) % (2 * Math.PI) - Math.PI;
            return Math.Abs(diff < -Math.PI ? diff + 2 * Math.PI : diff);
        }

        static string PFromAngle(C012 c)
        {
            var a = Math.Atan2(c.Y, c.X);
            var arr = new double[] { a0, a1, a2 };
            var m = arr.Select((q, i) => (q, i))
                .OrderBy(_ => AngleDifference(_.q, a)).Select(_ => _.i).Take(2).ToArray();
            var c1 = m[0] == 0 ? Color.Black : m[0] == 1 ? Color.Red : Color.Blue;
            var c2 = m[1] == 0 ? Color.Black : m[1] == 1 ? Color.Red : Color.Blue;

            var f1 = AngleDifference(arr[m[0]], a);
            var f2 = AngleDifference(arr[m[1]], a);

            var r = (byte)((f2 * c1.R + f1 * c2.R) / (f1 + f2));
            var b = (byte)((f2 * c1.B + f1 * c2.B) / (f1 + f2));

            return $"{c1}:{f1} + {c2}:{f2} & r,b={r}, {b}";
        }

        static Color FromAngle(C012 c)
        {
            var a = Math.Atan2(c.Y, c.X);            
            var arr = new double[] { a0, a1, a2 };
            var m = arr.Select((q, i) => (q, i))
                .OrderBy(_ => AngleDifference(_.q, a)).Select(_ => _.i).Take(2).ToArray();
            var c1 = m[0] == 0 ? Color.Black : m[0] == 1 ? Color.Red : Color.Blue;
            var c2 = m[1] == 0 ? Color.Black : m[1] == 1 ? Color.Red : Color.Blue;

            var f1 = AngleDifference(arr[m[0]], a);
            var f2 = AngleDifference(arr[m[1]], a);

            var r = (byte)((f2 * c1.R + f1 * c2.R) / (f1 + f2));
            var b = (byte)((f2 * c1.B + f1 * c2.B) / (f1 + f2));
            var g = (byte)((1L * c.X * c.X + 1L * c.Y * c.Y) * 64 / MX);

            return Color.FromArgb(r, g, b);
        }

        static void Plot()
        {
            var bmp = new Bitmap(100, 100);

            Color[] Colors = { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Orange, Color.Cyan, Color.Crimson,              
            Color.White};

            //{ 0, 0, 0, 2, 1, 1, 0, 2}

            for(int y=-49;y<50;y++)
            {
                for(int x=-50;x<50;x++)
                {
                    var p = new C012(x, y);
                    var i = p.ResolveIndex();

                    //bmp.SetPixel(x + 50, -y + 50, Colors[i]);

                    if (i == 1) bmp.SetPixel(x + 50, -y + 50, Color.Red);
                    else if (i == 2) bmp.SetPixel(x + 50, -y + 50, Color.Blue);
                    else bmp.SetPixel(x + 50, -y + 50, Color.Black);
                }
            }
            bmp.Save("plot.png");
        }

        static FlipnoteVisualSource Load(string path)
        {
            var bmp = new Bitmap(path);
            var fvs = new FlipnoteVisualSource(bmp.Width, bmp.Height);

            for(int y=0;y<bmp.Height;y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var c = bmp.GetPixel(x, y);
                    if (c.G > 128)
                        fvs.Data[y * fvs.Width + x] = 0;
                    else if (c.R > 128)
                        fvs.Data[y * fvs.Width + x] = 1;                   
                    else if (c.B > 128) 
                        fvs.Data[y * fvs.Width + x] = 2;                    
                }
            }
            return fvs;
        }

        static void Save(FlipnoteVisualSource s, string path)
        {
            var bmp = new Bitmap(s.Width, s.Height);

            for(int y=0;y<s.Height;y++)
            {
                for (int x = 0; x < s.Width; x++)
                {
                    var c = s.Data[y * s.Width + x];
                    if (c == 1)                                            
                        bmp.SetPixel(x, y, Color.Red);                    
                    else if (c == 2)
                        bmp.SetPixel(x, y, Color.Blue);
                    else bmp.SetPixel(x, y, Color.White);
                }
            }
            bmp.Save(path);
        }

        static void Save(C012[] cls, int w, int h, string path)
        {
            var bmp = new Bitmap(w, h);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int i = y * w + x;                    
                    //bmp.SetPixel(x, y, Color.FromArgb(a, 0, b));
                    bmp.SetPixel(x, y, FromAngle(cls[i]));
                }
            }
            bmp.Save(path);
        }

        static int f(int x, int a, int b) => x <= a ? a : x >= b ? b : x;
    }
}
