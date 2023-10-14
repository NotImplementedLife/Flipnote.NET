using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.Utils.Temporal.ValueTransformers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FlipnoteDotNet.Utils.Temporal
{    
    public interface ITimeDependentValue 
    {
        object CurrentValue { get; set; }
        void AddValueChanger(int timestamp, IValueChanger valueChanger);
        void ClearValueChangers();
        void PutTransformer(IValueTransformer transformer, int timestamp);
        IEnumerable<(IValueTransformer Transformer, int Timestamp)> GetTransformers();
        void UpdateTransformations();
        void UpdateTimestamps();
    }
    
    public class TimeDependentValue<T> : ITimeDependentValue
    {
        private ITemporalContext Context { get; }
        private List<(int Timestamp, T Value)> States { get; } = new List<(int, T)>();

        private int GetBeforeTimestampCount(int t) // how many first States have Timestamp <= t
        {
            int low = 0;
            int high = States.Count;
            while (low < high)
            {
                int mid = (low + high) / 2;
                if (t < States[mid].Timestamp)
                    high = mid;
                else
                    low = mid + 1;
            }
            return low;
        }

        public T InitialValue { get; }

        public T GetValueAt(int timestamp)
        {            
            var timestampsBefore = GetBeforeTimestampCount(timestamp);
            if (timestampsBefore == 0) return InitialValue;
            return States[timestampsBefore - 1].Value;
        }
        public int CurrentTimestamp => Context.CurrentTimestamp;        

        public static implicit operator T(TimeDependentValue<T> v) => v.CurrentValue;

        private SortedDictionary<int, List<IValueChanger<T>>> ValueChangers { get; } = new SortedDictionary<int, List<IValueChanger<T>>>();

        public void PrintChangers()
        {
            foreach (var kv in ValueChangers)
            {
                int timestamp = kv.Key;
                var changers = kv.Value;                
                Debug.WriteLine($"{timestamp}=>{changers.JoinToString(", ")}");
            }
        }

        public void UpdateTimestamps()
        {
            States.Clear();
            var value = InitialValue;

            foreach(var kv in ValueChangers)
            {
                int timestamp = kv.Key;
                var changers = kv.Value;
                value = changers.Aggregate(value, (v, c) => c.ChangeValue(v));
                States.Add((timestamp, value));
            }
        }

        public TimeDependentValue(ITemporalContext context, T initialValue = default(T))
        {
            Context = context;
            InitialValue = initialValue;
            ClearValueChangers();

        }

        public void ClearValueChangers()
        {
            ValueChangers.Clear();
            AddValueChanger(int.MinValue, new ConstantValueChanger<T>(InitialValue));
            UpdateTimestamps();
        }

        public void AddValueChanger(int timestamp, IValueChanger<T> valueChanger)
        {
            if (!ValueChangers.ContainsKey(timestamp))
                ValueChangers[timestamp] = new List<IValueChanger<T>>();

            if (valueChanger is ConstantValueChanger<T>)
                ValueChangers[timestamp].Clear(); // small optimization for constant changers
            ValueChangers[timestamp].Add(valueChanger);
        }

        public void SetCurrentValue(T value)
        {
            AddValueChanger(CurrentTimestamp, new ConstantValueChanger<T>(value));
        }

        public T GetCurrentValue() => GetValueAt(CurrentTimestamp);

        public T CurrentValue
        {
            get => GetCurrentValue();
            set => SetCurrentValue(value);
        }
        object ITimeDependentValue.CurrentValue { get => CurrentValue; set => CurrentValue = (T)value; }

        public void PutCurrentConstantTransformer(T value, int timestamp, bool autoUpdate = false)
        {
            PutTransformer(new ConstantValueTransformer<T>(value), timestamp);
            if(autoUpdate)
            {
                UpdateTransformations();
                UpdateTimestamps();
            }
        }       

        public void PutTransformer(IValueTransformer transformer, int timestamp)
        {
            ForceGetAbstractTransformableTemporalContext(nameof(PutTransformer))
                .PutTransformer(this, transformer, timestamp);
        }

        public void UpdateTransformations()
        {            
            ForceGetAbstractTransformableTemporalContext(nameof(UpdateTransformations))
                .UpdateTransformations(this);
        }

        void ITimeDependentValue.AddValueChanger(int timestamp, IValueChanger valueChanger)
        {
            if (valueChanger is IValueChanger<T> valueChangerT)
                AddValueChanger(timestamp, valueChangerT);
            else
                AddValueChanger(timestamp, new ConvertedValueChanger<T>(valueChanger));
        }

        IEnumerable<(IValueTransformer Transformer, int Timestamp)> ITimeDependentValue.GetTransformers()
        {            
            return ForceGetAbstractTransformableTemporalContext(nameof(ITimeDependentValue.GetTransformers)).GetTransformers(this);
        }

        public IEnumerable<(IValueTransformer<T> Transformer, int Timestamp)> GetTransformers()
            => (this as ITimeDependentValue).GetTransformers()
                .Select(_ => (_.Transformer as IValueTransformer<T>, _.Timestamp));

        private AbstractTransformableTemporalContext ForceGetAbstractTransformableTemporalContext(string callerName)
        {
            if (!(Context is AbstractTransformableTemporalContext tctx))
                throw new InvalidOperationException($"{callerName}() method only works on classes derived from AbstractTransformableTemporalContext");
            return tctx;
        }

    }
}
