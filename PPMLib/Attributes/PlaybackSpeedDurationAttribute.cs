using System;
using System.Collections.Generic;
using System.Text;

namespace PPMLib.Attributes
{
    /// <summary>
    /// Describes playback speed in terms of Nintendo DS vertical blank periods count.    
    /// </summary>
    /// <example>For Value=15, it means that a flipnote frame is played once every 15 VBlanks, or ~0.25 seconds</example>
    [AttributeUsage(AttributeTargets.All)]
    public class PlaybackSpeedDurationAttribute : Attribute
    {
        public int Value { get; set; }

        public PlaybackSpeedDurationAttribute(int value)
        {
            Value = value;
        }
    }
}
