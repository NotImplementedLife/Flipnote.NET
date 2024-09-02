using FlipnoteDotNet.App.Canvas.Components;
using FlipnoteDotNet.App.Storage;
using System.Drawing.Imaging;

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

        public abstract AssetDTO ToDTO();

        protected byte[] GetThumbnailBytes()
        {
            using(var ms=new MemoryStream())
            {
                Thumbnail.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

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
