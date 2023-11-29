using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Data.Manager;
using FlipnoteDotNet.Model.Entities;
using System.Linq;

namespace FlipnoteDotNet.Model.Actions
{
    public class SelectLayerAction : DatabaseAction<FlipnoteSharedActionContext>
    {
        private readonly int LayerId;
        private IEntityReference<Entity> OldSelectedEntity;        
        private IEntityReference<Layer> OldSelectedLayer;

        public SelectLayerAction(int layerId)
        {
            LayerId = layerId;
        }

        public override void Do(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {
            IEntityReference<Layer> layer = LayerId < 0 ? null
                : ctx.SelectedSequence.Entity.Layers.Where(l => l.Id == LayerId).FirstOrDefault();
            OldSelectedEntity = ctx.SelectedEntity;            
            OldSelectedLayer = ctx.SelectedLayer;
            ctx.SelectedEntity = ctx.SelectedLayer = layer;            
        }

        public override void Undo(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {
            ctx.SelectedEntity = OldSelectedEntity;            
            ctx.SelectedLayer = OldSelectedLayer;
        }
    }
}
