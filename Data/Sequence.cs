using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Data.StateChangeGenerators;
using System.Collections.Generic;
using System.Drawing;

namespace FlipnoteDotNet.Data
{
    internal class Sequence : ICloneable
    {
        public class Element
        {
            public ILayer Layer { get; }
            public CummulativeStateChangeGenerator StateChangeGenerator = new CummulativeStateChangeGenerator();

        }
        public List<Element> Elements { get; private set; } = new List<Element>();

        [Editable]
        [Atemporal]
        public string Name { get; set; } = "";

        [Editable]
        [Atemporal]
        public Color Color { get; set; } = Color.DodgerBlue;

        public Sequence Clone()
        {
            return new Sequence
            {
                Name = Name,
                Color = Color,
                Elements = Elements
            };
        }

        ICloneable ICloneable.Clone() => Clone();
    }
}
