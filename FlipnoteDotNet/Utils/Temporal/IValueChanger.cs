using FlipnoteDotNet.Utils.Serialization;
using System;

namespace FlipnoteDotNet.Utils.Temporal
{
    public interface IValueChanger : ISerializable
    {
        object ChangeValue(object previousValue);
    }

    public interface IValueChanger<T> : IValueChanger
    {
        T ChangeValue(T previousValue);
    }

    public class ConstantValueChanger<T> : IValueChanger<T>
    {
        public T Value { get; }
        public T ChangeValue(T previousValue) => Value;

        object IValueChanger.ChangeValue(object previousValue) => ChangeValue((T)previousValue);

        public void Serialize(SerializeWriter w)
        {
            w.Write(Value);
        }

        public void Deserialize(SerializeReader r, object target, bool isInitialized)
        {
            var value = r.Read<T>();
        }

        public ConstantValueChanger(T value)
        {
            Value = value;
        }
    }

    public class FunctionalValueChanger<T> : IValueChanger<T>
    {
        private Func<T,T> Function { get; }
        public T ChangeValue(T previousValue) => Function.Invoke(previousValue);
        public FunctionalValueChanger(Func<T, T> function)
        {
            Function = function;
        }

        object IValueChanger.ChangeValue(object previousValue) => ChangeValue((T)previousValue);

        public void Serialize(SerializeWriter w)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(SerializeReader r, object target, bool isInitialized)
        {
            throw new NotImplementedException();
        }
    }

    public class ConvertedValueChanger<T> : IValueChanger<T>
    {
        private readonly IValueChanger ValueChanger;

        public ConvertedValueChanger(IValueChanger valueChanger)
        {
            ValueChanger = valueChanger;
        }

        public T ChangeValue(T previousValue)
        {
            var value = ValueChanger.ChangeValue(previousValue);

            if (value == null && typeof(T).IsClass)
                return (T)value;
            if (value is T) return (T)value;
            throw new InvalidCastException($"ConvertedValueChanger<>: Cannot cast {value.GetType()} to {typeof(T)}");
        }

        public object ChangeValue(object previousValue) => ChangeValue((T)previousValue);

        public void Deserialize(SerializeReader r, object target, bool isInitialized)
        {
            var valueChanger = r.Read<IValueChanger>();
            r.CallConstructor<ConvertedValueChanger<T>>(target, valueChanger);
        }

        public void Serialize(SerializeWriter w)
        {
            w.Write(ValueChanger);
        }
    }
}
