using FlipnoteDotNet.Utils.Temporal;
using System;

namespace FlipnoteDotNet.Utils.Serialization.DTO
{
    public interface IDTOConverter<TI, TDTO>
    {
        TDTO Instance2DTO(DTOModel model, TI instance);
        TI DTO2Instance(DTOModel model, TDTO dto, Func<TI> activator);
    }
}
