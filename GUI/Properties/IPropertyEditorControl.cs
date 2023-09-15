using System;
using System.Reflection;

namespace FlipnoteDotNet.GUI.Properties
{
    public interface IPropertyEditorControl
    {
        object ObjectPropertyValue { get; set; }

        event EventHandler ObjectPropertyValueChanged;
        KeyFramesEditor KeyframesEditor { get; set; }
        bool IsTimeDependent { get; set; }
        PropertyInfo Property { get; set; }
    }
}
