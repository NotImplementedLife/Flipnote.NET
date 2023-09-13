using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Utils;
using FlipnoteDotNet.Utils.Temporal;

namespace FlipnoteDotNet.Data.Layers
{
    public abstract class LocalizableLayer : AbstractLayer, ITemporalContext
    {
        protected LocalizableLayer()
        {
            X = new TimeDependentValue<int>(this);
            Y = new TimeDependentValue<int>(this);

            this.Initialize();
        }

        [Editable]
        [Description("Object X position")]
        public TimeDependentValue<int> X { get; }

        [Editable]
        [Description("Object Y position")]
        public TimeDependentValue<int> Y { get; }        
    }
}
