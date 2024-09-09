using FlipnoteDotNet.App.Storage;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace FlipnoteDotNet.Utils
{
    public class Serializer
    {
        private readonly Dictionary<object, int> InstanceRefs = new();

        private readonly XmlSerializerNamespaces NoNS = new XmlSerializerNamespaces(new[]
        {
            new XmlQualifiedName("","")
        });

        public void Serialize<T>(Stream stream, T obj)
        {
            var xml = ToXml(obj);
            Debug.WriteLine(xml);

            using var archive = new ZipArchive(stream, ZipArchiveMode.Create, true);
            archive.WriteEntry("obj.xml", xml);
            BytesContainer.ForEach((k, v) =>
            {
                archive.WriteEntry($"ref/{k}", v);
            });
        }
        
        public T Deserialize<T>(Stream stream)
        {
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read, true);
            var xml = archive.ReadEntryString("obj.xml");
            Debug.WriteLine(xml);

            var refRoot = "ref/";
            foreach(var entry in archive.Entries)
            {
                if(entry.FullName.StartsWith(refRoot))
                {
                    var key = entry.FullName.Substring(refRoot.Length);
                    var bytes = entry.ReadBytes();
                    BytesContainer.Put(key, bytes);
                }                
            }
            var obj = FromXml<T>(xml);
            return obj;
        }

        private string ToXml<T>(T obj)
        {
            var xmlSer = new XmlSerializer(typeof(T));
            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xmlSer.Serialize(writer, obj, NoNS);
                    return sww.ToString();
                }
            }
        }

        private T FromXml<T>(string xml)
        {
            var xmlSer = new XmlSerializer(typeof(T));
            using (var sr = new StringReader(xml))
                return (T)xmlSer.Deserialize(sr); 
        }

        public void Scan(object obj)
        {
            if (obj == null) return;
            if (InstanceRefs.ContainsKey(obj))
                return;
            var type = obj.GetType();
            if(type.IsClass)
            {
                InstanceRefs[obj] = InstanceRefs.Count + 1;
            }

            if (type.IsClass || (type.IsValueType && !type.IsEnum && !type.IsPrimitive)) 
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                foreach (var field in fields) 
                {
                    var fieldType = field.FieldType;
                    if (fieldType.IsArray)
                    {
                        foreach (var item in field.GetValue(obj) as Array)
                        {
                            Scan(item);
                        }
                        continue;
                    }
                    if (fieldType.IsClass)
                    {
                        Scan(field.GetValue(obj));
                    }
                }
            }            
        }

        public void DumpRefs()
        {
            foreach(var (k,v) in InstanceRefs)
            {
                Debug.WriteLine($"{v}. {k}");
            }
        }

    }
}
