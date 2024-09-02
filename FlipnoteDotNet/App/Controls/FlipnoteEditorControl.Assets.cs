using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.App.Forms;
using FlipnoteDotNet.App.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.App.Controls
{
    public partial class FlipnoteEditorControl
    {
        AssetsService AssetsService;

        private void InitializeAssets(AssetsService assetsService)
        {
            AssetsService = assetsService;
            AssetsListView.TargetList = AssetsService.Assets;            
            AssetsListView.ItemSelectionChanged += AssetsListView_ItemSelectionChanged;

            AssetAddButton.Click += AssetAddButton_Click;
            AssetRemoveButton.Click += AssetRemoveButton_Click;

            AssetRemoveButton.Enabled = AssetsListView.SelectedItems.Count != 0;
        }

        private void AssetsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            AssetRemoveButton.Enabled = AssetsListView.SelectedItems.Count != 0;
        }        

        private void AssetAddButton_Click(object sender, EventArgs e)
        {
            using (var assetImporterForm = new AssetImporterForm())
            {
                assetImporterForm.FrameConfig = FramesManager.GetCurrentFrame().FrameConfig;
                if (assetImporterForm.ShowDialog() == DialogResult.OK)
                {
                    AssetsService.AddAsset(assetImporterForm.ResultAsset);
                }
            }
        }

        private void AssetRemoveButton_Click(object sender, EventArgs e)
        {
            AssetsService.RemoveAsset(AssetsListView.SelectedItems[0].Tag as Asset);
        }
    }
}
