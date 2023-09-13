using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FlipnoteDotNet.Extensions
{
    internal static class ReflectExtensions
    {
        public static IEnumerable<PropertyInfo> GetAllPublicProperties(this Type type)
        {            
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance).AsEnumerable();
            /*
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                yield return property;

            var baseType = type.BaseType;
            if (baseType == null)
                yield break;

            foreach (var property in GetAllPublicProperties(baseType))
                yield return property;*/
        }

        public static bool IsGenericConstruct(this Type type, Type genericTypeDef)
            => type.IsGenericType && type.GetGenericTypeDefinition() == genericTypeDef;
    }
}
