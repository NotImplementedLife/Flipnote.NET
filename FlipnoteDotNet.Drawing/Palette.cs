namespace FlipnoteDotNet.Drawing
{
    public readonly struct Palette
    {
        public readonly Color[] Colors;
        public readonly int SpreadR, SpreadG, SpreadB;

        public Palette(Color[] colors = null)
        {
            Colors = colors ?? new Color[] { Color.Black, Color.White };
            ComputeStdDev(Colors, out SpreadR, out SpreadG, out SpreadB);
        }

        private static int Square(int x) => x * x;

        private static void ComputeStdDev(Color[] colors, out int dr, out int dg, out int db)
        {
            int n = colors.Length;
            int sr = 0, sg = 0, sb = 0;
            for (int i = 0; i < n; i++)
            {
                sr += colors[i].R;
                sg += colors[i].G;
                sb += colors[i].B;
            }
            int vr = 0, vg = 0, vb = 0;
            for (int i = 0; i < n; i++)
            {
                vr += Square(n * colors[i].R - sr);
                vg += Square(n * colors[i].G - sg);
                vb += Square(n * colors[i].B - sb);
            }

            int n3 = n * n * n;

            dr = (int)Math.Sqrt(vr / n3);
            dg = (int)Math.Sqrt(vg / n3);
            db = (int)Math.Sqrt(vb / n3);
        }

        private readonly int Distance(Color color, int r1, int g1, int b1)
        {
            int dr = color.R - r1, dg = color.G - g1, db = color.B - b1;
            return dr * dr + dg * dg + db * db;
        }

        public readonly Color ClosestColor(int r, int g, int b)
        {
            Color color = Colors[0];
            int d = Distance(color, r, g, b);

            for (int i = 1; i < Colors.Length; i++)
            {
                int d1 = Distance(Colors[i], r, g, b);
                if (d1 < d)
                {
                    color = Colors[i];
                    d = d1;
                }
            }
            return color;
        }

        public readonly Palette IndexedSubpalette(int[] indices)
        {
            var colors = new Color[indices.Length];
            for (int i = 0; i < indices.Length; i++) colors[i] = Colors[indices[i]];
            return new Palette(colors);
        }
    }
}