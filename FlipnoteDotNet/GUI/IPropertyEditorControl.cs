using System;
using System.Reflection;

namespace FlipnoteDotNet.GUI
{
    public interface IPropertyEditorControl
    {
        object ObjectPropertyValue { get; set; }

        event EventHandler ObjectPropertyValueChanged;
        PropertyInfo Property { get; set; }
    }   
}
