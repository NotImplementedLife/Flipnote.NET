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

        public static IEnumerable<MethodInfo> GetStaticVoidMethods(this Type type, string name, params Type[] paramTypes)
            => GetStaticMethods(type, name, typeof(void), paramTypes);

        public static IEnumerable<MethodInfo> GetStaticMethods(this Type type, string name, Type returnType, params Type[] paramTypes)
        {
            return from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                   where (method.Name) == name && method.ReturnParameter.ParameterType == returnType
                   let pms = (from p in method.GetParameters() select p.ParameterType).ToArray()
                   where pms.Length == paramTypes.Length
                   where paramTypes.Zip(pms, (t, p) => t.IsInterface ? p.GetInterfaces().Contains(t) : t == p).All(_ => _)
                   select method;
        }       
    }
}
