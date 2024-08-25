namespace FlipnoteDotNet.PropertyEditor.Editors
{
    public interface IEditor
    {
        Control Parent { get; set; }
        object Target { get; set; }
        string Name { get; set; }
        int RequestedTextRows { get; }
        void OnPaint(Graphics g, Font font);
        void OnMouseDown(MouseButtons buttons, int x, int y);
        void OnMouseMove(MouseButtons buttons, int x, int y);
        void OnMouseUp(MouseButtons buttons, int x, int y);
        void OnMouseLeave();
        void OnKeyPress(KeyPressEventArgs e);
        void OnKeyDown(KeyEventArgs e);
        void OnKeyUp(KeyEventArgs e);

        object Value { get; set; }
        event EventHandler<ByUserValueChangedEventArgs> ByUserValueChanged;
        Action<IEditor> OnInvalidate { get; set; }
        void Invalidate();

        void OnFocus();
        void OnFocusLost();

        void SummonControl(Control control);
        void DisposeControl();
        Control SummonedControl { get; }
        event EventHandler ControlSummoned;
        event EventHandler ControlDismissed;

        public class SummonedControlStats
        {
            public readonly IEditor Editor;
            public readonly Rectangle InnerBounds;            

            public SummonedControlStats(IEditor editor, Rectangle innerBounds)
            {
                Editor = editor;
                InnerBounds = innerBounds;
            }
        }
    }
}
