using FlipnoteDotNet.App.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.App.Service.FlipnoteEditorActions
{
    public class ReflectionPropertySet : IUndoableAction
    {
        private readonly object Target;
        private readonly object OldValue;
        private readonly object NewValue;
        private readonly MethodInfo Setter;
        public ReflectionPropertySet(object target, object oldValue, object newValue, MethodInfo setter)
        {
            Target = target;
            OldValue = oldValue;
            NewValue = newValue;
            Setter = setter;
        }

        public void Do()
        {
            Debug.WriteLine($"Reflection use on {Target.GetType()}::{Setter}");
            Setter.Invoke(Target, new[] { NewValue });
        }

        public void Undo()
        {
            Setter.Invoke(Target, new[] { OldValue });
        }
    }
}
