namespace FlipnoteDotNet.GUI.Properties
{
    partial class PropertyRow
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
            this.Label = new System.Windows.Forms.Label();
            this.EditorPanel = new System.Windows.Forms.Panel();
            this.EffectsButton = new System.Windows.Forms.Button();
            this.KeyframesButton = new System.Windows.Forms.Button();
            this.EffectsTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.KeyframesTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // Label
            // 
            this.Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label.Location = new System.Drawing.Point(0, 0);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(75, 27);
            this.Label.TabIndex = 0;
            this.Label.Text = "label1";
            this.Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EditorPanel
            // 
            this.EditorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EditorPanel.Location = new System.Drawing.Point(75, 0);
            this.EditorPanel.Name = "EditorPanel";
            this.EditorPanel.Size = new System.Drawing.Size(146, 27);
            this.EditorPanel.TabIndex = 1;
            // 
            // EffectsButton
            // 
            this.EffectsButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.EffectsButton.BackgroundImage = global::FlipnoteDotNet.Properties.Resources.ic_effects;
            this.EffectsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.EffectsButton.Location = new System.Drawing.Point(257, 1);
            this.EffectsButton.Name = "EffectsButton";
            this.EffectsButton.Size = new System.Drawing.Size(24, 24);
            this.EffectsButton.TabIndex = 3;
            this.EffectsButton.UseVisualStyleBackColor = true;
            // 
            // KeyframesButton
            // 
            this.KeyframesButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.KeyframesButton.BackgroundImage = global::FlipnoteDotNet.Properties.Resources.ic_keyframes;
            this.KeyframesButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.KeyframesButton.Location = new System.Drawing.Point(227, 1);
            this.KeyframesButton.Name = "KeyframesButton";
            this.KeyframesButton.Size = new System.Drawing.Size(24, 24);
            this.KeyframesButton.TabIndex = 2;
            this.KeyframesButton.UseVisualStyleBackColor = true;
            // 
            // PropertyRow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EffectsButton);
            this.Controls.Add(this.KeyframesButton);
            this.Controls.Add(this.EditorPanel);
            this.Controls.Add(this.Label);
            this.Name = "PropertyRow";
            this.Size = new System.Drawing.Size(281, 27);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Label;
        private System.Windows.Forms.Panel EditorPanel;
        private System.Windows.Forms.Button KeyframesButton;
        private System.Windows.Forms.Button EffectsButton;
        private System.Windows.Forms.ToolTip EffectsTooltip;
        private System.Windows.Forms.ToolTip KeyframesTooltip;
    }
}
