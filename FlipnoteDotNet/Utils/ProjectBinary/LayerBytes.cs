using FlipnoteDotNet.Data;
using FlipnoteDotNet.Data.Layers;
using FlipnoteDotNet.Extensions;
using PPMLib.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FlipnoteDotNet.Utils.ProjectBinary
{
    internal static class LayerBytes
    {
        private static Dictionary<Type, (ushort TypeId, MethodInfo Method)> TypeIds;

        public static void Initialize()
        {
            TypeIds = typeof(LayerBytes).GetStaticVoidMethods("WriteLayer", typeof(BinaryWriter), typeof(ILayer))
                .Select((m, i) => (t: m.GetParameters()[1].ParameterType, i: (ushort)(i + 1), m: m))
                .ToDictionary(_ => _.t, _ => (_.i, _.m));
            foreach(var t in TypeIds.Keys)
            {                
                if (typeof(LayerBytes).GetStaticMethods("ReadLayer", returnType: t, typeof(BinaryReader)).Count() > 0)
                    continue;
                throw new NotImplementedException($"No read method defined for {t}");
            }
        }


        public static void WriteLayer(this BinaryWriter bw, StaticImageLayer layer)
        {
            bw.Write(layer.DisplayName);            
            bw.Write(layer.VisualSource);
            bw.Write(layer.Dithering);
            bw.Write(Commons.RescaleMethodToBytes(layer.RescaleMethod));
            bw.Write(layer.X);
            bw.Write(layer.Y);
            bw.Write(layer.ScaleX);
            bw.Write(layer.ScaleY);
        }

        public static StaticImageLayer ReadLayer(this BinaryReader br)
        {
            var name = br.ReadString();
            var vs = FlipnoteVisualSource.FromBinaryReader(br);
            var dithering = br.ReadBoolean();
            var rescaleMethod = br.ReadRescaleMethod();



            return null;
        }

        public static void Write(this BinaryWriter bw, ILayer layer)
        {
            if (!TypeIds.TryGetValue(layer.GetType(), out var im))
                throw new NotImplementedException($"Cannot write layer {layer.GetType()}");
            bw.Write(im.TypeId);
            im.Method.Invoke(null, new object[] { bw, layer });
        }      
    }
}
