namespace FlipnoteDotNet.GUI.Forms.Controls
{
    partial class VisualSourceEditorControl
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
            this.Canvas = new FlipnoteDotNet.GUI.Forms.Controls.PaintDeviceCanvas();
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.CanvasViewLocation = new System.Drawing.Point(-471, -183);
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Canvas.Location = new System.Drawing.Point(26, 0);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(275, 205);
            this.Canvas.TabIndex = 4;
            // 
            // ToolStrip
            // 
            this.ToolStrip.Dock = System.Windows.Forms.DockStyle.Left;
            this.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.Size = new System.Drawing.Size(26, 205);
            this.ToolStrip.TabIndex = 3;
            this.ToolStrip.Text = "toolStrip1";
            // 
            // VisualSourceEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Canvas);
            this.Controls.Add(this.ToolStrip);
            this.Name = "VisualSourceEditorControl";
            this.Size = new System.Drawing.Size(301, 205);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PaintDeviceCanvas Canvas;
        private System.Windows.Forms.ToolStrip ToolStrip;
    }
}
