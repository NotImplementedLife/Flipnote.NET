namespace FlipnoteDotNet.GUI.Forms.Controls
{
    partial class PenPreviewPicker
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
            this.Pen1Box = new System.Windows.Forms.RadioButton();
            this.Colors1Group = new System.Windows.Forms.Panel();
            this.BlueColor1Box = new System.Windows.Forms.RadioButton();
            this.RedColor1Box = new System.Windows.Forms.RadioButton();
            this.InversePaperColor1Box = new System.Windows.Forms.RadioButton();
            this.Colors2Group = new System.Windows.Forms.Panel();
            this.BlueColor2Box = new System.Windows.Forms.RadioButton();
            this.RedColor2Box = new System.Windows.Forms.RadioButton();
            this.InversePaperColor2Box = new System.Windows.Forms.RadioButton();
            this.Pen2Box = new System.Windows.Forms.RadioButton();
            this.Colors1Group.SuspendLayout();
            this.Colors2Group.SuspendLayout();
            this.SuspendLayout();
            // 
            // Pen1Box
            // 
            this.Pen1Box.AutoSize = true;
            this.Pen1Box.Location = new System.Drawing.Point(3, 7);
            this.Pen1Box.Name = "Pen1Box";
            this.Pen1Box.Size = new System.Drawing.Size(53, 17);
            this.Pen1Box.TabIndex = 0;
            this.Pen1Box.TabStop = true;
            this.Pen1Box.Text = "Pen 1";
            this.Pen1Box.UseVisualStyleBackColor = true;
            this.Pen1Box.CheckedChanged += new System.EventHandler(this.Pen1Box_CheckedChanged);
            // 
            // Colors1Group
            // 
            this.Colors1Group.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Colors1Group.Controls.Add(this.BlueColor1Box);
            this.Colors1Group.Controls.Add(this.RedColor1Box);
            this.Colors1Group.Controls.Add(this.InversePaperColor1Box);
            this.Colors1Group.Location = new System.Drawing.Point(94, 0);
            this.Colors1Group.Name = "Colors1Group";
            this.Colors1Group.Size = new System.Drawing.Size(173, 30);
            this.Colors1Group.TabIndex = 1;
            // 
            // BlueColor1Box
            // 
            this.BlueColor1Box.Appearance = System.Windows.Forms.Appearance.Button;
            this.BlueColor1Box.Location = new System.Drawing.Point(63, 3);
            this.BlueColor1Box.Name = "BlueColor1Box";
            this.BlueColor1Box.Size = new System.Drawing.Size(24, 24);
            this.BlueColor1Box.TabIndex = 2;
            this.BlueColor1Box.TabStop = true;
            this.BlueColor1Box.UseVisualStyleBackColor = true;
            // 
            // RedColor1Box
            // 
            this.RedColor1Box.Appearance = System.Windows.Forms.Appearance.Button;
            this.RedColor1Box.Location = new System.Drawing.Point(33, 3);
            this.RedColor1Box.Name = "RedColor1Box";
            this.RedColor1Box.Size = new System.Drawing.Size(24, 24);
            this.RedColor1Box.TabIndex = 1;
            this.RedColor1Box.TabStop = true;
            this.RedColor1Box.UseVisualStyleBackColor = true;
            // 
            // InversePaperColor1Box
            // 
            this.InversePaperColor1Box.Appearance = System.Windows.Forms.Appearance.Button;
            this.InversePaperColor1Box.Location = new System.Drawing.Point(3, 3);
            this.InversePaperColor1Box.Name = "InversePaperColor1Box";
            this.InversePaperColor1Box.Size = new System.Drawing.Size(24, 24);
            this.InversePaperColor1Box.TabIndex = 0;
            this.InversePaperColor1Box.TabStop = true;
            this.InversePaperColor1Box.UseVisualStyleBackColor = true;
            this.InversePaperColor1Box.Paint += new System.Windows.Forms.PaintEventHandler(this.InversePaperColorBox_Paint);
            // 
            // Colors2Group
            // 
            this.Colors2Group.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Colors2Group.Controls.Add(this.BlueColor2Box);
            this.Colors2Group.Controls.Add(this.RedColor2Box);
            this.Colors2Group.Controls.Add(this.InversePaperColor2Box);
            this.Colors2Group.Location = new System.Drawing.Point(94, 32);
            this.Colors2Group.Name = "Colors2Group";
            this.Colors2Group.Size = new System.Drawing.Size(173, 30);
            this.Colors2Group.TabIndex = 3;
            // 
            // BlueColor2Box
            // 
            this.BlueColor2Box.Appearance = System.Windows.Forms.Appearance.Button;
            this.BlueColor2Box.Location = new System.Drawing.Point(63, 3);
            this.BlueColor2Box.Name = "BlueColor2Box";
            this.BlueColor2Box.Size = new System.Drawing.Size(24, 24);
            this.BlueColor2Box.TabIndex = 2;
            this.BlueColor2Box.TabStop = true;
            this.BlueColor2Box.UseVisualStyleBackColor = true;
            // 
            // RedColor2Box
            // 
            this.RedColor2Box.Appearance = System.Windows.Forms.Appearance.Button;
            this.RedColor2Box.Location = new System.Drawing.Point(33, 3);
            this.RedColor2Box.Name = "RedColor2Box";
            this.RedColor2Box.Size = new System.Drawing.Size(24, 24);
            this.RedColor2Box.TabIndex = 1;
            this.RedColor2Box.TabStop = true;
            this.RedColor2Box.UseVisualStyleBackColor = true;
            // 
            // InversePaperColor2Box
            // 
            this.InversePaperColor2Box.Appearance = System.Windows.Forms.Appearance.Button;
            this.InversePaperColor2Box.Location = new System.Drawing.Point(3, 3);
            this.InversePaperColor2Box.Name = "InversePaperColor2Box";
            this.InversePaperColor2Box.Size = new System.Drawing.Size(24, 24);
            this.InversePaperColor2Box.TabIndex = 0;
            this.InversePaperColor2Box.TabStop = true;
            this.InversePaperColor2Box.UseVisualStyleBackColor = true;
            this.InversePaperColor2Box.Paint += new System.Windows.Forms.PaintEventHandler(this.InversePaperColorBox_Paint);
            // 
            // Pen2Box
            // 
            this.Pen2Box.AutoSize = true;
            this.Pen2Box.Location = new System.Drawing.Point(3, 39);
            this.Pen2Box.Name = "Pen2Box";
            this.Pen2Box.Size = new System.Drawing.Size(53, 17);
            this.Pen2Box.TabIndex = 4;
            this.Pen2Box.TabStop = true;
            this.Pen2Box.Text = "Pen 2";
            this.Pen2Box.UseVisualStyleBackColor = true;
            this.Pen2Box.CheckedChanged += new System.EventHandler(this.Pen2Box_CheckedChanged);
            // 
            // PenPreviewPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Pen2Box);
            this.Controls.Add(this.Colors2Group);
            this.Controls.Add(this.Colors1Group);
            this.Controls.Add(this.Pen1Box);
            this.Name = "PenPreviewPicker";
            this.Size = new System.Drawing.Size(267, 65);
            this.Colors1Group.ResumeLayout(false);
            this.Colors2Group.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton Pen1Box;
        private System.Windows.Forms.Panel Colors1Group;
        private System.Windows.Forms.RadioButton InversePaperColor1Box;
        private System.Windows.Forms.RadioButton BlueColor1Box;
        private System.Windows.Forms.RadioButton RedColor1Box;
        private System.Windows.Forms.Panel Colors2Group;
        private System.Windows.Forms.RadioButton BlueColor2Box;
        private System.Windows.Forms.RadioButton RedColor2Box;
        private System.Windows.Forms.RadioButton InversePaperColor2Box;
        private System.Windows.Forms.RadioButton Pen2Box;
    }
}
