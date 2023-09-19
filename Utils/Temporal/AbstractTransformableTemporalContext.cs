using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace FlipnoteDotNet.Utils.Temporal
{    
    public abstract class AbstractTransformableTemporalContext : ITimeLocalizable, IInitialize
    {
        private int _CurrentTimestamp;

        public int CurrentTimestamp 
        {
            get => _CurrentTimestamp;
            set
            {
                var oldValue = _CurrentTimestamp;
                _CurrentTimestamp = value;
                CurrentTimestampChanged?.Invoke(this, new PropertyChanedEventArgs<int>(oldValue, value));
            }
        }
        public event EventHandler<PropertyChanedEventArgs<int>> CurrentTimestampChanged;
        public event EventHandler<PropertyChanedEventArgs<int>> StartTimestampChanged;

        class TransformersManager
        {
            Dictionary<IValueTransformer, int> Transformers = new Dictionary<IValueTransformer, int>();
            SortedDictionary<int, List<IValueTransformer>> Timestamps = new SortedDictionary<int, List<IValueTransformer>>();

            public IEnumerable<KeyValuePair<IValueTransformer, int>> GetTransformersAndTimestamps()
                => Transformers;

            public IEnumerable<IValueTransformer> GetTransformersAt(int timestamp)
            {
                return Timestamps.TryGetValue(timestamp, out var result) ? result : Enumerable.Empty<IValueTransformer>();
            }

            public IEnumerable<IValueTransformer> GetAllTransformers()
            {
                return Transformers.Keys;
            }
            public void ChangeTimestamp(IValueTransformer transformer, int newTimestamp)
            {
                var oldTimestamp = Transformers[transformer];
                GetList(oldTimestamp).Remove(transformer);
                Transformers[transformer] = newTimestamp;
                GetList(newTimestamp).Add(transformer);
            }

            public void ShiftTimestamp(IValueTransformer transformer, int dt)
            {
                ChangeTimestamp(transformer, Transformers[transformer] + dt);
            }

            public void Put(IValueTransformer transformer, int timestamp)
            {
                if (transformer == null) throw new ArgumentNullException();

                Debug.WriteLine(transformer.GetType());
                if(transformer.GetType().GetCustomAttribute<ExclusiveAttribute>()!=null)
                {
                    var candidates = GetTransformersAt(timestamp).Where(_ => _.GetType() == transformer.GetType()).ToList();
                    candidates.ForEach(Remove);
                }

                Transformers[transformer] = timestamp;
                GetList(timestamp).Add(transformer);
            }

            public void Remove(IValueTransformer transformer)
            {
                if(Transformers.TryGetValue(transformer, out var timestamp))
                {
                    Transformers.Remove(transformer);
                    GetList(timestamp).Remove(transformer);
                }
            }

            private List<IValueTransformer> GetList(int timestamp)
                => Timestamps.TryGetValue(timestamp, out var result) ? result : (Timestamps[timestamp] = new List<IValueTransformer>());
        }

        private readonly Dictionary<ITimeDependentValue, TransformersManager> TemporalTransformers = new Dictionary<ITimeDependentValue, TransformersManager>();

        public void OnInitialize()
        {
            GetType()
                .GetAllPublicProperties()
                .Where(_ => _.PropertyType.IsConstructedGenericType
                         && _.PropertyType.GetGenericTypeDefinition() == typeof(TimeDependentValue<>))
                .Select(_ => _.GetValue(this) as ITimeDependentValue)
                .ForEach(_ => TemporalTransformers[_] = new TransformersManager());
        }

        private int _StartTimestamp;        

        public virtual int StartTimestamp 
        {
            get => _StartTimestamp;
            set
            {
                var dt = value - _StartTimestamp;
                if (dt == 0) return;
                foreach(var manager in TemporalTransformers.Values)
                {
                    foreach (var transformer in manager.GetAllTransformers().ToArray()) 
                    {
                        manager.ShiftTimestamp(transformer, dt);                        
                    }
                }
                var oldValue = _StartTimestamp;
                _StartTimestamp = value;
                UpdateAllTimeDependentValues();
                StartTimestampChanged?.Invoke(this, new PropertyChanedEventArgs<int>(oldValue, value));
            }
        }

        public void PutTransformer(ITimeDependentValue value, IValueTransformer transformer, int timestamp)
        {
            if (TemporalTransformers.TryGetValue(value, out var manager))
            {
                manager.Put(transformer, timestamp);                
            }
            else throw new InvalidOperationException("Property is not owned by the instance");
        }

        public void RemoveTransformer(ITimeDependentValue value, IValueTransformer transformer)
        {
            GetTransformersManager(value).Remove(transformer);            
        }

        public void UpdateAllTimeDependentValues()
        {
            TemporalTransformers.Keys.ForEach(_ =>
            {
                UpdateTransformations(_);
                _.UpdateTimestamps();
            });            
        }

        public void UpdateTransformations(ITimeDependentValue value)
        {
            value.ClearValueChangers();
            foreach (var kv in GetTransformersManager(value).GetTransformersAndTimestamps())
            {
                var transformer = kv.Key;
                var timestamp = kv.Value;                
                ValueTransformer.Apply(transformer, value, timestamp);
            }            
        }

        public IEnumerable<(IValueTransformer Transformer, int Timestamp)> GetTransformers(ITimeDependentValue value)
        {
            foreach (var kv in GetTransformersManager(value).GetTransformersAndTimestamps()) 
                yield return (kv.Key, kv.Value);         
        }

        private TransformersManager GetTransformersManager(ITimeDependentValue value)
        {
            if (TemporalTransformers.TryGetValue(value, out var manager))
            {
                return manager;
            }
            else throw new InvalidOperationException("Property is not owned by the instance");
        }
    }
}
