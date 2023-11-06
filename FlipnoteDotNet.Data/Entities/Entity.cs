using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlipnoteDotNet.Data.Entities
{
    public class Entity
    {
        public override string ToString()
        {
            return "{" + String.Join(", ", GetType().GetProperties().Select(p => $"{p.Name}={p.GetValue(this)}")) + "}";
        }

        /*public Entity Clone()
        {
            var e = Activator.CreateInstance(GetType()) as Entity;
            
            foreach(var prop in GetType().GetProperties())
            {
                var val = prop.GetValue(this);

                if (val is EntityList<Entity> eList)
                {
                    prop.SetValue(e, eList.Clone());
                }
                else if (prop.PropertyType.IsValueType || prop.PropertyType == typeof(string))
                {
                    prop.SetValue(e, val);
                }
                else throw new InvalidOperationException($"Cannot clone object of type {prop.PropertyType}");
            }
            return e;
        }*/
    }
}
