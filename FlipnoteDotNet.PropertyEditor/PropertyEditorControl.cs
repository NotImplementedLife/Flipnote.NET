using FlipnoteDotNet.Core.GUI;
using FlipnoteDotNet.Core.Utils;
using FlipnoteDotNet.PropertyEditor.Editors;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using static FlipnoteDotNet.PropertyEditor.PropertiesCollection;

namespace FlipnoteDotNet.PropertyEditor
{
    public class PropertyEditorControl : Control
    {
        private PropertiesCollection PropertiesCollection;
        private int[] EditorsY;
        private int[] EditorsHeight;
        private int PropWidth;
        private int EditorWidth;
        private int ScrollY;

        private IEditor[] Editors;
        readonly Dictionary<IEditor, (int Y, int Height)> EditorsLayout = new Dictionary<IEditor, (int Y, int Height)>();
        readonly Dictionary<IEditor, PropertyData> EditorProperty = new Dictionary<IEditor, PropertyData>();
        readonly Dictionary<string, (IEditor, PropertyData)> ByName = new Dictionary<string, (IEditor, PropertyData)>();

        private readonly VirtualScroller VirtualScroller = new VirtualScroller(vertical: true);

        public PropertyEditorControl()
        {
            DoubleBuffered = true;            
            VirtualScroller.Attach(this);
            VirtualScroller.VScrollChanged += VirtualScroller_VScrollChanged;            
        }

        private void VirtualScroller_VScrollChanged(object sender, int e)
        {
            ScrollY = e;
            UpdateSummonedControls();            
            Invalidate();
        }

        private object fObject;
        public object Object
        {
            get => fObject;
            set
            {
                if (fObject == value) return;
                AttachObject(value);
                RefreshEditors();                
                Invalidate();                
            }
        }        

        Random rng = new Random();

        protected override void OnPaint(PaintEventArgs e)
        {
            var bounds = e.Graphics.ClipBounds;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            Debug.WriteLine($"Painting {e.ClipRectangle}");
            using (var b = new SolidBrush(Color.FromArgb((int)(0xFF000000 | rng.Next()))))
                e.Graphics.FillRectangle(b, e.ClipRectangle);

            if (PropertiesCollection == null) return;      
            
            e.Graphics.DrawLine(Pens.Black, PropWidth, 0, PropWidth, Height);

            int sy = -ScrollY;
            int i;
            for (i = 0; i < PropertiesCollection.Length; i++) 
            {
                int editorY = sy + EditorsY[i];
                if (!Numbers.IntervalsIntersect(
                    editorY, editorY + EditorsHeight[i]-1,
                    e.ClipRectangle.Top, e.ClipRectangle.Bottom-1))
                {
                    continue;
                }

                Debug.WriteLine($"Paint Updated {Editors[i].Name}");

                e.Graphics.SetClip(bounds);

                TextRenderer.DrawText(e.Graphics, PropertiesCollection[i].Name, Font,
                    new Rectangle(0, editorY, PropWidth, EditorsHeight[i]),
                    ForeColor, BackColor, TextFormatFlags.Default);
                e.Graphics.DrawLine(Pens.Black, 0, editorY, Width, editorY);
                e.Graphics.SetClip(new Rectangle(PropWidth + 1, editorY + 1, EditorWidth - 1, EditorsHeight[i] - 1));
                Editors[i].OnPaint(e.Graphics, Font);
            }
            e.Graphics.SetClip(bounds);
            if (i > 0)
            {
                int eY = sy + EditorsY[i - 1] + EditorsHeight[i - 1];
                e.Graphics.DrawLine(Pens.Black, 0, eY, Width, eY);
            }
            base.OnPaint(e);
        }        

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateColumnWidths();
            UpdateSummonedControls();            
            Invalidate();
        }

        private void UpdateColumnWidths()
        {
            PropWidth = Math.Max(64, Width / 2);
            EditorWidth = Math.Max(Width - PropWidth - 10, 0);
        }

