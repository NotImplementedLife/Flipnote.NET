using FlipnoteDotNet.Commons.Reflection;
using System;
using System.Linq;
using System.Reflection;

namespace FlipnoteDotNet.Commons.GUI.Events
{
    public static class EventLoader
    {
        private static void ProcessTargetFields(object obj, Action<object, EventInfo, Delegate> action)
        {
            var objType = obj.GetType();
            var methodFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var field in ClassScanner.GetFields(objType, nonPublic: true)) 
            {
                var attributes = field.GetCustomAttributes(true).Where(a => a is EventAttribute).Cast<EventAttribute>().ToArray();
                if (attributes.Length == 0)
                    continue;

                var fieldValue = field.GetValue(obj);
                foreach (var attr in attributes)
                {
                    var ev = fieldValue.GetType().GetEvent(attr.EventName);                    
                    var method = objType.GetMethod(attr.HandlerName ?? $"{field.Name}_{attr.EventName}", methodFlags);
                    var handler = Delegate.CreateDelegate(ev.EventHandlerType, obj, method);
                    action(fieldValue, ev, handler);
                }
            }
        }

        public static void AttachAll(object obj)
        {
            ProcessTargetFields(obj, (f, ev, handler) => ev.AddEventHandler(f, handler));
        }

        public static void DetachAll(object obj)
        {
            ProcessTargetFields(obj, (f, ev, handler) => ev.RemoveEventHandler(f, handler));
        }

    }
}
