using FlipnoteDotNet.Utils.Temporal;
using System;

namespace FlipnoteDotNet.Data.Layers
{
    public abstract class AbstractLayer : AbstractTransformableTemporalContext, ILayer
    {
        public event EventHandler UserUpdate;

        public void TriggerUserUpdate()
        {
            UserUpdate?.Invoke(this, new EventArgs());
        }
    }
}
