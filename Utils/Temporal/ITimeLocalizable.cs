﻿namespace FlipnoteDotNet.Utils.Temporal
{
    public interface ITimeLocalizable : ITemporalContext
    {
        int StartTimestamp { get; set; }
    }
}
