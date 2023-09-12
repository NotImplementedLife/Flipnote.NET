using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Data.Drawing;

namespace FlipnoteDotNet.Data.Layers
{
    internal class StaticImageLayer : LocalizableLayer
    {
        [Editable]
        [DisplayName("Visual")]
        public FlipnoteVisualSource VisualSource { get; set; }

        public StaticImageLayer(int x, int y, FlipnoteVisualSource visualSource)
        {
            X = x;
            Y = y;
            VisualSource = visualSource;
        }

        public override ILayer Clone() => new StaticImageLayer(X, Y, VisualSource?.Clone());
    }
}
