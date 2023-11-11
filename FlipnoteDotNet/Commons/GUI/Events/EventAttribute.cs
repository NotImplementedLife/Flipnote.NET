using System;

namespace FlipnoteDotNet.Commons.GUI.Events
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    internal class EventAttribute : Attribute
    {
        public string EventName { get; }
        public string HandlerName { get; }

        public EventAttribute(string eventName, string handlerName = null)
        {
            EventName = eventName;
            HandlerName = handlerName;
        }
    }
}
