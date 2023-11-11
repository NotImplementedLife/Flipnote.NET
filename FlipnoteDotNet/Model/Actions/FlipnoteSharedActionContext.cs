using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Data.Manager;
using FlipnoteDotNet.Model.Entities;
using System;

namespace FlipnoteDotNet.Model.Actions
{
    public class FlipnoteSharedActionContext : ISharedActionContext
    {
        public int Timestamp { get; set; } = 0;

        public IEntityReference<FlipnoteProject> Project { get; set; }

        private IEntityReference<Entity> pSelectedEntity;
        public IEntityReference<Entity> SelectedEntity
        {
            get => pSelectedEntity;
            set
            {
                pSelectedEntity = value;
                SelectedEntityChanged?.Invoke(this, pSelectedEntity);
            }
        }

        private IEntityReference<Sequence> pSelectedSequence;
        public IEntityReference<Sequence> SelectedSequence
        {
            get => pSelectedSequence;
            set
            {
                pSelectedSequence = value;
                SelectedSequenceChanged?.Invoke(this, pSelectedSequence);
            }
        }

        private IEntityReference<Layer> pSelectedLayer;
        public IEntityReference<Layer> SelectedLayer
        {
            get => pSelectedLayer;
            set
            {
                pSelectedLayer = value;
                SelectedLayerChanged?.Invoke(this, pSelectedLayer);
            }
        }



        public event EventHandler<IEntityReference<Entity>> SelectedEntityChanged;
        public event EventHandler<IEntityReference<Sequence>> SelectedSequenceChanged;
        public event EventHandler<IEntityReference<Layer>> SelectedLayerChanged;

        public void Reset()
        {
            Timestamp = 0;
            Project = null;
            SelectedEntity = null;
        }
    }
}
