namespace PPMLib.Extensions
{
    internal static class IntExtensions
    {
        public static bool IsBetween(this ushort x, ushort a, ushort b) => a <= x && x <= b;        
        public static bool IsBetween(this int x, int a, int b) => a <= x && x <= b;        
    }
}
