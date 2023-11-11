using FlipnoteDotNet.Data.Entities;

namespace FlipnoteDotNet.Model.Entities
{
    public class Layer : Entity
    {
        public string Name { get; set; }

        [Temporal]
        public int X { get; set; }

        [Temporal]
        public int Y { get; set; }

        [Temporal]
        public int Width { get; set; }

        [Temporal]
        public int Height { get; set; }

        [Temporal]
        public float Rotation { get; set; }        
    }
}
