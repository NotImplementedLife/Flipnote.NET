namespace FlipnoteDotNet.GUI.Controls
{
    partial class ScrollView
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
            this.ScrollContainer = new System.Windows.Forms.Panel();
            this.HScrollBar = new System.Windows.Forms.HScrollBar();
            this.VScrollBar = new System.Windows.Forms.VScrollBar();
            this.VScrollbarContainer = new System.Windows.Forms.Panel();
            this.CornerFiller = new System.Windows.Forms.Panel();
            this.VScrollbarContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // Container
            // 
            this.ScrollContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScrollContainer.Location = new System.Drawing.Point(0, 0);
            this.ScrollContainer.Name = "Container";
            this.ScrollContainer.Size = new System.Drawing.Size(499, 171);
            this.ScrollContainer.TabIndex = 6;
            this.ScrollContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.Container_Paint);
            this.ScrollContainer.Resize += new System.EventHandler(this.Container_Resize);
            // 
            // HScrollBar
            // 
            this.HScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.HScrollBar.Location = new System.Drawing.Point(0, 171);
            this.HScrollBar.Name = "HScrollBar";
            this.HScrollBar.Size = new System.Drawing.Size(499, 17);
            this.HScrollBar.TabIndex = 5;
            this.HScrollBar.ValueChanged += new System.EventHandler(this.ScrollBar_ValueChanged);
            // 
            // VScrollBar
            // 
            this.VScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VScrollBar.Location = new System.Drawing.Point(0, 0);
            this.VScrollBar.Name = "VScrollBar";
            this.VScrollBar.Size = new System.Drawing.Size(17, 171);
            this.VScrollBar.TabIndex = 1;
            this.VScrollBar.ValueChanged += new System.EventHandler(this.ScrollBar_ValueChanged);
            // 
            // VScrollbarContainer
            // 
            this.VScrollbarContainer.Controls.Add(this.VScrollBar);
            this.VScrollbarContainer.Controls.Add(this.CornerFiller);
            this.VScrollbarContainer.Dock = System.Windows.Forms.DockStyle.Right;
            this.VScrollbarContainer.Location = new System.Drawing.Point(499, 0);
            this.VScrollbarContainer.Name = "VScrollbarContainer";
            this.VScrollbarContainer.Size = new System.Drawing.Size(17, 188);
            this.VScrollbarContainer.TabIndex = 7;
            // 
            // CornerFiller
            // 
            this.CornerFiller.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CornerFiller.Location = new System.Drawing.Point(0, 171);
            this.CornerFiller.Name = "CornerFiller";
            this.CornerFiller.Size = new System.Drawing.Size(17, 17);
            this.CornerFiller.TabIndex = 3;
            // 
            // ScrollView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ScrollContainer);
            this.Controls.Add(this.HScrollBar);
            this.Controls.Add(this.VScrollbarContainer);
            this.Name = "ScrollView";
            this.Size = new System.Drawing.Size(516, 188);
            this.Resize += new System.EventHandler(this.ScrollView_Resize);
            this.VScrollbarContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Panel ScrollContainer;
        private System.Windows.Forms.HScrollBar HScrollBar;
        private System.Windows.Forms.VScrollBar VScrollBar;
        private System.Windows.Forms.Panel VScrollbarContainer;
        private System.Windows.Forms.Panel CornerFiller;
    }
}
