using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Data.Manager;
using System;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;

namespace FlipnoteDotNet.Model.Actions
{
    internal class SelectedEntityPropertyChangedAction : DatabaseAction<FlipnoteSharedActionContext>
    {
        private readonly PropertyInfo Property;
        private object OldValue = null;
        private readonly object NewValue;
        private readonly Action Callback;

        public SelectedEntityPropertyChangedAction(PropertyInfo property, object newValue, Action callback)
        {
            Property = property;            
            NewValue = newValue;
            Callback = callback;
        }

        public override void Do(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {
            OldValue = Property.GetValue(ctx.SelectedEntity.Entity);
            Property.SetValue(ctx.SelectedEntity.Entity, NewValue);
            ctx.SelectedEntity.Commit();
            Callback?.Invoke();
        }

        public override void Undo(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {
            Property.SetValue(ctx.SelectedEntity.Entity, OldValue);
            ctx.SelectedEntity.Commit();
            Callback?.Invoke();
        }
    }
}
