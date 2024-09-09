using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Utils
{
    public static class Zip
    {
        public static void WriteEntry(this ZipArchive archive, string path, string value)
        {
            var entry = archive.CreateEntry(path);
            using var stream = entry.Open();
            using var sw = new StreamWriter(stream, Encoding.Unicode);
            sw.Write(value);
        }

        public static void WriteEntry(this ZipArchive archive, string path, byte[] value)
        {
            var entry = archive.CreateEntry(path);
            using var stream = entry.Open();
            using var bw = new BinaryWriter(stream);
            bw.Write(value);
        }

        public static string ReadEntryString(this ZipArchive archive, string path)
        {
            var entry = archive.GetEntry(path);
            using var stream = entry.Open();
            using var sr = new StreamReader(stream, Encoding.Unicode);
            return sr.ReadToEnd();
        }

        public static byte[] ReadEntryBytes(this ZipArchive archive, string path)
        {
            var entry = archive.GetEntry(path);
            using var stream = entry.Open();
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

        public static byte[] ReadBytes(this ZipArchiveEntry entry)
        {            
            using var stream = entry.Open();
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
