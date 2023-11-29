using FlipnoteDotNet.Data.Entities;
using System.ComponentModel;

namespace FlipnoteDotNet.Model.Entities
{
    public class Layer : Entity
    {
        [DefaultValue("Layer")]
        public string Name { get; set; }

        [Temporal]
        public int X { get; set; }

        [Temporal]
        public int Y { get; set; }

        [Temporal]
        [DefaultValue(10)]
        public int Width { get; set; }

        [Temporal]
        [DefaultValue(10)]
        public int Height { get; set; }

        [Temporal]
        public float Rotation { get; set; }        
    }
}
