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
        private PropertiesCollection(Type type, string[] ignoredProperties = null)
        {
            ignoredProperties ??= new string[0];
            TargetType = type;
            Properties = (
                from pi in TargetType.GetProperties()
                where pi.CanRead && pi.GetGetMethod().IsPublic && (pi.GetSetMethod()?.IsPublic ?? true)
                where !ignoredProperties.Contains(pi.Name)
                orderby pi.Name
                select new PropertyData(pi)
                ).ToArray();
            Length = Properties.Length;
            Debug.WriteLine(string.Join("\n", Properties));
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


            public PropertyData(PropertyInfo propertyInfo)
            {                
                Getter = propertyInfo.GetGetMethod();
                Setter = propertyInfo.GetSetMethod();
                Name = propertyInfo.Name;
                PropertyType = propertyInfo.PropertyType;
            }

            public override string ToString() => $"{PropertyType.Name} {Name} {{ " +
                (Getter != null ? "get; " : "") +
                (Setter != null ? "set; " : "") +
                $"}}";
        }

        private static readonly Dictionary<Type, PropertiesCollection> CachedCollections
            = new Dictionary<Type, PropertiesCollection>();

        public static PropertiesCollection FromType(Type t)
        {
            return CachedCollections.TryGetValue(t, out var col)
                ? col
                : (CachedCollections[t] = new PropertiesCollection(t));
        }

        public static void RegisterType<T>()
        {
            CachedCollections[typeof(T)] = new PropertiesCollection(typeof(T));
        }        
    }
}
