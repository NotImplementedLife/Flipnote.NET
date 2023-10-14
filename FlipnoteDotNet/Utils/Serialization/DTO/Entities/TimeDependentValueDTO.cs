using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.Utils.Temporal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlipnoteDotNet.Utils.Serialization.DTO.Entities
{
    public class TimeDependentValueDTO<T>
    {
        public List<(int Timestamp, IValueTransformerDTO<T> Transformer)> Transformers { get; set; }

        public class Converter : IDTOConverter<TimeDependentValue<T>, TimeDependentValueDTO<T>>
        {
            public TimeDependentValueDTO<T> Instance2DTO(DTOModel model, TimeDependentValue<T> instance)
            {
                var transformers = instance.GetTransformers()
                    .Select(_ => (_.Timestamp, model.GetDTO<IValueTransformerDTO<T>>(_.Transformer))).ToList();
                return new TimeDependentValueDTO<T> { Transformers = transformers };
            }

            public TimeDependentValue<T> DTO2Instance(DTOModel model, TimeDependentValueDTO<T> dto, Func<TimeDependentValue<T>> activator)
            {
                var instance = activator();
                dto.Transformers
                    .Select(_ => (transformer: model.GetInstance<IValueTransformer<T>>(_.Transformer), timestamp: _.Timestamp))
                    .ForEach(instance.PutTransformer);
                return instance;
            }      
        }        
    }
}
