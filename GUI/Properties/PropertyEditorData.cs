using System;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties
{
    public class PropertyEditorData
    {
        public PropertyInfo Property { get; }
        public string Label { get; }
        public Control Control { get; }
        public bool IsTimeDependent { get; }

        public int Y { get; set; }

        public PropertyEditorData(PropertyInfo prop, string label, Control control, bool isTimeDependent)
        {
            Property = prop;
            Label = label;
            Control = control;
            IsTimeDependent = isTimeDependent;
        }

        public event EventHandler KeyframesButtonClick;
        public event EventHandler EffectsButtonClick;

        public void TriggerKeyframesButtonClick() => KeyframesButtonClick?.Invoke(this, new EventArgs());
        public void TriggerEffectsButtonClick() => EffectsButtonClick?.Invoke(this, new EventArgs());
    }
}