        private void Object_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            (var editor, var propData) = ByName[e.PropertyName];
            editor.Value = propData.Getter?.Invoke(fObject, null);
            editor.Invalidate();
        }

        private void DetachObject()
        {
            if (fObject == null) return;
            if (fObject is INotifyPropertyChanged obj)
            {
                obj.PropertyChanged -= Object_PropertyChanged;
            }
            PropertiesCollection = null;
            fObject = null;
        }

        private void AttachObject(object o)
        {
            DetachObject();
            if (o == null) return;
            fObject = o;
            if (fObject is INotifyPropertyChanged obj) 
            {
                obj.PropertyChanged += Object_PropertyChanged;
            }
            PropertiesCollection = PropertiesCollection.FromType(fObject.GetType());
        }

        private void RefreshEditors()
        {
            if (Editors != null)
            {
                for (int i = 0; i < Editors.Length; i++)
                {
                    Editors[i].OnInvalidate = null;
                    Editors[i].ByUserValueChanged -= PropertyEditorControl_ByUserValueChanged;
                    Editors[i].ControlSummoned -= Editor_ControlSummoned;
                    Editors[i].ControlDismissed -= Editor_ControlDismissed;
                }
                EditorsLayout.Clear();
                EditorProperty.Clear();
                ByName.Clear();
            }

            if(PropertiesCollection==null)
            {
                Editors = null;
                EditorsY = null;
                EditorsHeight = null;
                return;
            }

            int lineHeight = GetLineHeight();

            int length = PropertiesCollection.Length;
            Editors = new IEditor[length];
            EditorsY = new int[length];
            EditorsHeight = new int[length];

            int y = 0;
            for(int i=0;i<length;i++)
            {
                var propData = PropertiesCollection[i];
                var editor = PropertyEditors.CreateEditor(PropertiesCollection[i].PropertyType);

                editor.Name = propData.Name;
                editor.Value = propData.Getter?.Invoke(fObject, null);
                editor.ByUserValueChanged += PropertyEditorControl_ByUserValueChanged;
                editor.ControlSummoned += Editor_ControlSummoned;
                editor.ControlDismissed += Editor_ControlDismissed;
                editor.OnInvalidate = OnEditorInvalidate;
                int h = editor.RequestedTextRows * lineHeight;

                Editors[i] = editor;
                EditorsY[i] = y;                
                EditorsHeight[i] = h;
                y += h;

                ByName[propData.Name] = (editor, propData);
                EditorsLayout[editor] = (EditorsY[i], EditorsHeight[i]);
                EditorProperty[editor] = propData;                                
            }
            VirtualScroller.SetScrollableHeight(y);
        }

        private void Editor_ControlDismissed(object sender, EventArgs e)
        {
            var ed = sender as IEditor;
            Controls.Remove(ed.SummonedControl);            
        }

        private void Editor_ControlSummoned(object sender, EventArgs e)
        {            
            var ed = sender as IEditor;
            PlaceSummonedControl(ed.SummonedControl);
            Controls.Add(ed.SummonedControl);
            ed.SummonedControl.Focus();
        }

        private void PropertyEditorControl_ByUserValueChanged(object sender, ByUserValueChangedEventArgs e)
        {
            var editor = sender as IEditor;
            var edProp = EditorProperty[editor];
            edProp.Setter?.Invoke(fObject, new[] { editor.Value });
            if (!e.IsPreview) 
            {
                ByUserValueChangedNotPreview?.Invoke(this, new ByUserValueChangedEventArgs(e, edProp));
            }
        }

        public event EventHandler<ByUserValueChangedEventArgs> ByUserValueChangedNotPreview;

        private void OnEditorInvalidate(IEditor ed)
        {
            (int y, int h) = EditorsLayout[ed];
            Invalidate(new Rectangle(PropWidth, y - ScrollY, EditorWidth, h));
        }

        private int GetLineHeight()
        {            
            Font myFont = Font;
            FontFamily ff = myFont.FontFamily;
            float ascent = ff.GetCellAscent(myFont.Style);
            float descent = ff.GetCellDescent(myFont.Style);            
            float lineSpace = ff.GetLineSpacing(myFont.Style);
            float h = ascent + descent + lineSpace;
            int result = (int)(myFont.Size * h / ff.GetEmHeight(FontStyle.Regular));            
            return result;
        }

        private IEditor FocusedEditor = null;
        private void SetFocus(IEditor editor)
        {
            if (FocusedEditor == editor) return;
            FocusedEditor?.OnFocusLost();
            FocusedEditor = editor;
            FocusedEditor?.OnFocus();
        }

        private IEditor MsDownEditor = null;
        private int MsDownY;
        private int MsDownHeight;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
            int msY = ScrollY + e.Y;
            if (ColumnHitTest(e.X)) 
            {
                (MsDownEditor, MsDownY, MsDownHeight) = EditorHitTest(msY);
                MsDownEditor?.OnMouseDown(e.Button, e.X - PropWidth, msY - MsDownY);
                SetFocus(MsDownEditor);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {            
            base.OnMouseMove(e);
            int msY = ScrollY + e.Y;
            if (MsDownEditor!=null)
            {
                MsDownEditor.OnMouseMove(e.Button, e.X - PropWidth, msY - MsDownY);
            }
            else
            {
                if (ColumnHitTest(e.X)) 
                {
                    (var ed, var y, var h) = EditorHitTest(msY);
                    ed?.OnMouseMove(e.Button, e.X - PropWidth, msY - y);
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            int msY = ScrollY + e.Y;
            if (MsDownEditor!=null)
            {
                MsDownEditor.OnMouseUp(e.Button, e.X - PropWidth, msY - MsDownY);
                MsDownEditor = null;
            }
            else
            {
                if (ColumnHitTest(e.X))
                {
                    (var ed, var y, var h) = EditorHitTest(msY);
                    ed?.OnMouseUp(e.Button, e.X - PropWidth, msY - y);
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
        }

        private bool ColumnHitTest(int x) => PropWidth <= x && x < PropWidth + EditorWidth;


        private (IEditor, int, int) EditorHitTest(int y)
        {
            if (Editors == null) return (null, 0, 0);
            // TO DO: Binary Search
            for(int i=0;i<Editors.Length;i++)
            {
                if (EditorsY[i] <= y && y < EditorsY[i] + EditorsHeight[i])
                    return (Editors[i], EditorsY[i], EditorsHeight[i]);
            }
            return (null, 0, 0);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            FocusedEditor?.OnKeyPress(e);
        }        


        protected override void OnKeyDown(KeyEventArgs e)
        {            
            base.OnKeyDown(e);
            Debug.WriteLine($"Key down: {e.KeyCode} {e.KeyCode == Keys.Left}");
            FocusedEditor?.OnKeyDown(e);
            e.Handled = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            FocusedEditor?.OnKeyUp(e);
            e.Handled = true;
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            SetFocus(null);
        }

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (FocusedEditor?.SummonedControl?.Focused ?? false) 
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            if(msg.Msg == WM_KEYDOWN)
            {
                OnKeyDown(new KeyEventArgs(keyData));
                return true;
            }
            if(msg.Msg == WM_KEYUP)
            {
                OnKeyUp(new KeyEventArgs(keyData));
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);

            /*if ((keyData == Keys.Right) || (keyData == Keys.Left) ||
                (keyData == Keys.Up) || (keyData == Keys.Down))
            {
                //Do custom stuff
                //true if key was processed by control, false otherwise
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }*/
        }

        private void PlaceSummonedControl(Control control)
        {            
            var stats = control.Tag as IEditor.SummonedControlStats;
            if (stats == null) return;            
            (int ey, int eh) = EditorsLayout[stats.Editor];
            int x = stats.InnerBounds.X + PropWidth;
            int y = stats.InnerBounds.Y + ey;
            int w = stats.InnerBounds.Width;
            w = w <= 0 ? EditorWidth + w : w;
            int h = stats.InnerBounds.Height;
            h = h <= 0 ? eh + h : h;
            stats.Editor.SummonedControl?.SetBounds(x, y - ScrollY, w, h);
        }

        private void UpdateSummonedControls()
        {
            for (int i = 0; i < Controls.Count; i++)
                PlaceSummonedControl(Controls[i]);
        }

    }
}
