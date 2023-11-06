using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Data.Manager;
using FlipnoteDotNet.Model.Entities;
using System.ComponentModel;

namespace FlipnoteDotNet.Model.Actions
{
    public class AddSequenceAction : DatabaseAction<FlipnoteSharedActionContext>
    {
        private int TrackId;
        private int StartFrame;
        private int EndFrame;        

        public AddSequenceAction(int trackId, int startFrame, int endFrame)
        {
            TrackId = trackId;
            StartFrame = startFrame;
            EndFrame = endFrame;
        }

        public override void Do(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {
            var track = ctx.Project.Entity.Tracks[TrackId];

            var seq = db.Create<Sequence>();            
            seq.Entity.StartFrame = StartFrame;
            seq.Entity.EndFrame = EndFrame;
            seq.Entity.Name = "Sequence";            
            track.Entity.Sequences.Add(seq);
            seq.Commit();
            track.Commit();                       
        }

        public override void Undo(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {
            var track = ctx.Project.Entity.Tracks[TrackId];
            var seq = track.Entity.Sequences[track.Entity.Sequences.Count - 1];
            db.RemoveById(seq.Id);
            track.Entity.Sequences.RemoveAt(track.Entity.Sequences.Count - 1);
            track.Commit();
        }
    }
}
