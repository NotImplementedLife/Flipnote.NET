namespace FlipnoteDotNet.Utils.Temporal
{
    public class PropertyChanedEventArgs<T>
    {
        public T OldValue { get; }
        public T NewValue { get; }

        public PropertyChanedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
