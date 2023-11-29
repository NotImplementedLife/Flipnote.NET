using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Data.Manager;
using FlipnoteDotNet.Model.Entities;
using System;
using System.Diagnostics;
using System.Linq;

namespace FlipnoteDotNet.Model.Actions
{
    public class ChangeTimestampAction : DatabaseAction<FlipnoteSharedActionContext>
    {
        private readonly int Timestamp;
        private int oldTimestamp;

        public ChangeTimestampAction(int timestamp)
        {
            Timestamp = timestamp;
        }

        public override void Do(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {
            oldTimestamp = ctx.Timestamp;            
            ctx.Project.SwitchTimestamp(Timestamp);                     

            if (ctx.SelectedSequence != null)
                ctx.SelectedSequence = ctx.Project.EnumerateSequences().Where(s => s.Id == ctx.SelectedSequence.Id).First();
            if (ctx.SelectedLayer != null)
                ctx.SelectedLayer = ctx.Project.EnumerateLayers().Where(s => s.Id == ctx.SelectedLayer.Id).First();            


            if (ctx.SelectedEntity is IEntityReference<Sequence>)
                ctx.SelectedEntity = ctx.SelectedSequence;
            else if (ctx.SelectedEntity is IEntityReference<Layer>)
                ctx.SelectedEntity = ctx.SelectedLayer;            

            ctx.Timestamp = Timestamp;
        }

        public override void Undo(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {
            ctx.Project.SwitchTimestamp(oldTimestamp);


            if (ctx.SelectedSequence != null)
                ctx.SelectedSequence = ctx.Project.EnumerateSequences().Where(s => s.Id == ctx.SelectedSequence.Id).First();
            if (ctx.SelectedLayer != null)
                ctx.SelectedLayer = ctx.Project.EnumerateLayers().Where(s => s.Id == ctx.SelectedLayer.Id).First();

            if (ctx.SelectedEntity is IEntityReference<Sequence>)
                ctx.SelectedEntity = ctx.SelectedSequence;
            else if (ctx.SelectedEntity is IEntityReference<Layer>)
                ctx.SelectedEntity = ctx.SelectedLayer;

            ctx.Timestamp = oldTimestamp;
        }
    }
}
