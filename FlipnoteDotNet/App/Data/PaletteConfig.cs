using FlipnoteDotNet.Drawing;

namespace FlipnoteDotNet.App.Data
{
    public class PaletteConfig
    {
        public readonly Palette Palette = Palettes.FlipnotePalette;
        public readonly int ColorsPerFrame = 0;
        public readonly int[] PresetColorIndices;

        public PaletteConfig(Palette palette, int colorsPerFrame = 0, int[] colorIndices = null)
        {
            Palette = palette;
            ColorsPerFrame = colorsPerFrame;
            PresetColorIndices = colorIndices;
        }
    }
}
