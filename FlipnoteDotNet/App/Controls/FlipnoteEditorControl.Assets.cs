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
            AssetImportButton.Click += AssetImportButton_Click;
        }

        private void AssetImportButton_Click(object sender, EventArgs e)
        {
            var assetImporterForm = new AssetImporterForm();
            assetImporterForm.FrameConfig = FramesManager.GetCurrentFrame().FrameConfig;
            if (assetImporterForm.ShowDialog() == DialogResult.OK)
            {
                AssetsService.AddAsset(assetImporterForm.ResultAsset);                
            }
            assetImporterForm.Dispose();
        }
    }
}
