using FlipnoteDotNet.Commons.Reflection;
using FlipnoteDotNet.Data.Entities;
using PPMLib.Data;
using System;
using System.IO;

namespace FlipnoteDotNet.Data.Serialization
{
    [BinarySerializerProvider]
    public static class InternalSerializations
    {    
        public static void Write(BinaryWriter bw, Type type) => bw.Write(type.FullName);
        public static Type ReadType(BinaryReader br) => AssemblyScanner.GetTypeByFullName(br.ReadString());

        public static void Write(BinaryWriter bw, EntityDatabase db) => db.BinaryWrite(bw);
        public static EntityDatabase ReadEntityDatabase(BinaryReader br) => EntityDatabase.BinaryRead(br);
    }
}
