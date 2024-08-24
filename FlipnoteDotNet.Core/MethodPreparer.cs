using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Core
{
    public static class MethodPreparer
    {
        public static void WarmUp(Type type, string methodName)
        {
            var handle = type.GetMethod(methodName, MethodBindingFlags).MethodHandle;
            RuntimeHelpers.PrepareMethod(handle);
        }       

        private const BindingFlags MethodBindingFlags =
            BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.Static;
    }
}
