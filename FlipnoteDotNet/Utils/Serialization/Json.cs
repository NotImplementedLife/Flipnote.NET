using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace FlipnoteDotNet.Utils.Serialization
{
    public class Json
    {
        private JsonSerializerOptions SerializationOptions = new JsonSerializerOptions();
        public JavaScriptEncoder Encoder { get => SerializationOptions.Encoder; set => SerializationOptions.Encoder = value; }                
        public bool Indented { get => SerializationOptions.WriteIndented; set => SerializationOptions.WriteIndented = value; }

        public Json()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            Indented = true;
        }

        public object Deserialize(Type type, string json)
        {
            var method = typeof(Json).GetMethods().Where(_ => _.Name == "Deserialize" && _.GetParameters().Length == 1)
                .First().MakeGenericMethod(type);
            return method.Invoke(null, new object[] { json });
        }

        public T Deserialize<T>(string json)
        {            
            return JsonSerializer.Deserialize<T>(json);
        }

        public string Serialize<T>(T item)
        {            
            return JsonSerializer.Serialize(item, SerializationOptions);
        }

        public string DynamicSerialize(object item)
        {
            return JsonSerializer.Serialize(item, item.GetType(), SerializationOptions);
        }
    }
}
