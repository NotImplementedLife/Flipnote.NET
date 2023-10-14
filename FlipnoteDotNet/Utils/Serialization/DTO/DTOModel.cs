using FlipnoteDotNet.Extensions;
using System;
using System.Collections.Generic;

namespace FlipnoteDotNet.Utils.Serialization.DTO
{
    public delegate object I2DTO_Delegate(DTOModel model, object source);
    public delegate object DTO2I_Delegate(DTOModel model, object source, Func<object> activator=null);

    public delegate TDTO I2DTO_Delegate<TI, TDTO>(DTOModel model, TI source);
    public delegate TI DTO2I_Delegate<TI, TDTO>(DTOModel model, TDTO source, Func<TI> activator = null);

    public class DTOModel
    {       
        private Dictionary<Type, Record> InstanceTypes = new Dictionary<Type, Record>();
        private Dictionary<Type, Record> TransferTypes = new Dictionary<Type, Record>();

        public DTOModel()
        {
            RegisterPrimitives(typeof(int), typeof(float), typeof(double), typeof(bool), typeof(string), typeof(byte[]));
        }

        private void RegisterPrimitive(Type t) => RegisterConversion(t, t, (m, _) => _, (m, _, a) => _);
        private void RegisterPrimitives(params Type[] types) => types.ForEach(RegisterPrimitive);


        public void RegisterConversion<TI, TDTO>(I2DTO_Delegate<TI, TDTO> i2dto, DTO2I_Delegate<TI, TDTO> dto2i)
        {
            RegisterConversion(typeof(TI), typeof(TDTO),
                (m, _) => i2dto(m, _.SafeConvert<TI>()),
                (m, _, a) => dto2i(m, _.SafeConvert<TDTO>(), () => a().SafeConvert<TI>()));
        }

        public void RegisterConversion(Type instanceType, Type transferType, I2DTO_Delegate i2dto, DTO2I_Delegate dto2i)
        {
            if (InstanceTypes.ContainsKey(instanceType))
                throw new ArgumentException("Instance type already registered");
            if (TransferTypes.ContainsKey(transferType))
                throw new ArgumentException("Transfer type already registered");
            var rec = new Record(instanceType, transferType, i2dto, dto2i);
            InstanceTypes.Add(instanceType, rec);
            TransferTypes.Add(transferType, rec);
        }

        public object GetDTO(object instance)
        {
            if (instance == null) throw new NullReferenceException();
            var type = instance.GetType();
            if (!InstanceTypes.TryGetValue(type, out var rec))
                throw new InvalidOperationException($"Cannot serialize object of type {type}");
            return rec.Instance2DTO(this, instance);
        }

        public object GetInstance(object dto, Func<object> activator = null)
        {
            if (dto == null) throw new NullReferenceException();
            var type = dto.GetType();
            if (!TransferTypes.TryGetValue(type, out var rec))
                throw new InvalidOperationException($"Cannot deserialize object of type {type}");
            return rec.DTO2Instance(this, dto, activator);
        }

        public T GetDTO<T>(object instance)
            => GetDTO(instance) is T result ? result : throw new InvalidCastException();

        public T GetInstance<T>(object dto)        
            => GetInstance(dto) is T result ? result : throw new InvalidCastException();        

        class Record
        {
            public Type InstanceType;
            public Type TransferType;
            public I2DTO_Delegate Instance2DTO;
            public DTO2I_Delegate DTO2Instance;

            public Record(Type instanceType, Type transferType, I2DTO_Delegate instance2DTO, DTO2I_Delegate dTO2Instance)
            {
                InstanceType = instanceType;
                TransferType = transferType;
                Instance2DTO = instance2DTO;
                DTO2Instance = dTO2Instance;
            }
        }
    }
}
