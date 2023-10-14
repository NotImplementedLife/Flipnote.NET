using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Data;
using FlipnoteDotNet.Rendering;
using FlipnoteDotNet.Utils.Paint;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet
{
    public static partial class Constants
    {
        internal static class Reflection
        {
            public static Dictionary<Type, Type> DefaultEditors { get; private set; }

            public static Dictionary<Type, Type> LayerCanvasComponents { get; private set; }

            public static List<(Type ToolType, string ToolName, Bitmap ToolIcon, Type PaintContextEditorType)> PaintTools { get; private set; }


            public static List<Type> LayerTypes { get; private set; }

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
                        return (r.Type, r.Attribute.ToolName, bmp, r.Attribute.PaintContextEditor);
                    })
                    .ToList();

                LayerTypes = GetTypesFromAssembly(typeof(ILayer)).ToList();


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

            public static IEnumerable<Type> GetTypesFromAssembly()
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                         .SelectMany(t => t.GetTypes());
            }

            public static IEnumerable<Type> GetTypesFromAssembly(Type baseType)
                => GetTypesFromAssembly()
                    .Where(t => (baseType.IsInterface && t.GetInterfaces().Contains(baseType)) || t.IsSubclassOf(baseType));

        }
    }
}
