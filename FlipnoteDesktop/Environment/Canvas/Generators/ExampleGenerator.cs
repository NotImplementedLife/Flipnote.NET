using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlipnoteDesktop.Data;

namespace FlipnoteDesktop.Environment.Canvas.Generators
{
    public class ExampleGenerator : Generator
    {
        public ExampleGenerator() { }

        public override string Name { get => "Example"; }

        public override List<DecodedFrame> GenerateFrames()
        {
            var res = new List<DecodedFrame>();

            int shift = 0;
            for (int k = 1; k <= 39; k++) 
            {
                if (k > 12) shift++;
                var f = new DecodedFrame();
                for (int i = -23 + k; i < -11 + k; i++)
                    for (int j = 0; j < 12; j++)
                        PlaceSquare(f, (i + 2 * j) % 2 == 0 ? 1 : 2, i + j, j);

                f.Layer1Color = LayerColor.BlackWhite;
                f.Layer2Color = LayerColor.Blue;                
                f.SetImage(null, true);
                f.CreateThumbnail();
                res.Add(f);
            }
            return res;
        }

        public void PlaceSquare(DecodedFrame frame,int layer, int px,int py)
        {
            if (!((0 <= px && px < 16) && (0 <= py && py < 12))) 
                return;            
            for (int x = 0; x < 16; x++)
                for (int y = 0; y < 16; y++)
                {
                    if (layer == 1)
                        frame.Layer1Data[16 * px + x, 16 * py + y] = true;
                    else
                        frame.Layer2Data[16 * px + x, 16 * py + y] = true;
                }
        }
    }
}
