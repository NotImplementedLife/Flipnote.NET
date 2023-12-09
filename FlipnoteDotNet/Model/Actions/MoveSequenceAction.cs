using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Data.Manager;
using FlipnoteDotNet.Model.Entities;
using System;
using System.Linq;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace FlipnoteDotNet.Model.Actions
{
    internal class MoveSequenceAction : DatabaseAction<FlipnoteSharedActionContext>
    {
        private int SequenceId;
        private int TrackId;
        private int StartFrame;
        private int EndFrame;

        private int OldTrackId;
        private int OldStartFrame;
        private int OldEndFrame;

        private Action Callback;

        public MoveSequenceAction(int sequenceId, int trackId, int startFrame, int endFrame, Action callback)
        {
            SequenceId = sequenceId;
            TrackId = trackId;
            StartFrame = startFrame;
            EndFrame = endFrame;
            Callback = callback;
        }

        public override void Do(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {            
            for(int i=0;i<ctx.Project.Entity.Tracks.Count;i++)
            {
                var track = ctx.Project.Entity.Tracks[i];
                if (track.Entity.Sequences.Any(s => s.Id == SequenceId)) 
                {
                    var seq = track.Entity.Sequences.Where(s => s.Id == SequenceId).First();
                    (OldStartFrame, OldEndFrame) = (seq.Entity.StartFrame, seq.Entity.EndFrame);
                    OldTrackId = i;
                    var newTrack = ctx.Project.Entity.Tracks[TrackId];

                    track.Entity.Sequences.Remove(seq);
                    track.Commit();

                    newTrack.Entity.Sequences.Add(seq);
                    newTrack.Commit();

                    seq.Entity.StartFrame = StartFrame;
                    seq.Entity.EndFrame = EndFrame;                    
                    seq.Commit();
                    seq.MoveInTime(StartFrame-OldStartFrame);

                    foreach (var layer in seq.Entity.Layers)
                    {
                        layer.MoveInTime(StartFrame - OldStartFrame);
                        if (ctx.SelectedEntity?.Id == layer.Id)
                            ctx.SelectedEntity = layer;
                        if (ctx.SelectedLayer?.Id == layer.Id)
                            ctx.SelectedLayer = layer;
                    }

                    Callback?.Invoke();
                    return;
                }
            }
            throw new InvalidOperationException("No track contains this sequence");            
        }

        public override void Undo(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {            
            var oldTrack = ctx.Project.Entity.Tracks[OldTrackId];
            var newTrack = ctx.Project.Entity.Tracks[TrackId];

            var seq = newTrack.Entity.Sequences.Where(s => s.Id == SequenceId).First();
            if (seq == null)
                throw new InvalidOperationException("Sequence was null");

            oldTrack.Entity.Sequences.Add(seq);
            oldTrack.Commit();

            newTrack.Entity.Sequences.Remove(seq);
            newTrack.Commit();

            var startFrame = seq.Entity.StartFrame;

            seq.Entity.StartFrame = OldStartFrame;
            seq.Entity.EndFrame = OldEndFrame;
            seq.Commit();
            seq.MoveInTime(OldStartFrame - startFrame);

            foreach (var layer in seq.Entity.Layers)
            {
                layer.MoveInTime(OldStartFrame - startFrame);
                if (ctx.SelectedEntity?.Id == layer.Id)
                    ctx.SelectedEntity = layer;
                if (ctx.SelectedLayer?.Id == layer.Id)
                    ctx.SelectedLayer = layer;
            }
            

            Callback?.Invoke();
        }
    }
}
