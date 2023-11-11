using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Data.Manager;
using FlipnoteDotNet.Model.Entities;
using System;
using System.Linq;

namespace FlipnoteDotNet.Model.Actions
{
    public class SelectSequenceAction : DatabaseAction<FlipnoteSharedActionContext>
    {
        private readonly int SequenceId;        
        private IEntityReference<Entity> OldSelectedEntity;
        private IEntityReference<Sequence> OldSelectedSequence;        
        private IEntityReference<Layer> OldSelectedLayer;

        public SelectSequenceAction(int sequenceId)
        {
            SequenceId = sequenceId;            
        }

        public override void Do(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {
            IEntityReference<Sequence> sequence = SequenceId < 0 ? null
                : ctx.Project.Entity.Tracks.SelectMany(_ => _.Entity.Sequences.Where(s => s.Id == SequenceId)).FirstOrDefault();                
            OldSelectedEntity = ctx.SelectedEntity;
            OldSelectedSequence = ctx.SelectedSequence;
            OldSelectedLayer = ctx.SelectedLayer;
            ctx.SelectedEntity = ctx.SelectedSequence = sequence;
            ctx.SelectedLayer = null;
        }

        public override void Undo(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {
            ctx.SelectedEntity = OldSelectedEntity;
            ctx.SelectedSequence = OldSelectedSequence;
            ctx.SelectedLayer = OldSelectedLayer;
        }
    }
}
