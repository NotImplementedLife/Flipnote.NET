using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Data;
using FlipnoteDotNet.Extensions;
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

        //public static Dictionary<Type, List<PropertyInfo>> TimeDependendProperties { get; private set; }
            
        public static void Init()
        {
            DefaultEditors = new AttributesManager<PropertyEditorControlAttribute, Control>()
                .GroupBy(_ => _.Attribute.Type).Select(g => g.First())
                .ToDictionary(r => r.Attribute.Type, r => r.Type);

            //foreach (var kv in DefaultEditors) Debug.WriteLine($"{kv.Key} -> {kv.Value}");


            /*TimeDependendProperties = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .Where(t => t.IsClass && typeof(ITemporalContext).IsAssignableFrom(t))
                       .Select(t =>
                       {
                           var props = t.GetAllPublicProperties()
                           .Where(_ => _.PropertyType.IsConstructedGenericType
                            && _.PropertyType.GetGenericTypeDefinition() == typeof(TimeDependentValue<>))
                           .ToList();
                           return (Type: t, Props: props);
                       }).ToDictionary(_ => _.Type, _ => _.Props);

            foreach (var kv in TimeDependendProperties)
            {
                Debug.WriteLine($"{kv.Key} -> {kv.Value.JoinToString(", ")}");
            }*/

            Debug.WriteLine("--------------------------------");
            Debug.WriteLine("");
            Debug.WriteLine("");
            Debug.WriteLine("");

        }





    }
}
