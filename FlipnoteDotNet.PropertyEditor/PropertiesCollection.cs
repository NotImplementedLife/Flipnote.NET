using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FlipnoteDotNet.PropertyEditor
{
    public class PropertiesCollection
    {
        public readonly Type TargetType;
        public readonly PropertyData[] Properties;
        public readonly int Length;
        
        private PropertiesCollection(Type type, string[] ignoredProperties = null, Dictionary<PropertyInfo, PropertyEditorAttribute> propAttr = null)
        {           
            ignoredProperties ??= new string[0];
            TargetType = type;
            Properties = (
                from pi in TargetType.GetProperties()
                where pi.CanRead && pi.GetGetMethod().IsPublic && (pi.GetSetMethod()?.IsPublic ?? true)
                where !ignoredProperties.Contains(pi.Name)
                let attr = GetAttribute(propAttr, pi)
                where !attr?.Ignore ?? true
                orderby attr?.Name ?? pi.Name
                select new PropertyData(pi, attr)
                ).ToArray();
            Length = Properties.Length;
            Debug.WriteLine(string.Join("\n", Properties));
        }

        private static PropertyEditorAttribute GetAttribute(Dictionary<PropertyInfo, PropertyEditorAttribute> propAttr, PropertyInfo pi)
        {
            // class A { public int P{get;set;} }
            // class B : A {}
            // typeof(B).GetProperty("P") != typeof(A).GetProperty("P") ?

            if (propAttr != null)
            {
                foreach (var (p, a) in propAttr)
                {
                    if (p.DeclaringType == pi.DeclaringType && p.Name == pi.Name)
                        return a;
                }
            }
            return pi.GetCustomAttribute<PropertyEditorAttribute>();
        }

        public PropertyData this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Properties[index];
        }

        public readonly struct PropertyData
        {            
            public readonly MethodInfo Getter;
            public readonly MethodInfo Setter;
            public readonly string Name;
            public readonly Type PropertyType;
            public readonly Type PrefferedEditor;

            public PropertyData(PropertyInfo propertyInfo, PropertyEditorAttribute attr = null)
            {                
                Getter = propertyInfo.GetGetMethod();
                Setter = propertyInfo.GetSetMethod();
                Name = attr?.Name ?? propertyInfo.Name;
                PropertyType = propertyInfo.PropertyType;
                PrefferedEditor = attr?.EditorType;
            }

            public override string ToString() => $"{PropertyType.Name} {Name} {{ " +
                (Getter != null ? "get; " : "") +
                (Setter != null ? "set; " : "") +
                $"}}";
        }

        private static readonly Dictionary<Type, PropertiesCollection> CachedCollections
            = new Dictionary<Type, PropertiesCollection>();

        private static readonly Dictionary<PropertyInfo, PropertyEditorAttribute> RuntimePropertyAttributes
            = new Dictionary<PropertyInfo, PropertyEditorAttribute>();

        public static PropertiesCollection FromType(Type t)
        {
            return CachedCollections.TryGetValue(t, out var col)
                ? col
                : (CachedCollections[t] = new PropertiesCollection(t, null, RuntimePropertyAttributes));
        }

        public static void RegisterType<T>()
        {
            CachedCollections[typeof(T)] = new PropertiesCollection(typeof(T), null, RuntimePropertyAttributes);
        }        

        public static void RegisterAttribute<T>(string propertyName, PropertyEditorAttribute attr)
        {
            RuntimePropertyAttributes[typeof(T).GetProperty(propertyName)] = attr;
        }

    }
}
