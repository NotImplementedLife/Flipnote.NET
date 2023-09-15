using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace FlipnoteDotNet.Attributes
{
    internal class AttributesManager<A, T> : IEnumerable<AttributesManager<A, T>.TypeRecord> where A : Attribute
    {
        public TypeRecord[] TypeRecords { get; }
        public AttributesManager()
        {
            TypeRecords = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .Where(t => 
                            ((typeof(T).IsInterface && t.GetInterfaces().Contains(typeof(T)))
                            || t.IsSubclassOf(typeof(T))) && t.GetCustomAttribute(typeof(A)) != null)
                       .Select(t => new TypeRecord { Type = t })
                       .ToArray();
        }

        public class TypeRecord
        {
            public Type Type { get; set; }
            public A Attribute => Type.GetCustomAttribute<A>();
        }

        public IEnumerable<TypeRecord> WhereAttribute(Func<A, bool> predicate) => this.Where(r => predicate(r.Attribute));

        public IEnumerator<TypeRecord> GetEnumerator() => ((IEnumerable<TypeRecord>)TypeRecords).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
