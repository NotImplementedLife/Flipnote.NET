namespace FlipnoteDotNet.PropertyEditor.Editors
{
    public abstract class Editor<T> : IEditor
    {
        public string Name { get; set; }

        public abstract int RequestedTextRows { get; }

        protected T fValue;
        protected readonly bool InvalidateAfterValueChanged;

        protected Editor(bool invalidateAfterValueChanged=false)
        {
            InvalidateAfterValueChanged = invalidateAfterValueChanged;
        }

        public virtual object Value
        {
            get => fValue;
            set
            {
                fValue = (T)value;
                if (InvalidateAfterValueChanged) Invalidate();
            }
        }
        public Action<IEditor> OnInvalidate { get; set; }

        public event EventHandler ByUserValueChanged;

        public void Invalidate() => OnInvalidate?.Invoke(this);

        public virtual void OnMouseDown(MouseButtons buttons, int x, int y) { }
        public virtual void OnMouseLeave() { }        
        public virtual void OnMouseMove(MouseButtons buttons, int x, int y) { }        
        public virtual void OnMouseUp(MouseButtons buttons, int x, int y) { }
        public virtual void OnPaint(Graphics g, Font font) { }

        protected void TriggerUserValueChanged()
        {
            ByUserValueChanged?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnFocus() { }
        public virtual void OnFocusLost() { }

        public virtual void OnKeyPress(KeyPressEventArgs e) { }
        public virtual void OnKeyDown(KeyEventArgs e) { }
        public virtual void OnKeyUp(KeyEventArgs e) { }


        public virtual void SummonControl(Control control)
        {
            DisposeControl();
            if (control == null) return;
            SummonedControl = control;
            ControlSummoned?.Invoke(this, EventArgs.Empty);

        }
        public virtual void DisposeControl()
        {
            if (SummonedControl == null) return;
            ControlDismissed?.Invoke(this, EventArgs.Empty);
            SummonedControl.Dispose();
            SummonedControl = null;
        }
        public Control SummonedControl { get; private set; }
        public event EventHandler ControlSummoned;
        public event EventHandler ControlDismissed;

    }
}
