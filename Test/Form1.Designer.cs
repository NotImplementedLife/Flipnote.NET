namespace Test
{
    partial class Form1
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
            this.visualComponentsScene1 = new FlipnoteDotNet.VisualComponentsEditor.VisualComponentsScene();
            this.SuspendLayout();
            // 
            // visualComponentsScene1
            // 
            this.visualComponentsScene1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.visualComponentsScene1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visualComponentsScene1.Location = new System.Drawing.Point(0, 0);
            this.visualComponentsScene1.Name = "visualComponentsScene1";
            this.visualComponentsScene1.Size = new System.Drawing.Size(576, 280);
            this.visualComponentsScene1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 280);
            this.Controls.Add(this.visualComponentsScene1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private FlipnoteDotNet.VisualComponentsEditor.VisualComponentsScene visualComponentsScene1;
    }
}

