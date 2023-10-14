namespace FlipnoteDotNet.Utils
{
    internal static class Convert
    {
        public static T SafeConvert<T>(this object obj)
        {
            return obj is T result ? result : throw new System.InvalidCastException();
        }

    }
}
