namespace FlipnoteDotNet.Utils
{
    public static class MainThreadInvoker
    {
        private static readonly Control Invoker;

        public static object Invoke(Delegate method) => Invoker.Invoke(method);
        public static object Invoke(Delegate method, params object[] args) => Invoker.Invoke(method, args);

        static MainThreadInvoker()
        {
            Invoker = new Control();
            Invoker.CreateControl();
        }

    }
}
