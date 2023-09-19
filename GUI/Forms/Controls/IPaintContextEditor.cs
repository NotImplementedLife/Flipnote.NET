using FlipnoteDotNet.Utils.Paint;

namespace FlipnoteDotNet.GUI.Forms.Controls
{
    public interface IPaintContextEditor
    {
        PaintContext PaintContext { get; }

        void LoadPaintContext(PaintContext pc);
    }
}
