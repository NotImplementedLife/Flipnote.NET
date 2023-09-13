using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace FlipnoteDotNet.Constants
{
    internal static class Reflection
    {
        public static Dictionary<Type, Type> DefaultEditors { get; private set; }
            
        public static void Init()
        {
            DefaultEditors = new AttributesManager<PropertyEditorControlAttribute, Control>()
                .GroupBy(_ => _.Attribute.Type).Select(g => g.First())
                .ToDictionary(r => r.Attribute.Type, r => r.Type);

            foreach (var kv in DefaultEditors)
                Debug.WriteLine($"{kv.Key} -> {kv.Value}");
        }

       
    }
}
