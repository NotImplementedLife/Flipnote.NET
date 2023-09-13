using System;

namespace FlipnoteDotNet.Utils.Temporal
{
    public interface IValueChanger 
    {
        object ChangeValue(object previousValue);
    }

    public interface IValueChanger<T> : IValueChanger
    {
        T ChangeValue(T previousValue);
    }

    public class ConstantValueChanger<T> : IValueChanger<T>
    {
        private T Value { get; }
        public T ChangeValue(T previousValue) => Value;

        object IValueChanger.ChangeValue(object previousValue) => ChangeValue((T)previousValue);        

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
    }
}
