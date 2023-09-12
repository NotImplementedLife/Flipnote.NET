using FlipnoteDotNet.Data.StateChangeGenerators;
using System.Collections.Generic;

namespace FlipnoteDotNet.Data
{
    internal class Sequence
    {
        public class Element
        {
            public ILayer Layer { get; }
            public CummulativeStateChangeGenerator StateChangeGenerator = new CummulativeStateChangeGenerator();

        }
        public List<Element> Elements { get; } = new List<Element>();

    }
}
