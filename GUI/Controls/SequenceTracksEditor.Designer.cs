namespace FlipnoteDotNet.GUI.Controls
{
    partial class SequenceTracksEditor
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
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.AddNewSequenceButton = new System.Windows.Forms.ToolStripButton();
            this.SequenceTracksViewer = new FlipnoteDotNet.GUI.Tracks.SequenceTracksViewer();
            this.ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolStrip
            // 
            this.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddNewSequenceButton});
            this.ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ToolStrip.Size = new System.Drawing.Size(518, 25);
            this.ToolStrip.TabIndex = 0;
            this.ToolStrip.Text = "toolStrip1";
            // 
            // AddNewSequenceButton
            // 
            this.AddNewSequenceButton.CheckOnClick = true;
            this.AddNewSequenceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddNewSequenceButton.Image = global::FlipnoteDotNet.Properties.Resources.ic_seq_add;
            this.AddNewSequenceButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddNewSequenceButton.Name = "AddNewSequenceButton";
            this.AddNewSequenceButton.Size = new System.Drawing.Size(23, 22);
            this.AddNewSequenceButton.Text = "Add new Sequence";
            this.AddNewSequenceButton.Click += new System.EventHandler(this.AddNewSequenceButton_Click);
            // 
            // SequenceTracksViewer
            // 
            this.SequenceTracksViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SequenceTracksViewer.Location = new System.Drawing.Point(0, 25);
            this.SequenceTracksViewer.Name = "SequenceTracksViewer";
            this.SequenceTracksViewer.Size = new System.Drawing.Size(518, 186);
            this.SequenceTracksViewer.SurfaceSize = new System.Drawing.Size(1100, 90);
            this.SequenceTracksViewer.TabIndex = 1;
            this.SequenceTracksViewer.TrackSignPosition = 0;
            this.SequenceTracksViewer.SequenceCreateModeEnded += new System.EventHandler(this.SequenceTracksViewer_SequenceCreateModeEnded);
            this.SequenceTracksViewer.CurrentFrameChanged += new System.EventHandler(this.SequenceTracksViewer_CurrentFrameChanged);
            this.SequenceTracksViewer.SelectedElementChanged += new System.EventHandler(this.SequenceTracksViewer_SelectedElementChanged);
            // 
            // SequenceTracksEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SequenceTracksViewer);
            this.Controls.Add(this.ToolStrip);
            this.Name = "SequenceTracksEditor";
            this.Size = new System.Drawing.Size(518, 211);
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ToolStrip ToolStrip;
        private Tracks.SequenceTracksViewer SequenceTracksViewer;
        private System.Windows.Forms.ToolStripButton AddNewSequenceButton;
    }
}
