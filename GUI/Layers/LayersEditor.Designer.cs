namespace FlipnoteDotNet.GUI.Layers
{
    partial class LayersEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddLayerButton = new System.Windows.Forms.ToolStripButton();
            this.RemoveLayerButton = new System.Windows.Forms.ToolStripButton();
            this.LayersListBox = new FlipnoteDotNet.GUI.Layers.LayersListBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddLayerButton,
            this.RemoveLayerButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(166, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // AddLayerButton
            // 
            this.AddLayerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddLayerButton.Image = global::FlipnoteDotNet.Properties.Resources.ic_layer_add;
            this.AddLayerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddLayerButton.Name = "AddLayerButton";
            this.AddLayerButton.Size = new System.Drawing.Size(23, 22);
            this.AddLayerButton.Text = "Add new layer";
            // 
            // RemoveLayerButton
            // 
            this.RemoveLayerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RemoveLayerButton.Enabled = false;
            this.RemoveLayerButton.Image = global::FlipnoteDotNet.Properties.Resources.ic_layer_remove;
            this.RemoveLayerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RemoveLayerButton.Name = "RemoveLayerButton";
            this.RemoveLayerButton.Size = new System.Drawing.Size(23, 22);
            this.RemoveLayerButton.Text = "Remove selected layer";
            // 
            // LayersListBox
            // 
            this.LayersListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayersListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.LayersListBox.FormattingEnabled = true;
            this.LayersListBox.ItemHeight = 50;
            this.LayersListBox.Location = new System.Drawing.Point(0, 25);
            this.LayersListBox.Name = "LayersListBox";
            this.LayersListBox.Size = new System.Drawing.Size(166, 134);
            this.LayersListBox.TabIndex = 1;
            this.LayersListBox.SelectedIndexChanged += new System.EventHandler(this.LayersListBox_SelectedIndexChanged);
            this.LayersListBox.DataSourceChanged += new System.EventHandler(this.LayersListBox_DataSourceChanged);
            // 
            // LayersEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LayersListBox);
            this.Controls.Add(this.toolStrip1);
            this.Name = "LayersEditor";
            this.Size = new System.Drawing.Size(166, 159);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        public LayersListBox LayersListBox;
        private System.Windows.Forms.ToolStripButton AddLayerButton;
        private System.Windows.Forms.ToolStripButton RemoveLayerButton;
    }
}
