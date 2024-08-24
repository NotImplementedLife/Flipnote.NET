using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Data;
using System.Diagnostics;

namespace FlipnoteDotNet.App.Service.AssetsActions
{
    internal class AddAsset : IUndoableAction
    {
        private readonly IList<Asset> List;
        private readonly Asset Asset;

        public AddAsset(IList<Asset> list, Asset asset)
        {
            List = list;
            Asset = asset;
        }

        public void Do()
        {
            Debug.WriteLine($"Do? {Asset}");
            List.Add(Asset);
        }

        public void Undo()
        {
            List.RemoveAt(List.Count - 1);
        }
    }
}
