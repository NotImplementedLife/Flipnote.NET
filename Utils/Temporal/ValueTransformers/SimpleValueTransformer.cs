using System;
using System.Collections.Generic;
using System.Linq;

namespace FlipnoteDotNet.Utils.Temporal.ValueTransformers
{
    public class SimpleValueTransformer<T> : AbstractValueTransformer<T>
    {        
        private IValueChanger<T> _ValueChanger;

        public IValueChanger<T> ValueChanger
        {
            get => _ValueChanger;
            protected set
            {
                if (_ValueChanger != value)
                {
                    _ValueChanger = value;
                    TriggerChanged();
                }
            }
        }

        private bool _Persistent;
        public bool Persistent
        {
            get => _Persistent;
            set
            {
                if (_Persistent != value)
                {
                    _Persistent = value;
                    TriggerChanged();
                }
            }
        }

        public SimpleValueTransformer(IValueChanger<T> valueChanger, bool persistent = true)
        {
            _ValueChanger = valueChanger;
            _Persistent = persistent;
        }


        public override IEnumerable<(int, IValueChanger<T>)> GenerateValueChangers(int timestamp)
        {
            var valueStore = new ValueStore();

            var changer = new FunctionalValueChanger<T>(t =>
            {
                valueStore.Value = t;
                return ValueChanger.ChangeValue(t);
            });

            yield return (timestamp, changer);

            if (!Persistent)
            {
                // reset on the next timestamp
                yield return (timestamp + 1, new FunctionalValueChanger<T>(t => valueStore.Value));
            }
        }

        private class ValueStore { public T Value { get; set; } }        
    }
}
