using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.App.Service.AssetsActions;
using FlipnoteDotNet.App.Service.FlipnoteEditorActions;
using System.ComponentModel;

namespace FlipnoteDotNet.App.Service
{
    public class AssetsService
    {
        private readonly UndoStack UndoStack;
        private readonly IList<Asset> fAssets;
        private readonly BindingList<Asset> BindingAssets;
        private int IdCounter = 0;

        public AssetsService(UndoStack undoStack, IList<Asset> assets)
        {
            UndoStack = undoStack;
            fAssets = assets;
            BindingAssets = new BindingList<Asset>(fAssets);
        }

        public AssetsService(UndoStack undoStack)
        {
            UndoStack = undoStack;
            fAssets = new List<Asset>();
            BindingAssets = new BindingList<Asset>(fAssets);
        }

        public void LoadSavedAssets(Asset[] assets)
        {
            foreach (Asset asset in assets)
            {
                Assets.Add(asset);
                IdCounter = Math.Max(IdCounter, asset.Id + 1);
            }
        }

        public void AddAsset(Asset asset)
        {
            if(asset.Id==0)
            {
                asset.Id = ++IdCounter;
            }
            UndoStack.Do(new AddAsset(BindingAssets, asset));
        }

        public void RemoveAsset(Asset asset)
        {
            int index = BindingAssets.IndexOf(asset);
            if (index < 0) return;
            UndoStack.Do(new RemoveAsset(BindingAssets, asset, index));
        }

        public IList<Asset> Assets => BindingAssets;
    }
}
