using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FlipnoteDotNet.Data.Entities
{
    public interface IEntityList
    {
        int Count { get; }
        IEntityReference<Entity> GetEntity(int index);
        void SetEntity(int index, IEntityReference<Entity> value);
        void AddEntity(IEntityReference<Entity> e);
        IEnumerable<IEntityReference<Entity>> AsEnumerable();
    }

    public class EntityList<E> : IList<IEntityReference<E>>, IEntityList where E:Entity
    {        
        private readonly List<IEntityReference<E>> Entities=new List<IEntityReference<E>>();
               
        public IEntityReference<E> this[int index] { get => Entities[index]; set => Entities[index] = value; }

        IEntityReference<Entity> IEntityList.GetEntity(int index) => Entities[index];
        void IEntityList.SetEntity(int index, IEntityReference<Entity> value)
        {
            if (!(value.Entity is E))
                throw new InvalidCastException();
            Entities[index] = (IEntityReference<E>)value;
        }

        public int Count => Entities.Count;

        public bool IsReadOnly => false;

        public void Add(IEntityReference<E> item) => Entities.Add(item);
        void IEntityList.AddEntity(IEntityReference<Entity> e) => Entities.Add((IEntityReference<E>)e);

        public void Clear() => Entities.Clear();

        public bool Contains(IEntityReference<E> item) => Entities.Contains(item);        

        public void CopyTo(IEntityReference<E>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IEntityReference<E>> GetEnumerator() => Entities.GetEnumerator();

        public int IndexOf(IEntityReference<E> item) => Entities.IndexOf(item);

        public void Insert(int index, IEntityReference<E> item) => Entities.Insert(index, item);

        public bool Remove(IEntityReference<E> item) => Entities.Remove(item);        

        public void RemoveAt(int index) => Entities.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => Entities.GetEnumerator();

        public IEnumerable<IEntityReference<Entity>> AsEnumerable() => Entities;

        public override string ToString()
        {            
            return $"[{string.Join("; ", AsEnumerable().Select(_ => _.Entity))}]";
        }

        public EntityList<E> Clone()
        {
            var list = new EntityList<E>();
            list.Entities.AddRange(Entities);
            return list;
        }
    }
}
