using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Data.Drawing;
using FlipnoteDotNet.GUI.Forms.LayerCreators;
using FlipnoteDotNet.Utils;
using FlipnoteDotNet.Utils.Temporal;

namespace FlipnoteDotNet.Data.Layers
{
    [Layer(DisplayName ="Static image layer", CreatorForm = typeof(StaticImageLayerCreatorForm))]
    internal class StaticImageLayer : LocalizableLayer, IDisplayLayer
    {
        [Editable]
        [DisplayName("Visual")]                      
        public FlipnoteVisualSource VisualSource { get; set; }

        [Editable]
        [DisplayName("Name")]
        public string DisplayName { get; set; }


        [Editable]        
        public TimeDependentValue<float> ScaleX { get; }

        [Editable]
        public TimeDependentValue<float> ScaleY { get; }

        public StaticImageLayer(FlipnoteVisualSource visualSource)
        {
            VisualSource = visualSource;
            ScaleX = new TimeDependentValue<float>(this, 1);
            ScaleY = new TimeDependentValue<float>(this, 1);
            this.Initialize();
        }
    }
}
