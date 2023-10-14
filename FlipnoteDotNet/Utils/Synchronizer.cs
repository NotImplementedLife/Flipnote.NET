using System;

namespace FlipnoteDotNet.Utils
{
    internal static class Synchronizer
    {
        public static void InvokeOnMainThread(EventHandler eventHandler, params object[] args) 
        {
            Constants.SnychronizingObject.Invoke(eventHandler, args);
        }

        public static void InvokeOnMainThread(Action action)
        {
            if (Constants.SnychronizingObject.IsHandleCreated)
                Constants.SnychronizingObject.Invoke(action);
        }

    }
}
