namespace FlipnoteDotNet.Canvas.Selections
{
    internal static class SelectionRenderer
    {
        public static void DrawPoint(Graphics g,  PointF point)
        {
            g.FillRectangle(Brushes.White, point.X - 2, point.Y - 2, 4, 4);
            g.DrawRectangle(Pens.DodgerBlue, point.X - 2, point.Y - 2, 4, 4);            
        }
    }
}
