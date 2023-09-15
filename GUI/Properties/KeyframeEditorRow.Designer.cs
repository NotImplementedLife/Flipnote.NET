namespace FlipnoteDotNet.GUI.Properties
{
    partial class KeyFrameEditorRow
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
            this.components = new System.ComponentModel.Container();
            this.FrameLabel = new System.Windows.Forms.Label();
            this.FrameNoLabel = new System.Windows.Forms.Label();
            this.EditorPanel = new System.Windows.Forms.Panel();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.RemoveButtonTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // FrameLabel
            // 
            this.FrameLabel.AutoSize = true;
            this.FrameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FrameLabel.Location = new System.Drawing.Point(3, 7);
            this.FrameLabel.Name = "FrameLabel";
            this.FrameLabel.Size = new System.Drawing.Size(48, 15);
            this.FrameLabel.TabIndex = 0;
            this.FrameLabel.Text = "Frame";
            // 
            // FrameNoLabel
            // 
            this.FrameNoLabel.AutoSize = true;
            this.FrameNoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FrameNoLabel.Location = new System.Drawing.Point(51, 7);
            this.FrameNoLabel.Name = "FrameNoLabel";
            this.FrameNoLabel.Size = new System.Drawing.Size(15, 15);
            this.FrameNoLabel.TabIndex = 1;
            this.FrameNoLabel.Text = "0";
            // 
            // EditorPanel
            // 
            this.EditorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EditorPanel.AutoSize = true;
            this.EditorPanel.Location = new System.Drawing.Point(6, 33);
            this.EditorPanel.Name = "EditorPanel";
            this.EditorPanel.Size = new System.Drawing.Size(327, 12);
            this.EditorPanel.TabIndex = 2;
            // 
            // RemoveButton
            // 
            this.RemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveButton.BackgroundImage = global::FlipnoteDotNet.Properties.Resources.ic_remove;
            this.RemoveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.RemoveButton.FlatAppearance.BorderSize = 0;
            this.RemoveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveButton.Location = new System.Drawing.Point(309, 3);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(24, 24);
            this.RemoveButton.TabIndex = 3;
            this.RemoveButton.UseVisualStyleBackColor = true;
            // 
            // KeyFrameEditorRow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.EditorPanel);
            this.Controls.Add(this.FrameNoLabel);
            this.Controls.Add(this.FrameLabel);
            this.Name = "KeyFrameEditorRow";
            this.Size = new System.Drawing.Size(336, 48);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label FrameLabel;
        private System.Windows.Forms.Label FrameNoLabel;
        private System.Windows.Forms.Panel EditorPanel;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.ToolTip RemoveButtonTooltip;
    }
}
