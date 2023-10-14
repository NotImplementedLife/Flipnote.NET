using System.Collections.Generic;
using System.Linq;

namespace DitheringTest
{
    internal class PointsCollection
    {
        struct Rec
        {
            public float X { get; }
            public float Y { get; }
            public C012 C { get; }

            public Rec(float x, float y, C012 c)
            {
                X = x;
                Y = y;
                C = c;
            }
        }

        Dictionary<(int X, int Y), List<Rec>> Points = new Dictionary<(int X, int Y), List<Rec>>();

        List<Rec> GetList(int x, int y)
        {
            return Points.TryGetValue((x, y), out var list) ? list : (Points[(x, y)] = new List<Rec>());
        }

        public void Add(float x, float y, C012 pt)
        {
            GetList((int)x, (int)y).Add(new Rec(x, y, pt));
        }

        public C012 GetValue(int x, int y, float sx, float sy)
        {
            var pts = new List<Rec>();
            int dx = (int)sx / 2 + 1;
            int dy = (int)sy / 2 + 1;
            for (int i = -dx; i <= dx; i++) 
            {
                for (int j = -dy; j <= dy; j++) 
                {
                    pts.AddRange(GetList(x + i, y + j));
                }
            }
            var px = pts.OrderBy(_ => dist(x, y, _.X, _.Y)).Take(4)
               .Select(_ => (_.C, D: dist(x, y, _.X, _.Y)))               
               .ToList();
            if (px.Count == 0) return new C012(0, 0);

            var p = px[0].C * px[0].D;
            for (int i = 1; i < px.Count; i++)
                p += px[i].C * px[i].D;

            return p / px.Count;

        }

        public void Clear() => Points.Clear();


        private static float dist(float x1, float y1, float x2, float y2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            return dx * dx + dy * dy;
        }

    }
}
