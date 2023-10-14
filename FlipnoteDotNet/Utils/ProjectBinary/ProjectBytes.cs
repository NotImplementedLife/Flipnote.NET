using FlipnoteDotNet.Data;
using FlipnoteDotNet.Data.Layers;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.Project;
using FlipnoteDotNet.Utils.Temporal;
using FlipnoteDotNet.Utils.Temporal.ValueTransformers;
using PPMLib.Data;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace FlipnoteDotNet.Utils.ProjectBinary
{
    internal static class ProjectBytes
    {
        public static readonly char Version = '0';

        public static byte[] Export(FlipnoteProject project)
        {
            using(var ms=new MemoryStream())
            {
                using(var bw=new BinaryWriter(ms))
                {
                    bw.Write(Encoding.ASCII.GetBytes($"FLIPNET{Version}"));
                    var sm = project.SequenceManager;
                    bw.Write(sm.TracksCount);

                    var seqsi = sm.GetTracks().Select((s, i) => s.GetSequences().Select(seq => (seq, i))).SelectMany(_ => _).ToArray();
                    bw.Write(seqsi.Length);
                    seqsi.ForEach(bw.Write);
                }
                return ms.ToArray();
            }         
        }

        private static void Write(this BinaryWriter bw, Sequence s, int trackId)
        {
            bw.Write(trackId);
            bw.Write(s.Name);
            bw.Write(s.StartFrame);
            bw.Write(s.EndFrame);
            bw.Write(s.Color.ToArgb());

            bw.Write(s.PaperColor, Commons.FlipnotePaperColorToBytes);
            bw.Write(s.Pen1, Commons.FlipnotePenToBytes);
            bw.Write(s.Pen2, Commons.FlipnotePenToBytes);

            bw.Write(s.Layers.Count);
            s.Layers.ForEach(bw.Write);
            
        }               
    }
}
