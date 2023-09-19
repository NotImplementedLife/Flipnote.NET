using System;
using System.Drawing;

namespace FlipnoteDotNet.Data.Layers
{
    public interface IDisplayLayer
    {
        string DisplayName { get; set; }

        Bitmap GetDisplayThumbnail();

        event EventHandler DisplayChanged;
    }
}
