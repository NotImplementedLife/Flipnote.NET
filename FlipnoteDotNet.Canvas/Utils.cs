namespace FlipnoteDotNet.Canvas
{
    internal static class Utils
    {
        public static float DistanceSquared(PointF p1, PointF p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }
    }
}
