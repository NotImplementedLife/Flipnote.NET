using FlipnoteDotNet.Commons;
using FlipnoteDotNet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FlipnoteDotNet.Data.Manager
{
    public class DatabaseManager
    {
        private EntityDatabase pDatabase = new EntityDatabase();
        private ISharedActionContext pActionContext;
        private readonly PreserveStack<IDatabaseAction> Actions = new PreserveStack<IDatabaseAction>();
        public ISharedActionContext ActionContext { set => pActionContext = value; }

        public DatabaseManager(ISharedActionContext actionContext = null)
        {
            pActionContext = actionContext;
        }

        public DatabaseManager(Stream stream, ISharedActionContext actionContext = null) : this(actionContext)
            => Load(stream);

        public DatabaseManager(string filename, ISharedActionContext actionContext = null) : this(actionContext)
            => LoadFromFile(filename);
        
        public IEnumerable<IEntityReference<Entity>> EnumerateEntities(int timestamp = -1)
            => pDatabase.EnumerateEntities(timestamp);

        public void WithDatabase(Action<EntityDatabase> action)
        {
            Actions.Clear();
            ActionsListChanged?.Invoke(this, EventArgs.Empty);
            action?.Invoke(pDatabase);
        }

        public void DoAction(IDatabaseAction action)
        {
            Debug.WriteLine($"[DM] DO {action.GetType().Name}");
            action.Do(pDatabase, pActionContext);
            Actions.Push(action);
            ActionsListChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UndoLastAction()
        {
            if (!CanUndo) return;            
            var action = Actions.Pop();
            ActionsListChanged?.Invoke(this, EventArgs.Empty);
            Debug.WriteLine($"[DM] UNDO {action.GetType().Name}");
            action.Undo(pDatabase, pActionContext);            
        }

        public void RedoLastAction()
        {
            if(!CanRedo) return;
            var action = Actions.UnPop();
            ActionsListChanged?.Invoke(this, EventArgs.Empty);
            Debug.WriteLine($"[DM] REDO {action.GetType().Name}");
            action.Do(pDatabase, pActionContext);
        }

        public event EventHandler ActionsListChanged;

        public bool CanUndo => Actions.CanPop;
        public bool CanRedo => Actions.CanUnPop;

        #region IO

        public void Load(Stream stream)
        {
            Actions.Clear();
            using(var br=new BinaryReader(stream))
                pDatabase = BinarySerializer.Deserialize<EntityDatabase>(br);
        }

        public void LoadFromFile(string filename)
        {
            using (var f = File.OpenRead(filename))
                Load(f);
        }

        public void Save(Stream stream)
        {
            using (var bw = new BinaryWriter(stream))
                BinarySerializer.Serialize(bw, pDatabase);
        }

        public void SaveToFile(string filename)
        {
            using (var f = File.Create(filename))
                Save(f);
        }
        #endregion IO
    }
}
