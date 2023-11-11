using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Data.Manager;
using FlipnoteDotNet.Model.Entities;
using System;

namespace FlipnoteDotNet.Model.Actions
{
    /// <summary>
    /// Adds a new Sequence to a track. Changes are visible in Context.Project
    /// </summary>
    public class AddSequenceAction : DatabaseAction<FlipnoteSharedActionContext>
    {
        private int TrackId;
        private int StartFrame;
        private int EndFrame;
        private Action Callback;
        private int SequenceId = -1;        

        public AddSequenceAction(int trackId, int startFrame, int endFrame, Action callback = null)
        {
            TrackId = trackId;
            StartFrame = startFrame;
            EndFrame = endFrame;
            Callback = callback;
        }

        public override void Do(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {
            var track = ctx.Project.Entity.Tracks[TrackId];

            var seq = SequenceId < 0 ? db.Create<Sequence>() : db.Create<Sequence>(SequenceId);   
            seq.Entity.StartFrame = StartFrame;
            seq.Entity.EndFrame = EndFrame;
            seq.Entity.Name = "Sequence";            
            track.Entity.Sequences.Add(seq);
            seq.Commit();
            track.Commit();

            SequenceId = seq.Id;

            Callback?.Invoke();
        }

        public override void Undo(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {
            var track = ctx.Project.Entity.Tracks[TrackId];
            var seq = track.Entity.Sequences[track.Entity.Sequences.Count - 1];
            db.RemoveById(seq.Id);
            track.Entity.Sequences.RemoveAt(track.Entity.Sequences.Count - 1);
            track.Commit();
            Callback?.Invoke();
        }
    }
}
