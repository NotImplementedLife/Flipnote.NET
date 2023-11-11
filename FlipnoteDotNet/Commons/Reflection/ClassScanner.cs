using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace FlipnoteDotNet.Commons.Reflection
{
    public static class ClassScanner
    {        
        private static bool TypeArrayEquals(Type[] t1, Type[] t2)
        {
            if (t1.Length != t2.Length) return false;
            for (int i = 0; i < t1.Length; i++)
                if (t1[i] != t2[i])
                    return false;
            return true;
        }        


        public static IEnumerable<MethodInfo> GetMethods(Type type, Type[] argTypes, Type returnType, bool nonPublic = false, bool @static = false)
        {
            var flags = BindingFlags.Public;
            if (nonPublic) flags |= BindingFlags.NonPublic;
            if (@static) flags |= BindingFlags.Static;
            else flags |= BindingFlags.Instance;

            return from method in type.GetMethods(flags)
                   where method.ReturnType == returnType
                   let args = method.GetParameters().Select(p => p.ParameterType).ToArray()
                   where TypeArrayEquals(args, argTypes)
                   select method;
        }

        public static IEnumerable<PropertyInfo> GetProperties(Type type, bool nonPublic = false, bool @static = false)
        {
            var flags = BindingFlags.Public;
            if (nonPublic) flags |= BindingFlags.NonPublic;
            if (@static) flags |= BindingFlags.Static;
            else flags |= BindingFlags.Instance;

            return type.GetProperties(flags);
        }

        public static IEnumerable<FieldInfo> GetFields(Type type, bool nonPublic = false, bool @static = false)
        {
            var flags = BindingFlags.Public;
            if (nonPublic) flags |= BindingFlags.NonPublic;
            if (@static) flags |= BindingFlags.Static;
            else flags |= BindingFlags.Instance;

            return type.GetFields(flags);
        }
    }
}
