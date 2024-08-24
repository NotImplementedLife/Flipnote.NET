namespace FlipnoteDotNet.Utils.GUI
{
    public readonly struct DisplayLength
    {
        public readonly float Value;
        public readonly char Unit;
        public DisplayLength(float value, char unit)
        {
            Value = value;
            Unit = unit;
        }
        public readonly bool IsProportional => Unit == '*';
        public readonly bool IsPixels => Unit == 'p';
        public static DisplayLength Pixels(float value) => new DisplayLength(value, 'p');
        public static DisplayLength Proportional(float value) => new DisplayLength(value, '*');

        public static DisplayLength operator +(DisplayLength d, float value) => new DisplayLength(d.Value + value, d.Unit);
        public static DisplayLength operator -(DisplayLength d, float value) => new DisplayLength(d.Value - value, d.Unit);
    }
}
