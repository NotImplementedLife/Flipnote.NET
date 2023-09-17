using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Data;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Canvas;
using FlipnoteDotNet.Rendering;
using FlipnoteDotNet.Utils.Paint;
using FlipnoteDotNet.Utils.Temporal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.Constants
{
    internal static class Reflection
    {
        public static Dictionary<Type, Type> DefaultEditors { get; private set; }

        public static Dictionary<Type, Type> LayerCanvasComponents { get; private set; }

        public static List<(Type ToolType, string ToolName, Bitmap ToolIcon)> PaintTools { get; private set; }
        
            
        public static void Init()
        {
            DefaultEditors = new AttributesManager<PropertyEditorControlAttribute, Control>()
                .GroupBy(_ => _.Attribute.Type).Select(g => g.First())
                .ToDictionary(r => r.Attribute.Type, r => r.Type);

            LayerCanvasComponents = new AttributesManager<CanvasComponentAttribute, ILayerCanvasComponent>()                
                .GroupBy(_ => _.Attribute.ObjectType).Select(g => g.First())
                .ToDictionary(r => r.Attribute.ObjectType, r => r.Type);

            PaintTools = new AttributesManager<PaintToolAttribute, IPaintTool>()
                .Select(r =>
                {
                    var bmp = string.IsNullOrEmpty(r.Attribute.IconResourceName)
                        ? null : GetResourceByName<Bitmap>(r.Attribute.IconResourceName);
                    return (r.Type, r.Attribute.ToolName, bmp);
                })
                .ToList();


            foreach (var kv in LayerCanvasComponents)
            {
                Debug.WriteLine($"{kv.Key} => {kv.Value}");
            }

            Debug.WriteLine("--------------------------------");
            Debug.WriteLine("");
            Debug.WriteLine("");
            Debug.WriteLine("");
        }

        public static T GetResourceByName<T>(string name)
            => (T)typeof(Properties.Resources)
                .GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .GetValue(null);


    }
}
