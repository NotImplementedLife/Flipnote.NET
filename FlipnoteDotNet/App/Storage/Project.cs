using FlipnoteDotNet.App.Data;

namespace FlipnoteDotNet.App.Storage
{
    public abstract class Project : IDisposable
    {
        public ushort FormatVersion { get; private set; }

        protected Project(ushort formatVersion)
        {
            FormatVersion = formatVersion;
        }

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    OnDisposing();
                }                
                OnUnmanagedDisposing();
                disposedValue = true;
            }
        }

        protected virtual void OnDisposing()
        {            
            
        }

        protected virtual void OnUnmanagedDisposing()
        {
            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
        }
        
        ~Project()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
