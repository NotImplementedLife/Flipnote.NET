using System;
using System.Collections.Generic;
using System.Linq;

namespace FlipnoteDotNet.Utils.Temporal.ValueTransformers
{
    public abstract class AbstractValueTransformer : IValueTransformer
    {
        public event EventHandler Changed;

        public abstract IEnumerable<(int Timestamp, IValueChanger Changers)> GenerateValueChangers(int timestamp);

        protected void TriggerChanged()
        {
            Changed?.Invoke(this, new EventArgs());
        }
    }

    public abstract class AbstractValueTransformer<T> : IValueTransformer<T>
    {
        public event EventHandler Changed;            
        public void Apply(TimeDependentValue<T> value, int timestamp) => ValueTransformer.Apply(this, value, timestamp);

        public abstract IEnumerable<(int Timestamp, IValueChanger<T> Changers)> GenerateValueChangers(int timestamp);

        IEnumerable<(int Timestamp, IValueChanger Changers)> IValueTransformer.GenerateValueChangers(int timestamp)
            => GenerateValueChangers(timestamp).Select(_ => (_.Timestamp, _.Changers as IValueChanger));

        protected void TriggerChanged()
        {
            Changed?.Invoke(this, new EventArgs());
        }
    }
}
