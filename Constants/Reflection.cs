using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Data;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Canvas;
using FlipnoteDotNet.Rendering;
using FlipnoteDotNet.Utils.Temporal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.Constants
{
    internal static class Reflection
    {
        public static Dictionary<Type, Type> DefaultEditors { get; private set; }

        public static Dictionary<Type, Type> LayerCanvasComponents { get; private set; }
        
            
        public static void Init()
        {
            DefaultEditors = new AttributesManager<PropertyEditorControlAttribute, Control>()
                .GroupBy(_ => _.Attribute.Type).Select(g => g.First())
                .ToDictionary(r => r.Attribute.Type, r => r.Type);

            LayerCanvasComponents = new AttributesManager<CanvasComponentAttribute, ILayerCanvasComponent>()                
                .GroupBy(_ => _.Attribute.ObjectType).Select(g => g.First())
                .ToDictionary(r => r.Attribute.ObjectType, r => r.Type);

            foreach(var kv in LayerCanvasComponents)
            {
                Debug.WriteLine($"{kv.Key} => {kv.Value}");
            }

            Debug.WriteLine("--------------------------------");
            Debug.WriteLine("");
            Debug.WriteLine("");
            Debug.WriteLine("");
        }


    }
}
