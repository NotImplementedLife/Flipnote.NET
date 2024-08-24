namespace FlipnoteDotNet.PropertyEditor.Editors
{
    internal class Placeholder : Editor<int>
    {        
        public override int RequestedTextRows => 1;

        public override object Value { get; set; }

        public override void OnPaint(Graphics g, Font font)
        {
            g.DrawString(Value?.ToString() ?? "null", font, Brushes.Black, g.ClipBounds);
        }        
    }
}
