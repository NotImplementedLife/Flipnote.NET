using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Data.Manager;

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
            ctx.SelectedEntity?.SwitchTimestamp(Timestamp);
            ctx.Timestamp = Timestamp;
        }

        public override void Undo(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {
            ctx.Project.SwitchTimestamp(oldTimestamp);
            ctx.SelectedEntity?.SwitchTimestamp(oldTimestamp);
            ctx.Timestamp = oldTimestamp;
        }
    }
}
