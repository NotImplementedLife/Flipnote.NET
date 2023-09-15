using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace FlipnoteDotNet.Extensions
{
    internal static class ReflectExtensions
    {
        static Dictionary<Type, PropertyInfo[]> PublicPropertiesCache = new Dictionary<Type, PropertyInfo[]>();


        public static IEnumerable<PropertyInfo> GetAllPublicProperties(this Type type)
        {
            if (PublicPropertiesCache.TryGetValue(type, out var result))
                return result;

            return (PublicPropertiesCache[type] = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .OrderBy(_ => _.Name)
                .Peek(_ => Debug.WriteLine(_.Name))
                .ToArray()).AsEnumerable();
        }

        public static bool IsGenericConstruct(this Type type, Type genericTypeDef)
            => type.IsGenericType && type.GetGenericTypeDefinition() == genericTypeDef;
    }
}
