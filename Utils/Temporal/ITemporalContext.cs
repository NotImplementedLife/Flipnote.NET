using System;

namespace FlipnoteDotNet.Utils.Temporal
{
    public interface ITemporalContext
    {
        int CurrentTimestamp { get; set; }

        event EventHandler CurrentTimestampChanged;
    }
}
