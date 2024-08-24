namespace FlipnoteDotNet.Core.Utils
{
    public static class Arrays
    {
        public static T[] Initialize<T>(int length, T value)
        {
            var array = GC.AllocateUninitializedArray<T>(length);
            Array.Fill(array, value);
            return array;
        }
    }
}
