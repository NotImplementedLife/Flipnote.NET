namespace FlipnoteDotNet.GUI.Properties
{
    partial class PropertyEditor
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
            this.dataGridViewColumn1 = new System.Windows.Forms.DataGridViewColumn();
            this.dataGridViewColumn2 = new System.Windows.Forms.DataGridViewColumn();
            this.dataGridViewColumn3 = new System.Windows.Forms.DataGridViewColumn();
            this.dataGridViewColumn4 = new System.Windows.Forms.DataGridViewColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SuspendLayout();
            // 
            // dataGridViewColumn1
            // 
            this.dataGridViewColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewColumn1.HeaderText = "Property";
            this.dataGridViewColumn1.Name = "dataGridViewColumn1";
            // 
            // dataGridViewColumn2
            // 
            this.dataGridViewColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewColumn2.HeaderText = "Value";
            this.dataGridViewColumn2.Name = "dataGridViewColumn2";
            // 
            // dataGridViewColumn3
            // 
            this.dataGridViewColumn3.FillWeight = 50F;
            this.dataGridViewColumn3.HeaderText = "Keyframes";
            this.dataGridViewColumn3.Name = "dataGridViewColumn3";
            this.dataGridViewColumn3.Width = 30;
            // 
            // dataGridViewColumn4
            // 
            this.dataGridViewColumn4.FillWeight = 50F;
            this.dataGridViewColumn4.HeaderText = "Effects";
            this.dataGridViewColumn4.Name = "dataGridViewColumn4";
            this.dataGridViewColumn4.Width = 30;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // PropertyEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Name = "PropertyEditor";
            this.Size = new System.Drawing.Size(280, 20);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewColumn dataGridViewColumn1;
        private System.Windows.Forms.DataGridViewColumn dataGridViewColumn2;
        private System.Windows.Forms.DataGridViewColumn dataGridViewColumn3;
        private System.Windows.Forms.DataGridViewColumn dataGridViewColumn4;
    }
}
