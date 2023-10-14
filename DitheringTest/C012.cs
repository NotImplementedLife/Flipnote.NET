using System.Linq;

namespace DitheringTest
{
    public class C012
    {
        public C012(int a)
        {
            if (a == 1) Values[0] = 1;
            else if (a == 2) Values[1] = 1;            
        }

        public C012(float x, float y)
        {
            Values[0] = x;
            Values[1] = y;            
        }

        public float[] Values { get; } = new float[2] { 0, 0 };

        public C012 GetClosestColor()
        {
            if (Values[0] < 0.3 && Values[1] < 0.3) return new C012(0, 0);
            var mx = Values.Max();
            if (mx == Values[0]) return new C012(1);
            else return new C012(2);
        }

        public int ToIndex()
        {
            if (Values[0] < 0.5 && Values[1] < 0.5) return 0;
            var mx = Values.Max();
            if (mx == Values[0]) return 1;
            else return 2;
        }

        public float this[int index] => Values[index];
        public static C012 operator +(C012 a, C012 b) => new C012(a[0] + b[0], a[1] + b[1]);
        public static C012 operator -(C012 a, C012 b) => new C012(a[0] - b[0], a[1] - b[1]);
        public static C012 operator *(C012 a, float x) => new C012(a[0] * x, a[1] * x);
        public static C012 operator /(C012 a, float x) => new C012(a[0] / x, a[1] / x);

        public C012 Normalize()
        {
            var s = Values[0] + Values[1];
            if (s == 0) return new C012(0, 0);
            return new C012(Values[0] / s, Values[1] / s);
        }

        public override string ToString() => $"{Values[0]} {Values[1]}";
    }
}
