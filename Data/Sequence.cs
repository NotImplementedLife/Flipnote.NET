using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Data.StateChangeGenerators;
using System.Collections.Generic;
using System.Drawing;

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

        [Editable]
        public string Name { get; set; } = "";

        [Editable]
        public Color Color { get; set; } = Color.DodgerBlue;
    }
}
