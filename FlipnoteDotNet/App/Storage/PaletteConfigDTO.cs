using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.App.Storage
{
    public class PaletteConfigDTO
    {
        public int[] Colors { get; set; }        
        public int ColorsPerFrame { get; set; }
        public int[] PresetColorIndices { get; set; }

        public PaletteConfigDTO() { }

        public PaletteConfigDTO(Palette palette, int colorsPerFrame, int[] presetColorIndices)            
        {
            Colors = (from c in palette.Colors select c.ToArgb()).ToArray();
            ColorsPerFrame = colorsPerFrame;
            PresetColorIndices = presetColorIndices;
        }

        public PaletteConfig ToPaletteConfig()
        {
            var colors = Colors.Select(Color.FromArgb).ToArray();
            var palette = new Palette(colors);
            return new PaletteConfig(palette, ColorsPerFrame, PresetColorIndices);
        }
    }
}
