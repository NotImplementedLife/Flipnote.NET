using System.Reflection;

namespace FlipnoteDotNet.Utils
{
    public static class BinSerializer
    {
        public static byte[] ToArray<T>(T obj)
        {
            using(var ms=new MemoryStream())
            {
                Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        private static Dictionary<Type, Action<BinaryWriter, object>> WriteObj = new()
        {
            [typeof(bool)] = (bw, obj) => { bw.Write((byte)SerType.Int8); bw.Write((byte)obj); },
            [typeof(byte)] = (bw, obj) => { bw.Write((byte)SerType.Int8); bw.Write((byte)obj); },
            [typeof(sbyte)] = (bw, obj) => { bw.Write((byte)SerType.Int8); bw.Write((sbyte)obj); },
            [typeof(ushort)] = (bw, obj) => { bw.Write((byte)SerType.Int16); bw.Write((ushort)obj); },
            [typeof(short)] = (bw, obj) => { bw.Write((byte)SerType.Int16); bw.Write((short)obj); },
            [typeof(uint)] = (bw, obj) => { bw.Write((byte)SerType.Int32); bw.Write((uint)obj); },
            [typeof(int)] = (bw, obj) => { bw.Write((byte)SerType.Int32); bw.Write((int)obj); },
            [typeof(ulong)] = (bw, obj) => { bw.Write((byte)SerType.Int64); bw.Write((ulong)obj); },
            [typeof(long)] = (bw, obj) => { bw.Write((byte)SerType.Int64); bw.Write((long)obj); },
            [typeof(float)] = (bw, obj) => { bw.Write((byte)SerType.Float32); bw.Write((float)obj); },
            [typeof(double)] = (bw, obj) => { bw.Write((byte)SerType.Float64); bw.Write((double)obj); },
            [typeof(string)] = (bw, obj) => { bw.Write((byte)SerType.String); bw.Write((string)obj); },
            [typeof(byte[])] = (bw, obj) => { bw.Write((byte)(SerType.Int8 | SerType.Sequence)); bw.Write((byte[])obj); }
        };        

        public static void Serialize<T>(Stream s, T obj)
        {
            using var bw = new BinaryWriter(s);                   
            if(WriteObj.TryGetValue(typeof(T), out var writer))
            {
                writer(bw, obj);
                return;
            }

            if (typeof(T).IsValueType && !(typeof(T).IsPrimitive || typeof(T).IsEnum)) 
            {
                var values = new Dictionary<string, object>();
                var fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);                
                foreach (var field in fields) 
                {
                    values[field.Name] = field.GetValue(obj);
                }

                bw.Write((byte)SerType.Class);
                bw.Write(typeof(T).FullName);
                bw.Write((ushort)values.Count);
                foreach(var (key, value) in values)
                {
                    bw.Write(key);
                    //Serialize()
                }

                // Node a { Child = b, Parent=null }
                // Node b { Parent = a, Child=null }

            }


            //if(typeof(T).GetCustomAttribute<>)


            throw new NotImplementedException($"Serialize {typeof(T)}");
        }


        private enum SerType : byte
        {
            Null = 0b00000000,
            Int = 0b00000001,
            Float = 0b00000010,
            String = 0b00000011,
            Class = 0b00000100,

            Sequence = 0b00100000,

            Num8 = 0b00000000,
            Num16 = 0b01000000,
            Num32 = 0b10000000,
            Num64 = 0b11000000,

            Int8 = Int | Num8,
            Int16 = Int | Num16,
            Int32 = Int | Num32,
            Int64 = Int | Num64,

            Float32 = Float | Num32,
            Float64 = Float | Num64
        }

    }
}
