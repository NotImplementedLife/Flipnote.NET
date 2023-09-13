using System;

namespace FlipnoteDotNet.Attributes
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Property)]
    internal class PropertyEditorControlAttribute:Attribute
    {
        public Type Type { get; set; }

        public PropertyEditorControlAttribute(Type type)
        {
            Type = type;
        }
    }
}
