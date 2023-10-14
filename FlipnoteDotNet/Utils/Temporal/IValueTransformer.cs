using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.Utils.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlipnoteDotNet.Utils.Temporal
{
    public static class ValueTransformer
    {
        public static void Apply<T>(IValueTransformer<T> transformer, TimeDependentValue<T> value, int timestamp)
        {
            transformer.GenerateValueChangers(timestamp).ForEach(_ => value.AddValueChanger(_.Timestamp, _.Changers));
        }

        public static void Apply(IValueTransformer transformer, ITimeDependentValue value, int timestamp)
        {
            transformer.GenerateValueChangers(timestamp).ForEach(_ => value.AddValueChanger(_.Timestamp, _.Changers));
        }        
    }

    public interface IValueTransformer 
    {
        IEnumerable<(int Timestamp, IValueChanger Changers)> GenerateValueChangers(int timestamp);
        event EventHandler Changed;
    }

    public interface IValueTransformer<T> : IValueTransformer, ISerializable
    {
        new IEnumerable<(int Timestamp, IValueChanger<T> Changers)> GenerateValueChangers(int timestamp);

        void Apply(TimeDependentValue<T> value, int timestamp);        
    }       
}
