namespace FlipnoteDotNet.GUI.Forms
{
    partial class VisualSourceEditorForm
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
            this.Canvas = new FlipnoteDotNet.GUI.Canvas.CanvasSpaceControl();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.CanvasViewLocation = new System.Drawing.Point(-3, -4);
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Canvas.Location = new System.Drawing.Point(0, 0);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(440, 267);
            this.Canvas.TabIndex = 0;
            // 
            // VisualSourceEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 267);
            this.Controls.Add(this.Canvas);
            this.Name = "VisualSourceEditorForm";
            this.Text = "VisualSourceEditorForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Canvas.CanvasSpaceControl Canvas;
    }
}