namespace FlipnoteDotNet.App.Forms
{
    partial class AssetImporterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            PreviewPanel = new Panel();
            LoadImageButton = new Button();
            OkButton = new Button();
            ImageOpenFileDialog = new OpenFileDialog();
            ResizeTrackBar = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)ResizeTrackBar).BeginInit();
            SuspendLayout();
            // 
            // PreviewPanel
            // 
            PreviewPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PreviewPanel.BorderStyle = BorderStyle.FixedSingle;
            PreviewPanel.Location = new Point(12, 12);
            PreviewPanel.Name = "PreviewPanel";
            PreviewPanel.Size = new Size(463, 302);
            PreviewPanel.TabIndex = 0;
            PreviewPanel.Paint += PreviewPanel_Paint;
            PreviewPanel.Resize += PreviewPanel_Resize;
            // 
            // LoadImageButton
            // 
            LoadImageButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            LoadImageButton.Location = new Point(481, 12);
            LoadImageButton.Name = "LoadImageButton";
            LoadImageButton.Size = new Size(100, 23);
            LoadImageButton.TabIndex = 1;
            LoadImageButton.Text = "Load Image";
            LoadImageButton.UseVisualStyleBackColor = true;
            LoadImageButton.Click += LoadImageButton_Click;
            // 
            // OkButton
            // 
            OkButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            OkButton.Location = new Point(481, 291);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(75, 23);
            OkButton.TabIndex = 2;
            OkButton.Text = "Ok";
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // ImageOpenFileDialog
            // 
            ImageOpenFileDialog.Filter = "Images|*.png;*.jpg;*.jpeg";
            // 
            // ResizeTrackBar
            // 
            ResizeTrackBar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ResizeTrackBar.LargeChange = 3;
            ResizeTrackBar.Location = new Point(481, 106);
            ResizeTrackBar.Maximum = 20;
            ResizeTrackBar.Name = "ResizeTrackBar";
            ResizeTrackBar.Size = new Size(100, 45);
            ResizeTrackBar.TabIndex = 3;
            ResizeTrackBar.ValueChanged += ResizeTrackBar_ValueChanged;
            // 
            // AssetImporterForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(588, 326);
            Controls.Add(ResizeTrackBar);
            Controls.Add(OkButton);
            Controls.Add(LoadImageButton);
            Controls.Add(PreviewPanel);
            Name = "AssetImporterForm";
            Text = "AssetImporterForm";
            ((System.ComponentModel.ISupportInitialize)ResizeTrackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel PreviewPanel;
        private Button LoadImageButton;
        private Button OkButton;
        private OpenFileDialog ImageOpenFileDialog;
        private TrackBar ResizeTrackBar;
    }
}