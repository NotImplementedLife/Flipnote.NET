using System;
using System.Drawing;

namespace FlipnoteDotNet.Data.Layers
{
    public interface IDisplayLayer : ILayer
    {
        string DisplayName { get; set; }

        Bitmap GetDisplayThumbnail();

        event EventHandler DisplayChanged;
    }
}
