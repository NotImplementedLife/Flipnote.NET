using System;
using System.Collections.Generic;
using System.Text;

namespace PPMLib.Data
{
    public class FlipnoteThumbnail
    {
        private byte[] Data { get; } = new byte[1536];

        public FlipnoteThumbnail(byte[] data)
        {
            Array.Copy(data, Data, Math.Min(1536, data.Length));            
        }
    }
}
