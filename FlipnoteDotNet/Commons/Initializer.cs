using FlipnoteDotNet.Commons.Reflection;
using System;
using System.Collections.Generic;

namespace FlipnoteDotNet.Commons
{
    public static class InitializerService
    {                
        private static List<Type> PendingTypes = new List<Type>();

        public static void Run()
        {
            Register(typeof(AssemblyScanner));
            PendingTypes.ForEach(Run);
            PendingTypes.Clear();
        }

        public static void Register(params Type[] type) => PendingTypes.AddRange(type);

        private static void Run(Type type)
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);
        }
    }
}
