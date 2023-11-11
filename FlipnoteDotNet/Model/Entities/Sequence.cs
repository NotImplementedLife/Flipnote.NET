using FlipnoteDotNet.Data.Entities;
using PPMLib.Data;

namespace FlipnoteDotNet.Model.Entities
{
    public sealed class Sequence : Entity
    {
        public string Name { get; set; }

        public int StartFrame { get; set; }
        public int EndFrame { get; set; }

        [Temporal]
        public FlipnotePaperColor PaperColor { get; set; }

        [Temporal]
        public FlipnotePen Pen1 { get; set; }

        [Temporal]
        public FlipnotePen Pen2 { get; set; }

        public EntityList<Layer> Layers { get; set; } = new EntityList<Layer>();
    }
}
