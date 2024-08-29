using FlipnoteDotNet.Drawing;

namespace FlipnoteDotNet.App.Data
{
    public static class PaletteConfigs
    {
        public static readonly PaletteConfig Flipnote = new PaletteConfig(Palettes.FlipnotePalette, 3, new[] { 3, 2, 1 });
        public static readonly PaletteConfig Flipnote3D = new PaletteConfig(Palettes.Flipnote3DPalette);
        public static readonly PaletteConfig VGA16 = new PaletteConfig(Palettes.VGA16);
    }
}
