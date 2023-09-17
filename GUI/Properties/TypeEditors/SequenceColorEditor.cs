using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Properties.EditorFields;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace FlipnoteDotNet.GUI.Properties.TypeEditors
{
    public class SequenceColorEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {            
            return UITypeEditorEditStyle.DropDown;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context) => true;        

        public override void PaintValue(PaintValueEventArgs e)
        {                        
            var color = (Color)e.Value;

            using (var b = new SolidBrush(color))
                e.Graphics.FillRectangle(b, e.Bounds);                           
            //base.PaintValue(e);

        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            var color = (Color)value;
            var box = new EditorFields.SequenceColorEditor(color, edSvc);
            edSvc.DropDownControl(box);
            return box.Selection;
        }
    }
}
