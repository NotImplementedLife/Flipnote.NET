using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.Utils.GUI;
using System.ComponentModel;

namespace FlipnoteDotNet.App.Controls
{
    internal class AssetsListView : BoundedListView<Asset>
    {
        public AssetsListView()
        {
            var imageList = new ImageList
            {
                ImageSize = new Size(64, 64)
            };
            LargeImageList = imageList;
        }

        public void ResetList()
        {
            SelectedIndices.Clear();
            TargetList = new List<Asset>();
            LargeImageList.Images.Clear();
        }

        protected override ListViewItem CreateListViewItem(Asset value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            var imageKey = value.Id.ToString();
            LargeImageList.Images.Add(imageKey, Icon.FromHandle(value.Thumbnail.GetHicon()));
            return new ListViewItem(value.Name?.ToString() ?? "") { Tag = value, ImageKey = imageKey };
        }

        protected override void OnItemRemoved(int index)
        {
            var removedItem = Items[index].Tag as Asset;
            LargeImageList.Images.RemoveByKey(removedItem.Id.ToString());
            base.OnItemRemoved(index);
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);
            var asset = (e.Item as ListViewItem).Tag as Asset;
            var dragObject = new DragDropObject<Asset>(asset);
            DoDragDrop(dragObject, DragDropEffects.Copy | DragDropEffects.Move);
        }
    }
}
