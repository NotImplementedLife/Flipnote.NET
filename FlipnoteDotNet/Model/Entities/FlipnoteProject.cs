using FlipnoteDotNet.Data.Entities;

namespace FlipnoteDotNet.Model.Entities
{
    public sealed class FlipnoteProject : Entity
    {
        public string Name { get; set; }
        public EntityList<Track> Tracks { get; set; } = new EntityList<Track>();        
    }
}
