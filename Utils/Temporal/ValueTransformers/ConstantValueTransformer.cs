using FlipnoteDotNet.Attributes;
using System;

namespace FlipnoteDotNet.Utils.Temporal.ValueTransformers
{
    public class ConstantValueTransformer : ConstantValueTransformer<object>
    {
        public ConstantValueTransformer(object value, bool persistent = true) : base(value, persistent)
        {
        }

        public static IValueTransformer MakeFromType(Type type, object value, bool persistent = true)
        {
            var vtype = typeof(ConstantValueTransformer<>).MakeGenericType(type);
            return Activator.CreateInstance(vtype, value, persistent) as IValueTransformer;
        }
    }

    [EditorWildcard("Set Value %1")]
    public class ConstantValueTransformer<T> : SimpleValueTransformer<T>
    {
        private T _Value;
        [ParameterWildcard("%1")]
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

        public override string ToString() => $"ConstantValueTransformer({Value})";
    }
}
