using FlipnoteDotNet.PropertyEditor.Editors;
using System.Runtime.CompilerServices;

namespace FlipnoteDotNet.PropertyEditor
{
    public static class PropertyEditors
    {
        private static readonly Dictionary<Type, Type> TypeEditors = new Dictionary<Type, Type>();

        public static void RegisterDefaultEditor<T, E>() where E : IEditor
        {
            TypeEditors[typeof(T)] = typeof(E);
        }

        public static IEditor CreateEditor(Type type, Type editorType = null)
        {
            editorType ??= TypeEditors.TryGetValue(type, out var ed) ? ed : null;
            if (editorType == null)
                return new Editors.Placeholder();
            if (!typeof(IEditor).IsAssignableFrom(editorType))
                throw new InvalidOperationException($"{editorType} is not an editor for {type} data.");
            return Activator.CreateInstance(editorType) as IEditor;
        }

        static PropertyEditors()
        {
            RegisterDefaultEditor<bool, Editors.CheckBox>();
            RegisterDefaultEditor<string, Editors.TextBox>();
            RegisterDefaultEditor<int, Editors.IntegerBox>();
        }
    }
}
