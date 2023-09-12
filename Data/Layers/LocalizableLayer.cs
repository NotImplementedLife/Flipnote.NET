using FlipnoteDotNet.Attributes;

namespace FlipnoteDotNet.Data.Layers
{
    public abstract class LocalizableLayer : AbstractLayer
    {
        [Editable]
        [Description("Object X position")]
        public int X { get; set; }

        [Editable]
        [Description("Object Y position")]
        public int Y { get; set; }        
    }
}
