using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Commons.Reflection
{
    public static class AssemblyScanner
    {
        private readonly static Type[] Types;
        private readonly static Type[] FlipnoteDotNetTypes;

        private static readonly Dictionary<string, Type> TypesByName;

        static AssemblyScanner()
        {            
            Types = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .GroupBy(_ => _.FullName).Select(t => t.First())
                       .ToArray();
            FlipnoteDotNetTypes = Types.Where(_ => _.FullName.StartsWith(nameof(FlipnoteDotNet))).ToArray();
            
            TypesByName = Types.ToDictionary(_ => _.FullName, _ => _);
        }

        public static IEnumerable<Type> EnumerateTypes() => Types;
        public static IEnumerable<Type> EnumerateTypesHavingAttribute(Type attrType) => Types.Where(_ => _.GetCustomAttribute(attrType) != null);
        public static IEnumerable<Type> EnumerateTypesHavingAttribute<A>() where A : Attribute => Types.Where(_ => _.GetCustomAttribute<A>() != null);

        public static Type GetTypeByFullName(string fullName) => TypesByName.TryGetValue(fullName, out var type) ? type : null;

    }
}
