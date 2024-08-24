using FlipnoteDotNet.App.Canvas.Components;

namespace FlipnoteDotNet.App.Data
{
    public abstract class Asset : IDisposable
    {
        public int Id { get; set; }        
        public readonly Bitmap Thumbnail;
        public string Name;
        public Asset(Bitmap thumbnail, string name = "")
        {            
            Thumbnail = thumbnail;
            Name = name;
        }

        public abstract FlipnoteCanvasComponent CreateCanvasComponent();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                Thumbnail?.Dispose();
            }
            _disposed = true;
        }

        ~Asset()
        {
            Dispose(false);
        }
    }
}
