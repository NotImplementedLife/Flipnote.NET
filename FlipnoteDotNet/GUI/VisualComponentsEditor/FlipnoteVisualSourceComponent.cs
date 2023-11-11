using FlipnoteDotNet.Commons.Rendering;
using PPMLib.Rendering;
using PPMLib.Utils;
using PPMLib.Winforms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.GUI.VisualComponentsEditor
{
    public class FlipnoteVisualSourceComponent : VisualComponent
    {
        private readonly FlipnoteVisualSource pFlipnoteVisualSource;
        private Color pColor1 = FlipnoteColors.Black;
        private Color pColor2 = FlipnoteColors.Red;
        private bool pDithering = true;
        private RescaleMethod pRescaleMethod = RescaleMethod.Bilinear;


        public FlipnoteVisualSourceComponent(BitmapProcessor bitmapProcessor, FlipnoteVisualSource visualSource) : base(bitmapProcessor)
        {
            pFlipnoteVisualSource = visualSource;
        }

        public override Bitmap BuildBitmap()
        {
            var tx = Size.Width / 2;
            var ty = Size.Height / 2;
            using (var source = new FlipnoteVisualSource(pFlipnoteVisualSource, Size.Width, Size.Height, pDithering, pRescaleMethod)
                .ToBitmap(pColor1, pColor2))
            {
                var result = new Bitmap(Bounds.Width, Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                using(var g=Graphics.FromImage(result))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                    g.TranslateTransform(tx+(Bounds.Width - Size.Width) / 2, ty + (Bounds.Height - Size.Height) / 2);                    
                    g.RotateTransform(Rotation);
                    g.TranslateTransform(-tx, -ty);
                    g.DrawImage(source, 0, 0, Size.Width, Size.Height);
                }
                return result;
            }                
        }
    }
}
