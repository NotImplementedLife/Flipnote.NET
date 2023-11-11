using FlipnoteDotNet.Data.Entities;

namespace FlipnoteDotNet.Data.Manager
{
    public interface IDatabaseAction
    {
        void Do(EntityDatabase db, ISharedActionContext ctx);
        void Undo(EntityDatabase db, ISharedActionContext ctx);
    }

    public abstract class DatabaseAction<C> : IDatabaseAction where C : class, ISharedActionContext
    {
        public abstract void Do(EntityDatabase db, C ctx);
        public abstract void Undo(EntityDatabase db, C ctx);

        void IDatabaseAction.Do(EntityDatabase db, ISharedActionContext ctx) => Do(db, ctx as C);
        void IDatabaseAction.Undo(EntityDatabase db, ISharedActionContext ctx) => Undo(db, ctx as C);
    }
}
