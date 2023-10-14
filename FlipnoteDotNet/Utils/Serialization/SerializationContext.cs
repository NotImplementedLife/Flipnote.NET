using System;
using System.Collections.Generic;
using System.Linq;

namespace FlipnoteDotNet.Utils.Serialization
{
    internal static class SerializationContext
    {
        private static Dictionary<Type, short> TypeIds;
        private static Dictionary<short, Type> IdTypes;

        public static void Initialize()
        {
            return;
            var tmp = Constants.Reflection.GetTypesFromAssembly()
                .Where(t => t.GetInterfaces().Contains(typeof(ISerializable)))
                .Select((t, i) => (t, i: (short)(i + 1)))
                .ToArray();
            TypeIds = tmp.ToDictionary(p => p.t, p => p.i);
            IdTypes = tmp.ToDictionary(p => p.i, p => p.t);
        }

        public static void WriteType(Type t)
        {
            if(t.IsGenericType)
            {
                var typeId = GetIdFromType(t.GetGenericTypeDefinition());

            }
        }

        public static short GetIdFromType(Type t) => TypeIds.TryGetValue(t, out short id)
            ? id : throw new ArgumentException($"Type {t} is not registered as ISerializable");

        public static Type GetTypeFromId(short id) => IdTypes.TryGetValue(id, out Type t)
            ? t : throw new ArgumentException();

    }
}
