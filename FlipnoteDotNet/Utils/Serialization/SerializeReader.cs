using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace FlipnoteDotNet.Utils.Serialization
{
    public class SerializeReader : MemoryStream
    {
        public BinaryReader BinaryReader { get; }

        public SerializeReader(byte[] bytes) : base(bytes)
        {
            BinaryReader = new BinaryReader(this);
        }

        public T Read<T>(object target = null)
        {
            if (typeof(T).GetInterfaces().Contains(typeof(ISerializable)))
            {
                var typeId = BinaryReader.ReadInt16();
                if (typeId == 0)
                    return default(T);
                var type = SerializationContext.GetTypeFromId(typeId);

                ISerializable result = null;

                if (target != null)
                {
                    if (!typeof(T).IsAssignableFrom(target.GetType()))
                        throw new InvalidCastException();
                    result = target as ISerializable;
                }
                else
                    result = FormatterServices.GetUninitializedObject(type) as ISerializable;

                result.Deserialize(this, result, target != null);

                if (!typeof(T).IsAssignableFrom(result.GetType()))
                    throw new SerializationException($"Deserialization failed: expected {typeof(T)}, got {result.GetType()}");
                return (T)result;
            }
            var bwMethod = GetBinaryReaderMethod(typeof(T));
            if (bwMethod != null)
                return (T)bwMethod.Invoke(BinaryReader, new object[0]);
            throw new InvalidOperationException($"Cannot deserialize object of type {typeof(T)} from provided data");
        }

        private static MethodInfo GetBinaryReaderMethod(Type targetType)
            => typeof(BinaryReader).GetMethods()
                .Where(m => m.Name.StartsWith("Read") && m.GetParameters().Length == 0 && m.ReturnParameter.ParameterType == targetType)
                .FirstOrDefault();

        public void CallConstructor<T>(object obj, params object[] pms)
        {
            if (!(obj is T))
                throw new ArgumentException("Invalid type");
            var constructor = (from c in typeof(T).GetConstructors()
                               let cpms = c.GetParameters().Select(_ => _.ParameterType).ToArray()
                               where cpms.Length == pms.Length
                                  && cpms.Zip(pms, (c, p) => p == null ? !c.IsValueType : c.IsAssignableFrom(p.GetType())).All(_ => _)
                               select c).FirstOrDefault();
            if (constructor == null)
                throw new InvalidOperationException("Could not find suitable constructor");
            constructor.Invoke(obj, pms);
        }
    }
}