using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.PropertyEditor
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyEditorAttribute : Attribute
    {
        public readonly Type EditorType;

        public readonly string Name;
        public readonly bool Ignore;

        public PropertyEditorAttribute(Type editorType = null, string name = null, bool ignore = false)
        {
            EditorType = editorType;
            Name = name;
            Ignore = ignore;
        }
    }
}
