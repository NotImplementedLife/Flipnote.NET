using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FlipnoteDotNet.Utils.Serialization
{
    public class SerializeWriter : MemoryStream
    {
        public BinaryWriter BinaryWriter { get; }
        public SerializeWriter(): base()
        {
            BinaryWriter = new BinaryWriter(this);
        }

        public void Write<T>(T item)
        {
            if(typeof(T).GetInterfaces().Contains(typeof(ISerializable)))
            {
                if (item == null)
                    BinaryWriter.Write((short)0);
                else
                {
                    BinaryWriter.Write(SerializationContext.GetIdFromType(typeof(T)));
                    (item as ISerializable).Serialize(this);
                }
                return;
            }
            var bwMethod = GetBinaryWriterMethod(typeof(T));
            if (bwMethod != null)
                bwMethod.Invoke(BinaryWriter, new object[] { item });

            throw new InvalidOperationException($"Cannot serialize object of type {item}");
        }

        private static MethodInfo GetBinaryWriterMethod(Type targetType)
            => typeof(BinaryWriter).GetMethods()
                .Where(m => m.Name == "Write" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == targetType)
                .FirstOrDefault();
    }
}
