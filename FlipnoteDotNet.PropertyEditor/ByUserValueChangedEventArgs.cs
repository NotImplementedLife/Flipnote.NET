using static FlipnoteDotNet.PropertyEditor.PropertiesCollection;

namespace FlipnoteDotNet.PropertyEditor
{
    public class ByUserValueChangedEventArgs
    {
        public readonly bool IsPreview;
        public readonly object OldValue;
        public readonly object NewValue;
        public readonly PropertyData? PropertyData = null;

        public ByUserValueChangedEventArgs(bool isPreview, object oldValue, object newValue)
        {
            IsPreview = isPreview;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public ByUserValueChangedEventArgs(ByUserValueChangedEventArgs e, PropertyData propertyData)
            : this(e.IsPreview, e.OldValue, e.NewValue)
        {
            PropertyData = propertyData;
        }

    }
}
