namespace G_TypeConsole
{
    partial class GenSettingsWindow
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
            this.Editor = new System.Windows.Forms.RichTextBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.Body = new System.Windows.Forms.SplitContainer();
            this.HelpLink = new System.Windows.Forms.LinkLabel();
            this.PreviewButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Body)).BeginInit();
            this.Body.Panel1.SuspendLayout();
            this.Body.Panel2.SuspendLayout();
            this.Body.SuspendLayout();
            this.SuspendLayout();
            // 
            // Editor
            // 
            this.Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Editor.Font = new System.Drawing.Font("Lucida Console", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Editor.Location = new System.Drawing.Point(0, 0);
            this.Editor.Name = "Editor";
            this.Editor.Size = new System.Drawing.Size(694, 518);
            this.Editor.TabIndex = 0;
            this.Editor.Text = "";
            this.Editor.TextChanged += new System.EventHandler(this.Editor_TextChanged);
            this.Editor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Editor_KeyDown);
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.Location = new System.Drawing.Point(2, 32);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(68, 23);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // Body
            // 
            this.Body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Body.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.Body.Location = new System.Drawing.Point(0, 0);
            this.Body.Name = "Body";
            // 
            // Body.Panel1
            // 
            this.Body.Panel1.Controls.Add(this.Editor);
            // 
            // Body.Panel2
            // 
            this.Body.Panel2.Controls.Add(this.HelpLink);
            this.Body.Panel2.Controls.Add(this.PreviewButton);
            this.Body.Panel2.Controls.Add(this.OkButton);
            this.Body.Size = new System.Drawing.Size(775, 518);
            this.Body.SplitterDistance = 694;
            this.Body.TabIndex = 2;
            // 
            // HelpLink
            // 
            this.HelpLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.HelpLink.AutoSize = true;
            this.HelpLink.Location = new System.Drawing.Point(3, 496);
            this.HelpLink.Name = "HelpLink";
            this.HelpLink.Size = new System.Drawing.Size(62, 13);
            this.HelpLink.TabIndex = 2;
            this.HelpLink.TabStop = true;
            this.HelpLink.Text = "Need help?";
            // 
            // PreviewButton
            // 
            this.PreviewButton.Location = new System.Drawing.Point(2, 3);
            this.PreviewButton.Name = "PreviewButton";
            this.PreviewButton.Size = new System.Drawing.Size(68, 23);
            this.PreviewButton.TabIndex = 0;
            this.PreviewButton.Text = "Preview";
            this.PreviewButton.UseVisualStyleBackColor = true;
            // 
            // GenSettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 518);
            this.Controls.Add(this.Body);
            this.Name = "GenSettingsWindow";
            this.Text = "GenSettingsWindow";
            this.Body.Panel1.ResumeLayout(false);
            this.Body.Panel2.ResumeLayout(false);
            this.Body.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Body)).EndInit();
            this.Body.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox Editor;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.SplitContainer Body;
        private System.Windows.Forms.LinkLabel HelpLink;
        private System.Windows.Forms.Button PreviewButton;
    }
}