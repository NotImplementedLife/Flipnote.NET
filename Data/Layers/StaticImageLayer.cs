using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Data.Drawing;
using FlipnoteDotNet.Utils;

namespace FlipnoteDotNet.Data.Layers
{
    internal class StaticImageLayer : LocalizableLayer, IDisplayLayer
    {
        [Editable]
        [DisplayName("Visual")]                      
        public FlipnoteVisualSource VisualSource { get; set; }

        [Editable]
        [DisplayName("Name")]
        public string DisplayName { get; set; }

        public StaticImageLayer(FlipnoteVisualSource visualSource)
        {
            VisualSource = visualSource;
            this.Initialize();
        }
    }
}
