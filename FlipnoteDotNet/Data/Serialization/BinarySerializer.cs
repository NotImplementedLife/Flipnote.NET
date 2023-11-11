using FlipnoteDotNet.Commons.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FlipnoteDotNet.Data
{    
    public static class BinarySerializer
    {
        private class RW
        {
            public MethodInfo Read { get; set; }
            public MethodInfo Write { get; set; }
        }


        private static Dictionary<Type, RW> Serializers = new Dictionary<Type, RW>();

        static BinarySerializer()
        {
            var providers = AssemblyScanner.EnumerateTypesHavingAttribute<BinarySerializerProviderAttribute>();

            foreach(var provider in providers)
            {
                foreach (var method in provider.GetMethods(BindingFlags.Static | BindingFlags.Public)) 
                {
                    var pms = method.GetParameters();
                    if (pms.Length == 0)
                        continue;
                    if (pms[0].ParameterType == typeof(BinaryReader) && pms.Length == 1) 
                    {
                        var t = method.ReturnType;
                        if (t == typeof(void)) continue;

                        if (!Serializers.ContainsKey(t))
                            Serializers[t] = new RW();

                        if (Serializers[t].Read != null)
                            throw new InvalidOperationException($"Overload read serialization for {t}");
                        Serializers[t].Read = method;
                    }
                    else if (pms[0].ParameterType==typeof(BinaryWriter) && pms.Length==2 && method.ReturnType==typeof(void))
                    {
                        var t = pms[1].ParameterType;
                        if (!Serializers.ContainsKey(t))
                            Serializers[t] = new RW();
                        if (Serializers[t].Write != null)
                            throw new InvalidOperationException($"Overload write serialization for {t}");
                        Serializers[t].Write = method;
                    }
                }
                
                foreach(var entry in Serializers)
                {
                    var t = entry.Key;
                    var rw = entry.Value;
                    if (rw.Read == null)
                        throw new InvalidOperationException($"No read method specified for {t}, but write method provided");
                    if (rw.Write == null)
                        throw new InvalidOperationException($"No write method specified for {t}, but read method provided");
                }

            }
        }

        private static void WriteTypeHeader(BinaryWriter bw, object obj)
        {
            if(obj==null)
            {
                bw.Write((byte)0);
                return;
            }
            bw.Write((byte)1);
            bw.Write(obj.GetType().FullName);
        }

        private static Type ReadTypeHeader(BinaryReader br)
        {
            var nullIndex = br.ReadByte();
            if (nullIndex == 0) return null;
            var typeName = br.ReadString();
            var type = AssemblyScanner.GetTypeByFullName(typeName);
            if (type == null) throw new InvalidOperationException($"No type named '{typeName}'");
            return type;
        }        

        public static void Serialize(BinaryWriter bw, object item)
        {
            WriteTypeHeader(bw, item);
            if (item == null) return;
            var type = item.GetType();
            if (TryWriteWithCustomSerializers(bw, type, item)) return;
            if (TryWriteEnum(bw, type, item)) return;

            CallBinaryWriter(bw, item, type);
        }

        public static object Deserialize(BinaryReader br)
        {
            var type = ReadTypeHeader(br);
            if (type == null) return null;

            if (TryReadWithCustomSerializers(br, type, out var result))
                return result;

            if (TryReadEnum(br, type, out var enumResult))
                return enumResult;

            return CallBinaryReader(br, type);
        }

        public static void Serialize<T>(BinaryWriter bw, T item) => Serialize(bw, item as object);
        public static T Deserialize<T>(BinaryReader br) => (T)Deserialize(br);        

        #region Helper methods

        private static void CallBinaryWriter(BinaryWriter bw, object item, Type type)
        {
            var write = (from method in typeof(BinaryWriter).GetMethods()
                         where method.Name == nameof(BinaryWriter.Write)
                         let pms = method.GetParameters()
                         where pms.Length == 1 && pms[0].ParameterType == type
                         select method).FirstOrDefault();
            if(write==null)
                throw new InvalidOperationException($"Cannot serialize item of type {type}");            
            write.Invoke(bw, new object[] { item });            
        }

        private static object CallBinaryReader(BinaryReader br, Type type)
        {
            var read = (from method in typeof(BinaryReader).GetMethods()
                        where method.Name.StartsWith("Read") && method.Name != nameof(BinaryReader.Read)
                        where method.GetParameters().Length == 0 && method.ReturnParameter.ParameterType == type
                        select method).FirstOrDefault();
            var result = read?.Invoke(br, Array.Empty<object>())
                ?? throw new InvalidOperationException($"Cannot deserialize item of type {type}");
            return result;
        }

        private static bool TryWriteWithCustomSerializers(BinaryWriter bw, Type type, object item)
        {
            if (!Serializers.TryGetValue(type, out var rw)) return false;
            rw.Write.Invoke(null, new object[] { bw, item });
            return true;
        }

        private static bool TryReadWithCustomSerializers(BinaryReader br, Type type, out object item)
        {
            item = null;
            if (!Serializers.TryGetValue(type, out var rw)) return false;
            item = rw.Read.Invoke(null, new object[] { br });
            return true;
        }

        private static bool TryWriteEnum(BinaryWriter bw, Type enumType, object value)
        {
            if (!enumType.IsEnum) return false;
            bw.Write(value.ToString());
            return true;
        }

        private static bool TryReadEnum(BinaryReader br, Type enumType, out Enum value)
        {
            value = null;
            if (!enumType.IsEnum) return false;

            var tryParse = (from m in typeof(Enum).GetMethods(BindingFlags.Static | BindingFlags.Public)
                            where m.Name == nameof(Enum.TryParse) && m.GetParameters().Length == 2
                            select m).First().MakeGenericMethod(enumType);            
            object[] parameters = new object[] { br.ReadString(), null };
            object result = tryParse.Invoke(null, parameters);
            bool blResult = (bool)result;
            if (!blResult) return false;
            value = (Enum)parameters[1];
            return true;
        }
            

        #endregion
    }
}
