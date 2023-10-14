using FlipnoteDotNet.Utils.Temporal;
using System;

namespace FlipnoteDotNet.Data
{    
    public interface ILayer : ITemporalContext, ITimeLocalizable
    {
        event EventHandler UserUpdate;
        void TriggerUserUpdate();
    }
}
