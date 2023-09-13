namespace FlipnoteDotNet.Utils
{
    // https://stackoverflow.com/questions/6792877/c-sharp-how-to-execute-code-after-object-construction-postconstruction
    public interface IInitialize
    {
        void OnInitialize();
    }

    public static class InitializeExtensions
    {
        public static void Initialize<T>(this T obj) where T : IInitialize
        {
            if (obj.GetType() == typeof(T))
                obj.OnInitialize();
        }
    }
}
