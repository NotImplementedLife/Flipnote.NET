﻿using FlipnoteDotNet.Data;
using FlipnoteDotNet.Rendering;

namespace FlipnoteDotNet.Extensions
{
    internal static class SequenceExtensions
    {

        public static LayerRenderingOptions GetRenderingOptions(this Sequence s)
        {
            return new LayerRenderingOptions(s.PaperColor, s.Pen1, s.Pen2);
        }
    }
}
