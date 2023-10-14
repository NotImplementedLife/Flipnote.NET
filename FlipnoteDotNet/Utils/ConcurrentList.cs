using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FlipnoteDotNet.Utils
{
    [Serializable]
    public class ConcurrentList<T> : IList<T>
    {
        [NonSerialized]
        private readonly RWLock RWLock = new RWLock();

        private readonly List<T> Items = new List<T>();

        public T this[int index] 
        {
            get => RWLock.ReadLockExecute(() => Items[index]);
            set => RWLock.WriteLockExecute(() => Items[index] = value);
        }

        public T GetNoLock(int index) => Items[index];
        public void SetNoLock(int index, T item) => Items[index] = item;

        public int Count => RWLock.ReadLockExecute(() => Items.Count);
        public bool IsReadOnly => false;

        public void Add(T item) => RWLock.WriteLockExecute(() => Items.Add(item));
        public void AddNoLock(T item) => Items.Add(item);

        public void Clear() => RWLock.WriteLockExecute(() => Items.Clear());

        public bool Contains(T item) => RWLock.ReadLockExecute(() => Items.Contains(item));

        public void CopyTo(T[] array, int arrayIndex) => RWLock.ReadLockExecute(() =>
        {
            int len = Math.Min(array.Length - arrayIndex, Items.Count);
            if (len < 0) return;
            for (int i = 0; i < len; i++)
                array[arrayIndex + i] = Items[i];
        });


        public void LockRead() => RWLock.LockRead();
        public void UnlockRead() => RWLock.UnlockRead();
        public void LockWrite() => RWLock.LockWrite();
        public void UnlockWrite() => RWLock.UnlockWrite();

        public IEnumerator<T> GetEnumerator() => RWLock.ReadLockExecute(() => Items.ToList().GetEnumerator());

        public int IndexOf(T item) => RWLock.ReadLockExecute(() => Items.IndexOf(item));

        public void Insert(int index, T item) => RWLock.WriteLockExecute(() => Items.Insert(index, item));

        public bool Remove(T item) => RWLock.WriteLockExecute(() => Items.Remove(item));

        public void RemoveAt(int index) => RWLock.WriteLockExecute(() => Items.RemoveAt(index));

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void ForEach(Action<T> action) => RWLock.ReadLockExecute(() => Items.ToList().ForEach(action));

        public List<T> ToSimpleList() => RWLock.ReadLockExecute(() => Items.ToList());
    }
}
