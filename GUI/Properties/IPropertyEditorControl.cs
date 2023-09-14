using System;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties
{
    public interface IPropertyEditorControl
    {
        object ObjectPropertyValue { get; set; }

        event EventHandler ObjectPropertyValueChanged;
        Panel KeyframesPanel { get; set; }
        bool IsTimeDependent { get; set; }
        PropertyInfo Property { get; set; }
    }
}
