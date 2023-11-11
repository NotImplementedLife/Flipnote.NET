using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Data.Manager;
using FlipnoteDotNet.Model.Entities;
using System;
using System.Linq;

namespace FlipnoteDotNet.Model.Actions
{
    public class AddLayerAction : DatabaseAction<FlipnoteSharedActionContext>
    {
        private readonly Type LayerType;        
        private int AddedLayerId = -1;
        private Action Callback;
        public AddLayerAction(Type layerType, Action callback)
        {
            if (layerType == null) 
                throw new ArgumentNullException(nameof(layerType));
            if (!layerType.IsSubclassOf(typeof(Layer)))
                throw new ArgumentException("Argument must be a subclass of Layer", nameof(layerType));
            LayerType = layerType;
            Callback = callback;
        }

        public override void Do(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {
            var seq = GetSelectedSequence(ctx);
            var layer = AddedLayerId < 0 ? db.Create(LayerType) as IEntityReference<Layer>
                : db.Create(LayerType, AddedLayerId) as IEntityReference<Layer>;
            AddedLayerId = layer.Id;
            seq.Entity.Layers.Add(layer);
            seq.Commit();
            Callback?.Invoke();
        }

        public override void Undo(EntityDatabase db, FlipnoteSharedActionContext ctx)
        {
            var seq = GetSelectedSequence(ctx);
            var layer = seq.Entity.Layers.Where(_ => _.Id == AddedLayerId).First();
            seq.Entity.Layers.Remove(layer);
            db.RemoveById(AddedLayerId);            
            seq.Commit();
            Callback?.Invoke();
        }

        private static IEntityReference<Sequence> GetSelectedSequence(FlipnoteSharedActionContext ctx)
            => ctx.SelectedSequence
                ?? throw new InvalidOperationException("A Sequence must be selected before adding a Layer");
    }
}
