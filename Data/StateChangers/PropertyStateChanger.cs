using System;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;

namespace FlipnoteDotNet.Data.StateChangers
{
    internal abstract class PropertyStateChanger : IStateChanger
    {
        protected PropertyStateChanger(PropertyInfo property)
        {
            Property = property;
        }

        public PropertyInfo Property { get; }

        protected abstract object GetPropertyNewValue(ICloneable item);

        public ICloneable ChangeState(ICloneable item)
        {
            var clone = item.Clone();
            var newValue = GetPropertyNewValue(clone);
            Property.SetValue(item, newValue);
            return clone;
        }
    }
}
