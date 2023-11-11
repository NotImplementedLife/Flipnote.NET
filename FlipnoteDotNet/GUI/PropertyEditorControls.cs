using FlipnoteDotNet.Commons.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI
{
    public static class PropertyEditorControls
    {
        private static Dictionary<Type, Type> FieldTypes;

        static PropertyEditorControls()
        {
            FieldTypes = AssemblyScanner.EnumerateTypesHavingAttribute<PropertyEditorControlAttribute>()                
                .ToDictionary(t => t.GetCustomAttribute<PropertyEditorControlAttribute>().TargetType, t => t);
            foreach (var kv in FieldTypes)
            {
                if (!kv.Value.GetInterfaces().Contains(typeof(IPropertyEditorControl)))
                    throw new InvalidOperationException($"Field type {kv.Value} does not implement IPropertyEditorControl interface");

                if (!kv.Value.IsSubclassOf(typeof(Control)))
                    throw new InvalidOperationException($"Field type {kv.Value} is not a Control");
            }                
        }

        public static Control CreateField(Type targetType)
        {
            if (!FieldTypes.TryGetValue(targetType, out Type fieldType))
                return null;
            return Activator.CreateInstance(fieldType) as Control;
        }
    }

    public class FieldEditorAttribute : Attribute
    {
        public Type FieldType { get; }

        public FieldEditorAttribute(Type fieldType)
        {
            FieldType = fieldType;
        }
    }


    public class PropertyEditorControlAttribute : Attribute
    {
        public Type TargetType { get; }
        public PropertyEditorControlAttribute(Type targetType)
        {
            TargetType = targetType;
        }
    }
}
