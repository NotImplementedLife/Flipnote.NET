using FlipnoteDotNet.Data.Entities;

namespace FlipnoteDotNet.Model.Entities
{
    public class Track : Entity
    {
        public EntityList<Sequence> Sequences { get; set; } = new EntityList<Sequence>();
    }
}
