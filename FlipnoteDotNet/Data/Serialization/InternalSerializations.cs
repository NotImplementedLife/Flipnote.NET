using FlipnoteDotNet.Commons.Reflection;
using FlipnoteDotNet.Data.Entities;
using PPMLib.Data;
using System;
using System.Drawing;
using System.IO;

namespace FlipnoteDotNet.Data.Serialization
{
    [BinarySerializerProvider]
    public static class InternalSerializations
    {    
        public static void Write(BinaryWriter bw, Type type) => bw.Write(type.FullName);
        public static Type ReadType(BinaryReader br) => AssemblyScanner.GetTypeByFullName(br.ReadString());

        public static Color ReadColor(BinaryReader br)
        {
            var a = br.ReadByte();
            var r = br.ReadByte();
            var g = br.ReadByte();
            var b = br.ReadByte();
            return Color.FromArgb(a, r, g, b);
        }        

        public static void Write(BinaryWriter bw, Color color)
        {
            bw.Write(color.A);
            bw.Write(color.R);
            bw.Write(color.G);
            bw.Write(color.B);
        }

        public static void Write(BinaryWriter bw, EntityDatabase db) => db.BinaryWrite(bw);
        public static EntityDatabase ReadEntityDatabase(BinaryReader br) => EntityDatabase.BinaryRead(br);
    }
}
