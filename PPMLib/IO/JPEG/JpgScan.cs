using PPMLib.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace PPMLib.IO.JPEG
{
    public class JpgScan
    {
        public interface IJpgComponent 
        {
            int Position { get; }
        }        

        public class JpgMarker : IJpgComponent
        {
            public int Position { get; }
            public ushort Type { get; set; }
            public ushort Length { get; set; }
            public JpgMarker(int position, ushort type, ushort length)
            {
                Position = position;
                Type = type;
                Length = length;
            }
        }

        public class JpgData : IJpgComponent
        {
            public int Position { get; }
            public byte[] Data { get; set; }

            public JpgData(int position, byte[] data)
            {
                Position = position;
                Data = data;
            }
        }

        public static byte[] AssembleJpg(IEnumerable<IJpgComponent> components)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms)) 
            {
                void BEWrite(ushort x)
                {
                    bw.Write((byte)(x >> 8));
                    bw.Write((byte)(x & 0xFF));
                }

                foreach(var comp in components)
                {
                    if(comp is JpgMarker marker)
                    {
                        BEWrite(marker.Type);
                        if (!marker.Type.IsBetween(0xFFD0, 0xFFDA))
                            BEWrite(marker.Length);                        
                    }
                    else if(comp is JpgData jdat)
                    {
                        bw.Write(jdat.Data);
                    }

                }
                return ms.ToArray();
            }

        }


        public static IEnumerable<IJpgComponent> DissectJpg(byte[] jpg)
        {
            var ffIndices = jpg.Select((b, i) => (b, i)).Where(g => g.b == 0xFF && g.i < jpg.Length - 1 && jpg[g.i + 1] != 0)
                .Select(_ => _.i).ToList();

            var ffRemove = new List<int>();

            JpgMarker lastMarker = null;
            foreach(var ffi in ffIndices)
            {
                if (lastMarker != null)
                {
                    if (ffi < lastMarker.Position + lastMarker.Length + 2) 
                    {
                        ffRemove.Add(ffi);
                        continue;
                    }
                }

                var mrkType = (ushort)(jpg[ffi] * 256 + jpg[ffi + 1]);

                if (mrkType == 0xFFDA)               
                    continue;                
                else
                {
                    if (mrkType.IsBetween(0xFFD0, 0xFFD9))
                    {
                        lastMarker = new JpgMarker(ffi, mrkType, 0);                        
                    }
                    else
                    {
                        var size = (ushort)(jpg[ffi + 2] * 256 + jpg[ffi + 3]);
                        lastMarker = new JpgMarker(ffi, mrkType, size);                        
                    }
                }
            }

            foreach (var ff in ffRemove) ffIndices.Remove(ff);



            Debug.WriteLine($"ffix = {ffIndices.Select(_ => $"{_:X}").JoinToString(" ")}");

            int index = 0;            

            foreach(var ffi in ffIndices)
            {                              
                if(index<ffi)
                {
                    var data = new List<byte>();

                    for(int k=index;k<ffi;k++)
                    {                        
                        data.Add(jpg[k]);
                    }

                    yield return new JpgData(index, data.ToArray());                    
                    index = ffi;                    
                }                

                var mrkType = (ushort)(jpg[ffi] * 256 + jpg[ffi + 1]);

                if (mrkType == 0xFFDA)
                {
                    yield return new JpgMarker(index, mrkType, 0);                    
                    yield return new JpgData(index + 2, jpg.Skip(index + 2).ToArray());
                    break;
                }
                else
                {
                    if (mrkType.IsBetween(0xFFD0, 0xFFD9)) 
                    {
                        yield return new JpgMarker(index, mrkType, 0);
                        index = ffi + 2;
                    }
                    else
                    {
                        var size = (ushort)(jpg[ffi + 2] * 256 + jpg[ffi + 3]);
                        yield return new JpgMarker(index, mrkType, size);
                        index = ffi + 4;
                    }
                }
            }           
        }      
    }
}
