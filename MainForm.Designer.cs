using FlipnoteDotNet.Constants;

namespace FlipnoteDotNet
{    
    partial class MainForm
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
            FlipnoteDotNet.Data.SequenceManager sequenceManager1 = new FlipnoteDotNet.Data.SequenceManager();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.FormMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.MainContainer = new System.Windows.Forms.SplitContainer();
            this.LeftContainer = new System.Windows.Forms.SplitContainer();
            this.RightContainer = new System.Windows.Forms.SplitContainer();
            this.Canvas = new FlipnoteDotNet.GUI.Canvas.CanvasSpaceControl();
            this.SequenceTrackViewer = new FlipnoteDotNet.GUI.Tracks.SequenceTracksViewer();
            this.FormMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).BeginInit();
            this.MainContainer.Panel1.SuspendLayout();
            this.MainContainer.Panel2.SuspendLayout();
            this.MainContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LeftContainer)).BeginInit();
            this.LeftContainer.Panel1.SuspendLayout();
            this.LeftContainer.Panel2.SuspendLayout();
            this.LeftContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RightContainer)).BeginInit();
            this.RightContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormMenu
            // 
            this.FormMenu.BackColor = System.Drawing.Color.White;
            this.FormMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.FormMenu.Location = new System.Drawing.Point(0, 0);
            this.FormMenu.Name = "FormMenu";
            this.FormMenu.Size = new System.Drawing.Size(684, 24);
            this.FormMenu.TabIndex = 2;
            this.FormMenu.Text = "menuStrip1";
            this.FormMenu.Paint += new System.Windows.Forms.PaintEventHandler(this.BackgroundControlPaint);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // ToolStrip
            // 
            this.ToolStrip.BackColor = System.Drawing.Color.White;
            this.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.ToolStrip.Location = new System.Drawing.Point(0, 24);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ToolStrip.Size = new System.Drawing.Size(684, 25);
            this.ToolStrip.TabIndex = 3;
            this.ToolStrip.Text = "toolStrip1";
            this.ToolStrip.Paint += new System.Windows.Forms.PaintEventHandler(this.BackgroundControlPaint);
            // 
            // MainContainer
            // 
            this.MainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainContainer.Location = new System.Drawing.Point(0, 49);
            this.MainContainer.Name = "MainContainer";
            // 
            // MainContainer.Panel1
            // 
            this.MainContainer.Panel1.Controls.Add(this.LeftContainer);
            // 
            // MainContainer.Panel2
            // 
            this.MainContainer.Panel2.Controls.Add(this.RightContainer);
            this.MainContainer.Size = new System.Drawing.Size(684, 412);
            this.MainContainer.SplitterDistance = 491;
            this.MainContainer.TabIndex = 1;
            this.MainContainer.TabStop = false;
            this.MainContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.BackgroundControlPaint);
            // 
            // LeftContainer
            // 
            this.LeftContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftContainer.Location = new System.Drawing.Point(0, 0);
            this.LeftContainer.Name = "LeftContainer";
            this.LeftContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // LeftContainer.Panel1
            // 
            this.LeftContainer.Panel1.Controls.Add(this.Canvas);
            this.LeftContainer.Panel1.Padding = new System.Windows.Forms.Padding(2);
            // 
            // LeftContainer.Panel2
            // 
            this.LeftContainer.Panel2.Controls.Add(this.SequenceTrackViewer);
            this.LeftContainer.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.BackgroundControlPaint);
            this.LeftContainer.Panel2MinSize = 200;
            this.LeftContainer.Size = new System.Drawing.Size(491, 412);
            this.LeftContainer.SplitterDistance = 208;
            this.LeftContainer.TabIndex = 0;
            this.LeftContainer.TabStop = false;
            this.LeftContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.BackgroundControlPaint);
            // 
            // RightContainer
            // 
            this.RightContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightContainer.Location = new System.Drawing.Point(0, 0);
            this.RightContainer.Name = "RightContainer";
            this.RightContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // RightContainer.Panel1
            // 
            this.RightContainer.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.BackgroundControlPaint);
            // 
            // RightContainer.Panel2
            // 
            this.RightContainer.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.BackgroundControlPaint);
            this.RightContainer.Size = new System.Drawing.Size(189, 412);
            this.RightContainer.SplitterDistance = 165;
            this.RightContainer.SplitterWidth = 2;
            this.RightContainer.TabIndex = 0;
            this.RightContainer.TabStop = false;
            // 
            // Canvas
            // 
            this.Canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Canvas.CanvasViewLocation = new System.Drawing.Point(4782, -4300);
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Canvas.Location = new System.Drawing.Point(2, 2);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(487, 204);
            this.Canvas.TabIndex = 0;
            // 
            // SequenceTrackViewer
            // 
            this.SequenceTrackViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SequenceTrackViewer.Location = new System.Drawing.Point(0, 0);
            this.SequenceTrackViewer.Name = "SequenceTrackViewer";
            this.SequenceTrackViewer.SequenceManager = sequenceManager1;
            this.SequenceTrackViewer.Size = new System.Drawing.Size(491, 200);
            this.SequenceTrackViewer.SurfaceSize = new System.Drawing.Size(6700, 50);
            this.SequenceTrackViewer.TabIndex = 0;
            this.SequenceTrackViewer.Zoom = 660;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.MainContainer);
            this.Controls.Add(this.ToolStrip);
            this.Controls.Add(this.FormMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.FormMenu;
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "MainForm";
            this.Text = "Flipnote.NET";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.BackgroundControlPaint);
            this.FormMenu.ResumeLayout(false);
            this.FormMenu.PerformLayout();
            this.MainContainer.Panel1.ResumeLayout(false);
            this.MainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).EndInit();
            this.MainContainer.ResumeLayout(false);
            this.LeftContainer.Panel1.ResumeLayout(false);
            this.LeftContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LeftContainer)).EndInit();
            this.LeftContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RightContainer)).EndInit();
            this.RightContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GUI.Canvas.CanvasSpaceControl Canvas;
        private System.Windows.Forms.SplitContainer MainContainer;
        private System.Windows.Forms.SplitContainer LeftContainer;
        private System.Windows.Forms.SplitContainer RightContainer;
        private System.Windows.Forms.MenuStrip FormMenu;
        private System.Windows.Forms.ToolStrip ToolStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private GUI.Tracks.SequenceTracksViewer SequenceTrackViewer;
    }
}

