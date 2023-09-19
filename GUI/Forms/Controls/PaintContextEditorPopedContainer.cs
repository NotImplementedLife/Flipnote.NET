using FlipnoteDotNet.GUI.Controls.Primitives;
using FlipnoteDotNet.Utils.Paint;
using System;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Forms.Controls
{
    internal class PaintContextEditorPopedContainer : PopedCotainer
    {
        public IPaintContextEditor PaintContextEditor { get; }
        public PaintContext PaintContext { get; }
        public PaintContextEditorPopedContainer(IPaintContextEditor paintContextEditor, PaintContext paintContext)
        {
            PaintContextEditor = paintContextEditor;
            PaintContext = paintContext;         
            Controls.Add(PaintContextEditor as Control);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            PaintContextEditor.LoadPaintContext(PaintContext);
            Width = (PaintContextEditor as Control).Width;
            Height = (PaintContextEditor as Control).Height;
        }
    }
}
