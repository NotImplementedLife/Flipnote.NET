using FlipnoteDotNet.Data;
using FlipnoteDotNet.Utils.Manipulator;
using PPMLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FlipnoteDotNet.Rendering.Frames
{
    internal static class FlipnoteFramesRenderer
    {
        private static FlipnoteFrame RenderFrame(RenderFrameData frameData)
        {            
            var surface = new FrameRenderSurface();
            foreach(var layer in frameData.Layers)
            {
                var renderer = Manipulators.CreateManipulator<ILayerRenderer>(layer);
                if (renderer == null)
                    throw new InvalidOperationException($"No manipulator for {layer?.GetType()?.ToString() ?? "null"}");
                renderer?.Render(surface, layer, frameData.Timestamp);
            }
            return surface.ToFlipnoteFrame(frameData.PaperColor, frameData.Pen1, frameData.Pen2);
        }

        class RenderFrameData
        {
            public List<ILayer> Layers { get; } = new List<ILayer>();
            public FlipnotePaperColor PaperColor { get; set; } = FlipnotePaperColor.White;
            public FlipnotePen Pen1 { get; set; } = FlipnotePen.PaperInverse;
            public FlipnotePen Pen2 { get; set; } = FlipnotePen.Red;
            public int Timestamp { get; set; }
        }

        public static List<FlipnoteFrame> CreateFrames(SequenceManager manager)
        {
            int framesCount = 999;
            var framesData = new RenderFrameData[framesCount];
            for (int i = 0; i < framesCount; i++) framesData[i] = new RenderFrameData();

            var tracks = manager.GetTracks().Reverse<SequenceTrack>();

            foreach(var track in tracks)
            {
                foreach (var sequence in track.GetSequences().Reverse()) 
                {
                    int i0 = Math.Max(0, sequence.StartFrame);
                    int i1 = Math.Min(framesCount - 1, sequence.EndFrame);
                    for (int i = i0; i < i1; i++) 
                    {
                        foreach (var layer in sequence.Layers) framesData[i].Layers.Add(layer);
                        framesData[i].Timestamp = i;
                        framesData[i].PaperColor = sequence.PaperColor;
                        framesData[i].Pen1 = sequence.Pen1;
                        framesData[i].Pen2 = sequence.Pen2;
                    }
                    Thread.Sleep(20);
                }                
            }                      

            var frames = new List<FlipnoteFrame>();
            for (int i = 0; i < framesCount; i++) 
            {
                frames.Add(RenderFrame(framesData[i]));
            }
            return frames;
        }
    }
}
