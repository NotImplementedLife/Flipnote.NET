using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Canvas
{
    public readonly struct Angle
    {
        public readonly float Value = 0;
        public readonly float CosValue = 1;
        public readonly float SinValue = 0;

        public Angle(float value)
        {
            Value = value;
            CosValue = (float)Math.Cos(value * Math.PI / 180);
            SinValue = (float)Math.Sin(value * Math.PI / 180);
        }

        public Angle(int value)
        {
            Value = value;
            CosValue = (float)Math.Cos(value * Math.PI / 180);
            SinValue = (float)Math.Sin(value * Math.PI / 180);
        }

        public static implicit operator Angle(float value) => new Angle(value);        
        public override string ToString() => Value.ToString() + "deg";
    }
}
