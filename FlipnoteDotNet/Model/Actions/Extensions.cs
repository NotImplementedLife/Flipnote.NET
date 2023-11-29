using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlipnoteDotNet.Model.Actions
{
    internal static class Extensions
    {
        public static IEnumerable<IEntityReference<Sequence>> EnumerateSequences(this IEntityReference<FlipnoteProject> proj)
        {
            return proj.Entity.Tracks.SelectMany(t => t.Entity.Sequences);
        }

        public static IEnumerable<IEntityReference<Layer>> EnumerateLayers(this IEntityReference<FlipnoteProject> proj)
        {
            return proj.Entity.Tracks.SelectMany(t => t.Entity.Sequences.SelectMany(s => s.Entity.Layers));
        }
    }
}
