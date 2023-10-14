using FlipnoteDotNet.Utils.Temporal.ValueTransformers;
using FlipnoteDotNet.Utils.Temporal;
using PPMLib.Data;
using System;
using System.IO;
using System.Linq;
using PPMLib.Rendering;
using PPMLib.Utils;

namespace FlipnoteDotNet.Utils.ProjectBinary
{
    internal static class Commons
    {
        public static RescaleMethod ReadRescaleMethod(this BinaryReader br) => RescaleMethodFromBinaryReader(br);
        public static FlipnoteVisualSource ReadFlipnoteVisualSource(this BinaryReader br) => FlipnoteVisualSource.FromBinaryReader(br);

        public static void Write(this BinaryWriter bw, RescaleMethod rm) => bw.Write(RescaleMethodToBytes(rm));          
        public static void Write(this BinaryWriter bw, FlipnoteVisualSource vs) => bw.Write(vs.ToBytes());                                

        public static void Write<T>(this BinaryWriter bw, TimeDependentValue<T> tdv, Func<T, byte[]> tToBytes)
        {
            bw.Write(tToBytes(tdv.InitialValue));

            var transformers = tdv.GetTransformers().ToList();
            bw.Write(transformers.Count);
            foreach (var (transformer, timestamp) in transformers)
            {
                bw.Write(timestamp);
                bw.Write(transformer, tToBytes);
            }
        }

        public static void Write<T>(this BinaryWriter bw, IValueTransformer<T> tsf, Func<T, byte[]> tToBytes)
        {
            if (tsf is ConstantValueTransformer<T> ctt)
            {
                bw.Write(ctt.Persistent);
                bw.Write(tToBytes(ctt.Value));
                return;
            }
            throw new NotImplementedException($"No write method for transformer {tsf.GetType()}");
        }



        /*public static IValueTransformer<T> ReadValueTransformer()
        {

        }*/

        public static byte[] FlipnotePenToBytes(FlipnotePen pen)
           => new byte[] { (byte)(pen == FlipnotePen.Red ? 1 : pen == FlipnotePen.Blue ? 2 : 0) };

        public static byte[] FlipnotePaperColorToBytes(FlipnotePaperColor paperColor)
            => new byte[] { (byte)(paperColor == FlipnotePaperColor.Black ? 1 : 0) };

        public static byte[] RescaleMethodToBytes(RescaleMethod rescaleMethod)
            => new byte[] { (byte)(rescaleMethod == RescaleMethod.Bilinear ? 1 : 0) };        

        public static RescaleMethod RescaleMethodFromBinaryReader(BinaryReader br)
        {
            return br.ReadByte() == 1 ? RescaleMethod.Bilinear : RescaleMethod.NearestNeighbor;
        }
    }
}
