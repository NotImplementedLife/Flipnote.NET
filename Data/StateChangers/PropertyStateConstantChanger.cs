using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Data.StateChangers
{
    internal class PropertyStateConstantChanger : PropertyStateChanger
    {
        public object PropertyValue { get; }
        public PropertyStateConstantChanger(PropertyInfo property, object propertyValue) : base(property)
        {
            PropertyValue = propertyValue;
        }
        protected override object GetPropertyNewValue(ICloneable item) => PropertyValue;
    }
}
