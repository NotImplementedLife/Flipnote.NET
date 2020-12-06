using FlipnoteDesktop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDesktop.Environment.Canvas
{
    /// <summary>
    /// Automatic frames renderer
    /// </summary>
    public abstract class Generator
    {
        public Generator(bool usesBothLayers = false)
        {
            UsesBothLayers = usesBothLayers;
        }

        public abstract string Name { get; }
        public bool UsesBothLayers { get; }

        /// <summary>
        /// Generator entry point
        /// </summary>
        /// <param name="target">The frames list where the generation occurs.</param>
        public void Execute(List<DecodedFrame> target)
        {
            // get generated frames through the generator logic 
            var gframes = GenerateFrames();
            // For experimental reasons, let's just replace the original frames with the 
            // generated ones.
            target.Clear();
            target.AddRange(gframes);
        }

        public abstract List<DecodedFrame> GenerateFrames();
    }
}
