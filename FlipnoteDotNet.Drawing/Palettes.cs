using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Drawing
{
    public static class Palettes
    {
        public static readonly Palette FlipnotePalette = new Palette(new Color[]
        {
            Color.Black, Color.Red, Color.Blue, Color.White
        });

        public static readonly Palette Flipnote3DPalette = new Palette(new Color[]
        {
            Color.Black, Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.White
        });        

        public static readonly Palette VGA16 = new Palette(new Color[]
        {
            RGB(0x000000), RGB(0x800000), RGB(0x008000), RGB(0x808000),
            RGB(0x000080), RGB(0x800080), RGB(0x008080), RGB(0xc0c0c0),
            RGB(0x808080), RGB(0xff0000), RGB(0x00ff00), RGB(0xffff00),
            RGB(0x0000ff), RGB(0xff00ff), RGB(0x00ffff), RGB(0xffffff),
        });


        private static Color RGB(int rgb) => Color.FromArgb(unchecked((int)0xFF000000 | rgb));
    }
}
