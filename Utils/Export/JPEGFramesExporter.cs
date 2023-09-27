using FlipnoteDotNet.Data;
using FlipnoteDotNet.Rendering.Frames;
using FlipnoteDotNet.Utils.GUI;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace FlipnoteDotNet.Utils.Export
{
    internal static class JPEGFramesExporter
    {
        public static byte[] ExportArchive(SequenceManager sequenceManager, IProgressTracker tracker = null)
        {            
            if (sequenceManager == null)
                throw new System.ArgumentNullException(nameof(sequenceManager));

            var key = AppConfig.DSiJpegAesKey;

            if (key == null)
            {
                UserPrompt.Error("DSi Jpeg key is required for this action but is not provided.");
                return null;
            }

            var jpegExporter = new GDIJpegLayerExporter(key);

            var effectiveFramesCount = sequenceManager.GetTracks().Select(_ => _.GetSequences().Max(s => (int?)s.EndFrame) ?? 1).Max();
            tracker?.SetMaximumStepsCount(effectiveFramesCount);
            tracker?.ResetCurrentStep();

            var jpegLayerBytes = FlipnoteFramesRenderer
                .CreateFrames(sequenceManager).Take(effectiveFramesCount)
                .SelectMany(_ => new[] { _.Layer1, _.Layer2 })
                .Select(jpegExporter.Export);

            int i = 1;

            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var bytes in jpegLayerBytes)
                    {
                        if (tracker.CancellationPending) return null;
                        var entry = archive.CreateEntry($"HNI_{(i++).ToString().PadLeft(4, '0')}.JPG");
                        using (var zipStream = entry.Open())
                            zipStream.Write(bytes, 0, bytes.Length);
                        if (i % 2 == 1)
                            tracker.IncrementCurrentStep();
                    }                    
                }
                if (tracker.CancellationPending) return null;
                return ms.ToArray();
            }
        }
    }
}
