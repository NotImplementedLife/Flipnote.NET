using FlipnoteDotNet.Commons;
using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Data.Manager;
using FlipnoteDotNet.Model.Actions;
using FlipnoteDotNet.Model.Entities;
using System;
using System.Reflection;

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
        }        

        public void CreateNewProject()
        {
            ProjectPath = null;
            Context.Reset();
            Manager.WithDatabase(db =>
            {
                db.Clear();
                var proj = db.Create<FlipnoteProject>();                
                Commons.Utils.Repeat(5, () => proj.Entity.Tracks.Add(db.Create<Track>()));
                proj.Entity.Name = "My Flipnote project";
                proj.Commit();
                Context.Project = proj;
                ProjectChanged?.Invoke(this, EventArgs.Empty);
            });            
        }

        public void SetCurrentFrame(int frame)
        {
            Manager.DoAction(new ChangeTimestampAction(frame));
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

        public void SelectLayer(int layerId)
        {
            Manager.DoAction(new SelectLayerAction(layerId));
        }

        public void AddLayerToSelectedSequence(Type layerType)
        {
            Manager.DoAction(new AddLayerAction(layerType, () => LayersListChanged?.Invoke(this, EventArgs.Empty)));
        }        

        public void ChangeSelectedEntityProperty(PropertyInfo property, object value)
        {
            Manager.DoAction(new SelectedEntityPropertyChangedAction(property, value,
                () => SelectedEntityPropertyChanged?.Invoke(this, EventArgs.Empty)));
        }

        public event EventHandler<IEntityReference<Sequence>> SelectedSequenceChanged
        {
            add => Context.SelectedSequenceChanged += value;
            remove => Context.SelectedSequenceChanged -= value;
        }

        public event EventHandler<IEntityReference<Layer>> SelectedLayerChanged
        {
            add => Context.SelectedLayerChanged += value;
            remove => Context.SelectedLayerChanged -= value;
        }

        public event EventHandler SelectedEntityPropertyChanged;

        public event EventHandler LayersListChanged;
        public event EventHandler ProjectChanged;

        public event EventHandler<IEntityReference<Entity>> SelectedEntityChanged
        {
            add => Context.SelectedEntityChanged += value;
            remove => Context.SelectedEntityChanged -= value;
        }        
        public event EventHandler ActionsListChanged
        {
            add => Manager.ActionsListChanged += value;
            remove => Manager.ActionsListChanged -= value;
        }

        public event EventHandler<int> CurrentFrameChanged
        {
            add => Context.TimestampChanged += value;
            remove => Context.TimestampChanged -= value;
        }

        public event EventHandler TracksChanged;

        public bool CanUndo => Manager.CanUndo;
        public bool CanRedo => Manager.CanRedo;

        public void Undo() => Manager.UndoLastAction();
        public void Redo() => Manager.RedoLastAction();

        public IEntityReference<FlipnoteProject> Project => Context.Project;
        public IEntityReference<Sequence> SelectedSequence => Context.SelectedSequence;
        public IEntityReference<Entity> SelectedEntity => Context.SelectedEntity;
        public int Timestamp => Context.Timestamp;
    }
}
