using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FlipnoteDotNet.Utils.Temporal
{
    using TTemporalTransformers = Dictionary<ITimeDependentValue, Dictionary<IValueTransformer, int>>;
    public abstract class AbstractTransformableTemporalContext : ITimeLocalizable, IInitialize
    {
        public int CurrentTimestamp { get; set; }


        private readonly TTemporalTransformers TemporalTransformers = new TTemporalTransformers();

        public void OnInitialize()
        {
            GetType()
                .GetAllPublicProperties()
                .Where(_ => _.PropertyType.IsConstructedGenericType 
                         && _.PropertyType.GetGenericTypeDefinition() == typeof(TimeDependentValue<>))
                .Select(_ => _.GetValue(this) as ITimeDependentValue)
                .ForEach(_ => TemporalTransformers[_] = new Dictionary<IValueTransformer, int>());
        }

        private int _StartTimestamp;
        public virtual int StartTimestamp 
        {
            get => _StartTimestamp;
            set
            {
                var dt = value - _StartTimestamp;
                foreach(var transformersDict in TemporalTransformers.Values)
                {
                    foreach (var transformer in transformersDict.Keys.ToArray()) 
                    {
                        Debug.WriteLine($"Old = {transformersDict[transformer]}");
                        transformersDict[transformer] += dt;
                        Debug.WriteLine($"New = {transformersDict[transformer]}");
                    }
                }
                _StartTimestamp = value;
            }
        }

        public void PutTransformer(ITimeDependentValue value, IValueTransformer transformer, int timestamp)
        {
            if (TemporalTransformers.TryGetValue(value, out var transformersDict))
            {
                transformersDict[transformer] = timestamp;
            }
            else throw new InvalidOperationException("Property is not owned by the instance");
        }

        public void RemoveTransformer(ITimeDependentValue value, IValueTransformer transformer)
        {
            GetTransformersDict(value).Remove(transformer);            
        }

        public void UpdateTransformations(ITimeDependentValue value)
        {
            value.ClearValueChangers();
            foreach (var kv in GetTransformersDict(value))
            {
                var transformer = kv.Key;
                var timestamp = kv.Value;
                ValueTransformer.Apply(transformer, value, timestamp);
            }            
        }

        public IEnumerable<(IValueTransformer Transformer, int Timestamp)> GetTransformers(ITimeDependentValue value)
        {            
            foreach (var kv in GetTransformersDict(value))
                yield return (kv.Key, kv.Value);         
        }

        private Dictionary<IValueTransformer, int> GetTransformersDict(ITimeDependentValue value)
        {
            if (TemporalTransformers.TryGetValue(value, out var transformersDict))
            {
                return transformersDict;
            }
            else throw new InvalidOperationException("Property is not owned by the instance");
        }
    }
}
