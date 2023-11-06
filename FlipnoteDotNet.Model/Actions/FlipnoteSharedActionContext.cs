using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Data.Manager;
using FlipnoteDotNet.Model.Entities;

namespace FlipnoteDotNet.Model.Actions
{
    public class FlipnoteSharedActionContext : ISharedActionContext
    {
        public int Timestamp { get; set; } = 0;

        public IEntityReference<FlipnoteProject> Project { get; set; }        

        public IEntityReference<Entity> SelectedEntity { get; set; }        
    }
}
