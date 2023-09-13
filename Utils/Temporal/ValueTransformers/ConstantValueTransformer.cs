namespace FlipnoteDotNet.Utils.Temporal.ValueTransformers
{
    public class ConstantValueTransformer<T> : SimpleValueTransformer<T>
    {
        private T _Value;
        public T Value
        {
            get => _Value;
            set
            {
                if (!Equals(_Value, value)) 
                {
                    _Value = value;
                    ValueChanger = new ConstantValueChanger<T>(_Value);
                    TriggerChanged();
                }
            }
        }

        public ConstantValueTransformer(T value, bool persistent = true) : base(new ConstantValueChanger<T>(value), persistent)
        {
            _Value = value;
        }
    }
}
