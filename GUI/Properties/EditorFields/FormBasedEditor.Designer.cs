namespace FlipnoteDotNet.GUI.Properties.EditorFields
{
    partial class FormBasedEditor
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
            this.OpenFormButton = new System.Windows.Forms.Button();
            this.DisplayBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OpenFormButton
            // 
            this.OpenFormButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.OpenFormButton.Location = new System.Drawing.Point(192, 0);
            this.OpenFormButton.Name = "OpenFormButton";
            this.OpenFormButton.Size = new System.Drawing.Size(33, 25);
            this.OpenFormButton.TabIndex = 0;
            this.OpenFormButton.Text = "...";
            this.OpenFormButton.UseVisualStyleBackColor = true;
            this.OpenFormButton.Click += new System.EventHandler(this.OpenFormButton_Click);
            // 
            // DisplayBox
            // 
            this.DisplayBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DisplayBox.Location = new System.Drawing.Point(0, 0);
            this.DisplayBox.Multiline = true;
            this.DisplayBox.Name = "DisplayBox";
            this.DisplayBox.ReadOnly = true;
            this.DisplayBox.Size = new System.Drawing.Size(192, 25);
            this.DisplayBox.TabIndex = 1;
            // 
            // FormBasedEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DisplayBox);
            this.Controls.Add(this.OpenFormButton);
            this.Name = "FormBasedEditor";
            this.Size = new System.Drawing.Size(225, 25);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenFormButton;
        private System.Windows.Forms.TextBox DisplayBox;
    }
}
