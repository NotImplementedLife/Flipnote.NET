using FlipnoteDotNet.Utils.Temporal.ValueTransformers;
using System;

namespace FlipnoteDotNet.Utils.Serialization.DTO.Entities
{
    public class ConstantValueTransformerDTO<T> : IValueTransformerDTO<T>
    {
        public T Value { get; set; }
        public bool Persistent { get; set; }

        public class Converter : IDTOConverter<ConstantValueTransformer<T>, ConstantValueTransformerDTO<T>>
        {
            public ConstantValueTransformerDTO<T> Instance2DTO(DTOModel model, ConstantValueTransformer<T> instance)
            {
                return new ConstantValueTransformerDTO<T> { Value = instance.Value, Persistent = instance.Persistent };
            }

            public ConstantValueTransformer<T> DTO2Instance(DTOModel model, ConstantValueTransformerDTO<T> dto, Func<ConstantValueTransformer<T>> activator)
            {
                return new ConstantValueTransformer<T>(dto.Value, dto.Persistent);
            }
        }
    }
}
