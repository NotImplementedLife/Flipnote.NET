namespace FlipnoteDotNet.GUI.Forms.LayerCreators
{
    partial class StaticImageLayerCreatorForm
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
            this.VisualSourceEditorControl = new FlipnoteDotNet.GUI.Forms.Controls.VisualSourceEditorControl();
            this.AddLayerButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // VisualSourceEditorControl
            // 
            this.VisualSourceEditorControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VisualSourceEditorControl.Location = new System.Drawing.Point(3, 16);
            this.VisualSourceEditorControl.Name = "VisualSourceEditorControl";
            this.VisualSourceEditorControl.Size = new System.Drawing.Size(433, 240);
            this.VisualSourceEditorControl.TabIndex = 0;
            // 
            // AddLayerButton
            // 
            this.AddLayerButton.Location = new System.Drawing.Point(376, 277);
            this.AddLayerButton.Name = "AddLayerButton";
            this.AddLayerButton.Size = new System.Drawing.Size(75, 23);
            this.AddLayerButton.TabIndex = 1;
            this.AddLayerButton.Text = "Add Layer";
            this.AddLayerButton.UseVisualStyleBackColor = true;
            this.AddLayerButton.Click += new System.EventHandler(this.AddLayerButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.VisualSourceEditorControl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(439, 259);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Visual";
            // 
            // StaticImageLayerCreatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 312);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.AddLayerButton);
            this.Name = "StaticImageLayerCreatorForm";
            this.Text = "Create Static Image Layer...";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.VisualSourceEditorControl VisualSourceEditorControl;
        private System.Windows.Forms.Button AddLayerButton;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}