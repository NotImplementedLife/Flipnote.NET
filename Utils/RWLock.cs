using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Utils
{
    internal class RWLock
    {
        ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();

        public T ReadLockExecute<T>(Func<T> f)
        {
            Lock.EnterReadLock();
            var result = f();
            Lock.ExitReadLock();
            return result;
        }

        public T WriteLockExecute<T>(Func<T> f)
        {
            Lock.EnterWriteLock();
            var result = f();
            Lock.ExitWriteLock();
            return result;
        }

        public void ReadLockExecute(Action f)
        {
            Lock.EnterReadLock();
            f();
            Lock.ExitReadLock();            
        }

        public void WriteLockExecute(Action f)
        {
            Lock.EnterWriteLock();
            f();
            Lock.ExitWriteLock();            
        }



    }
}
