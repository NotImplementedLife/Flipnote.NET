using System;

namespace FlipnoteDotNet.GUI.Properties
{
    public interface IPropertyEditorControl
    {
        object ObjectPropertyValue { get; set; }

        event EventHandler ObjectPropertyValueChanged;
    }
}
