using FlipnoteDotNet.Data.Entities;

namespace FlipnoteDotNet.Model.Entities
{
    public sealed class Track : Entity
    {
        public EntityList<Sequence> Sequences { get; set; } = new EntityList<Sequence>();
    }
}
