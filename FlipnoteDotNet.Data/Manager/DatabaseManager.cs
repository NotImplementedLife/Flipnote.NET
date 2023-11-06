using FlipnoteDotNet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace FlipnoteDotNet.Data.Manager
{
    public class DatabaseManager
    {
        private EntityDatabase pDatabase = new EntityDatabase();
        private ISharedActionContext pActionContext;
        private readonly PreserveStack<IDatabaseAction> Actions = new PreserveStack<IDatabaseAction>();

        public ISharedActionContext ActionContext { set => pActionContext = value; } 

        public IEnumerable<IEntityReference<Entity>> EnumerateEntities(int timestamp = -1)
            => pDatabase.EnumerateEntities(timestamp);

        public DatabaseManager(ISharedActionContext actionContext = null)
        {
            pActionContext = actionContext;
        }

        public void WithDatabase(Action<EntityDatabase> action)
        {
            Actions.Clear();
            action?.Invoke(pDatabase);
        }

        public void DoAction(IDatabaseAction action)
        {
            Debug.WriteLine($"[DM] DO {action.GetType().Name}");
            action.Do(pDatabase, pActionContext);
            Actions.Push(action);
        }

        public void UndoLastAction()
        {
            if (!CanUndo) return;            
            var action = Actions.Pop();
            Debug.WriteLine($"[DM] UNDO {action.GetType().Name}");
            action.Undo(pDatabase, pActionContext);            
        }

        public void RedoLastAction()
        {
            if(!CanRedo) return;
            var action = Actions.UnPop();
            Debug.WriteLine($"[DM] REDO {action.GetType().Name}");
            action.Do(pDatabase, pActionContext);
        }

        public bool CanUndo => Actions.CanPop;
        public bool CanRedo => Actions.CanUnPop;

        public void Load(Stream stream)
        {
            using(var br=new BinaryReader(stream))
                pDatabase = BinarySerializer.Deserialize<EntityDatabase>(br);
        }

        public void Save(Stream stream)
        {
            using (var bw = new BinaryWriter(stream))
                BinarySerializer.Serialize(bw, pDatabase);
        }

        public static DatabaseManager LoadFromFile(string filename)
        {
            var mng = new DatabaseManager();
            using (var f = File.OpenRead(filename)) 
                mng.Load(f);
            return mng;
        }

        public void SaveToFile(string filename)
        {
            using (var f = File.Create(filename))
                Save(f);
        }
    }
}
