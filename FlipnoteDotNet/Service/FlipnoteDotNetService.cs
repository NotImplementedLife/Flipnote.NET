using FlipnoteDotNet.Commons;
using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Data.Manager;
using FlipnoteDotNet.Model.Actions;
using FlipnoteDotNet.Model.Entities;
using System;
using System.Diagnostics;

namespace FlipnoteDotNet.Service
{
    public class FlipnoteDotNetService
    {
        private readonly FlipnoteSharedActionContext Context = new FlipnoteSharedActionContext();
        private readonly DatabaseManager Manager = new DatabaseManager();
        private string ProjectPath = null;

        public bool IsProjectPathSet => ProjectPath != null;

        public FlipnoteDotNetService()
        {
            Manager.ActionContext = Context;

            Context.SelectedSequenceChanged += Context_SelectedSequenceChanged;
            Context.SelectedEntityChanged += Context_SelectedEntityChanged;
            Context.SelectedLayerChanged += Context_SelectedLayerChanged;
        }

        private void Context_SelectedLayerChanged(object sender, IEntityReference<Layer> e)
        {
            SelectedLayerChanged?.Invoke(sender, e);
        }

        private void Context_SelectedEntityChanged(object sender, IEntityReference<Entity> e)
        {
            SelectedEntityChanged?.Invoke(sender, e);
        }

        private void Context_SelectedSequenceChanged(object sender, IEntityReference<Sequence> e)
        {
            SelectedSequenceChanged?.Invoke(sender, e);
        }

        public void CreateNewProject()
        {
            ProjectPath = null;
            Context.Reset();
            Manager.WithDatabase(db =>
            {
                db.Clear();
                var proj = db.Create<FlipnoteProject>();
                Utils.Repeat(5, () => proj.Entity.Tracks.Add(db.Create<Track>()));
                proj.Entity.Name = "My Flipnote project";
                proj.Commit();
                Context.Project = proj;
                ProjectChanged?.Invoke(this, EventArgs.Empty);
            });            
        }

        public void LoadProject(string filename)
        {
            Manager.LoadFromFile(filename);
            ProjectPath = filename;
            Context.Reset();
            Manager.WithDatabase(db =>
            {
                Context.Project = db.FindFirst<FlipnoteProject>();
                ProjectChanged?.Invoke(this, EventArgs.Empty);
            });
        }

        public void SaveProject()
        {
            Manager.SaveToFile(ProjectPath ?? throw new InvalidOperationException("Project path not set"));
        }

        public void SaveProject(string filename)
        {
            Manager.SaveToFile(ProjectPath = filename);
        }

        public void AddSequence(int trackId, int startFrame, int endFrame)
        {
            Manager.DoAction(new AddSequenceAction(trackId, startFrame, endFrame,
                () => TracksChanged?.Invoke(this, EventArgs.Empty)));
        }

        public void MoveSequence(int sequenceId, int trackId, int startFrame, int endFrame)
        {
            Manager.DoAction(new MoveSequenceAction(sequenceId, trackId, startFrame, endFrame,
                () => TracksChanged?.Invoke(this, EventArgs.Empty)));
        }

        public void SelectSequence(int sequenceId)
        {
            Manager.DoAction(new SelectSequenceAction(sequenceId));
        }

        public void AddLayerToSelectedSequence(Type layerType)
        {
            Manager.DoAction(new AddLayerAction(layerType, () => LayersListChanged?.Invoke(this, EventArgs.Empty)));
        }

        public event EventHandler<IEntityReference<Sequence>> SelectedSequenceChanged;
        public event EventHandler<IEntityReference<Layer>> SelectedLayerChanged;
        public event EventHandler<IEntityReference<Entity>> SelectedEntityChanged;
        public event EventHandler LayersListChanged;
        


        public event EventHandler ProjectChanged;
        public event EventHandler ActionsListChanged
        {
            add => Manager.ActionsListChanged += value;
            remove => Manager.ActionsListChanged -= value;
        }

        public event EventHandler TracksChanged;

        public bool CanUndo => Manager.CanUndo;
        public bool CanRedo => Manager.CanRedo;

        public void Undo() => Manager.UndoLastAction();
        public void Redo() => Manager.RedoLastAction();

        public IEntityReference<FlipnoteProject> Project => Context.Project;
        public IEntityReference<Sequence> SelectedSequence => Context.SelectedSequence;
    }
}
