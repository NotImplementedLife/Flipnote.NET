namespace FlipnoteDotNet.GUI.Controls
{
    partial class Expander
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
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.Content = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(243, 40);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.HeaderPanel_Paint);
            this.HeaderPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Header_MouseClick);
            this.HeaderPanel.MouseLeave += new System.EventHandler(this.HeaderPanel_MouseLeave);
            this.HeaderPanel.MouseHover += new System.EventHandler(this.HeaderPanel_MouseHover);
            // 
            // Content
            // 
            this.Content.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Content.Location = new System.Drawing.Point(0, 40);
            this.Content.MinimumSize = new System.Drawing.Size(0, 25);
            this.Content.Name = "Content";
            this.Content.Size = new System.Drawing.Size(243, 121);
            this.Content.TabIndex = 1;
            this.Content.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_Paint);
            this.Content.Resize += new System.EventHandler(this.Content_Resize);
            // 
            // Expander
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Content);
            this.Controls.Add(this.HeaderPanel);
            this.Name = "Expander";
            this.Size = new System.Drawing.Size(243, 161);
            this.Load += new System.EventHandler(this.Expander_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel HeaderPanel;
        public System.Windows.Forms.Panel Content;
    }
}
