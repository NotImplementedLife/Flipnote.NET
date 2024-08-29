using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Data;

namespace FlipnoteDotNet.App.Service.FlipnoteEditorActions
{
    internal class RemoveAsset : IUndoableAction
    {
        private readonly IList<Asset> List;
        private readonly Asset Asset;
        private readonly int Index;

        public RemoveAsset(IList<Asset> list, Asset asset, int index)
        {
            List = list;
            Asset = asset;
            Index = index;
        }

        public void Do()
        {
            List.RemoveAt(Index);
        }

        public void Undo()
        {
            List.Insert(Index, Asset);
        }
    }
}
