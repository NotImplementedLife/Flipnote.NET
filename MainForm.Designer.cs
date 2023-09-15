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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.FormMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.MainContainer = new System.Windows.Forms.SplitContainer();
            this.LeftContainer = new System.Windows.Forms.SplitContainer();
            this.RightContainer = new System.Windows.Forms.SplitContainer();
            this.SelectedElementLabel = new System.Windows.Forms.Label();
            this.ContentPanel = new System.Windows.Forms.Panel();
            this.Canvas = new FlipnoteDotNet.GUI.Canvas.CanvasSpaceControl();
            this.SequenceTracksEditor = new FlipnoteDotNet.GUI.Controls.SequenceTracksEditor();
            this.LayersEditor = new FlipnoteDotNet.GUI.Layers.LayersEditor();
            this.KeyframesExpander = new FlipnoteDotNet.GUI.Controls.Expander();
            this.KeyFramesEditor = new FlipnoteDotNet.GUI.Properties.KeyFramesEditor();
            this.PropertiesExpander = new FlipnoteDotNet.GUI.Controls.Expander();
            this.PropertyEditor = new FlipnoteDotNet.GUI.Properties.PropertyEditor();
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
            this.RightContainer.Panel1.SuspendLayout();
            this.RightContainer.Panel2.SuspendLayout();
            this.RightContainer.SuspendLayout();
            this.KeyframesExpander.ContentsPanel.SuspendLayout();
            this.KeyframesExpander.SuspendLayout();
            this.PropertiesExpander.ContentsPanel.SuspendLayout();
            this.PropertiesExpander.SuspendLayout();
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
            this.LeftContainer.Panel2.Controls.Add(this.SequenceTracksEditor);
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
            this.RightContainer.Panel1.Controls.Add(this.LayersEditor);
            this.RightContainer.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.BackgroundControlPaint);
            // 
            // RightContainer.Panel2
            // 
            this.RightContainer.Panel2.AutoScroll = true;
            this.RightContainer.Panel2.Controls.Add(this.KeyframesExpander);
            this.RightContainer.Panel2.Controls.Add(this.PropertiesExpander);
            this.RightContainer.Panel2.Controls.Add(this.SelectedElementLabel);
            this.RightContainer.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.BackgroundControlPaint);
            this.RightContainer.Size = new System.Drawing.Size(189, 412);
            this.RightContainer.SplitterDistance = 165;
            this.RightContainer.SplitterWidth = 2;
            this.RightContainer.TabIndex = 0;
            this.RightContainer.TabStop = false;
            // 
            // SelectedElementLabel
            // 
            this.SelectedElementLabel.AutoEllipsis = true;
            this.SelectedElementLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))));
            this.SelectedElementLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.SelectedElementLabel.ForeColor = System.Drawing.Color.White;
            this.SelectedElementLabel.Location = new System.Drawing.Point(0, 0);
            this.SelectedElementLabel.Name = "SelectedElementLabel";
            this.SelectedElementLabel.Size = new System.Drawing.Size(189, 20);
            this.SelectedElementLabel.TabIndex = 3;
            this.SelectedElementLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ContentPanel
            // 
            this.ContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentPanel.Location = new System.Drawing.Point(0, 40);
            this.ContentPanel.MinimumSize = new System.Drawing.Size(0, 100);
            this.ContentPanel.Name = "ContentPanel";
            this.ContentPanel.Size = new System.Drawing.Size(189, 100);
            this.ContentPanel.TabIndex = 1;
            // 
            // Canvas
            // 
            this.Canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Canvas.CanvasViewLocation = new System.Drawing.Point(10262, -13890);
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Canvas.Location = new System.Drawing.Point(2, 2);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(487, 204);
            this.Canvas.TabIndex = 0;
            // 
            // SequenceTracksEditor
            // 
            this.SequenceTracksEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SequenceTracksEditor.Location = new System.Drawing.Point(0, 0);
            this.SequenceTracksEditor.Name = "SequenceTracksEditor";
            this.SequenceTracksEditor.Size = new System.Drawing.Size(491, 200);
            this.SequenceTracksEditor.TabIndex = 0;
            this.SequenceTracksEditor.SelectedElementChanged += new System.EventHandler<FlipnoteDotNet.Data.Sequence>(this.SequenceTracksEditor_SelectedElementChanged);
            this.SequenceTracksEditor.CurrentFrameChanged += new System.EventHandler(this.SequenceTracksEditor_CurrentFrameChanged);
            // 
            // LayersEditor
            // 
            this.LayersEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayersEditor.Location = new System.Drawing.Point(0, 0);
            this.LayersEditor.Name = "LayersEditor";
            this.LayersEditor.Sequence = null;
            this.LayersEditor.Size = new System.Drawing.Size(189, 165);
            this.LayersEditor.TabIndex = 0;
            this.LayersEditor.SelectionChanged += new System.EventHandler<FlipnoteDotNet.GUI.Layers.LayersEditor.SelectionChangedEventArgs>(this.LayersEditor_SelectionChanged);
            // 
            // KeyframesExpander
            // 
            // 
            // KeyframesExpander.ContentsPanel
            // 
            this.KeyframesExpander.ContentsPanel.AutoSize = true;
            this.KeyframesExpander.ContentsPanel.Controls.Add(this.KeyFramesEditor);
            this.KeyframesExpander.ContentsPanel.Location = new System.Drawing.Point(0, 40);
            this.KeyframesExpander.ContentsPanel.Name = "ContentsPanel";
            this.KeyframesExpander.ContentsPanel.Size = new System.Drawing.Size(189, 40);
            this.KeyframesExpander.ContentsPanel.TabIndex = 1;
            this.KeyframesExpander.Dock = System.Windows.Forms.DockStyle.Top;
            this.KeyframesExpander.IsExpanded = false;
            this.KeyframesExpander.Location = new System.Drawing.Point(0, 60);
            this.KeyframesExpander.Name = "KeyframesExpander";
            this.KeyframesExpander.Size = new System.Drawing.Size(189, 40);
            this.KeyframesExpander.TabIndex = 1;
            this.KeyframesExpander.Title = "Keyframes";
            this.KeyframesExpander.Resize += new System.EventHandler(this.KeyframesExpander_Resize);
            // 
            // KeyFramesEditor
            // 
            this.KeyFramesEditor.AutoSize = true;
            this.KeyFramesEditor.Location = new System.Drawing.Point(0, 0);
            this.KeyFramesEditor.Name = "KeyFramesEditor";
            this.KeyFramesEditor.Size = new System.Drawing.Size(88, 21);
            this.KeyFramesEditor.TabIndex = 0;
            // 
            // PropertiesExpander
            // 
            this.PropertiesExpander.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            // 
            // PropertiesExpander.ContentsPanel
            // 
            this.PropertiesExpander.ContentsPanel.AutoSize = true;
            this.PropertiesExpander.ContentsPanel.Controls.Add(this.PropertyEditor);
            this.PropertiesExpander.ContentsPanel.Location = new System.Drawing.Point(0, 40);
            this.PropertiesExpander.ContentsPanel.MinimumSize = new System.Drawing.Size(0, 25);
            this.PropertiesExpander.ContentsPanel.Name = "ContentsPanel";
            this.PropertiesExpander.ContentsPanel.Size = new System.Drawing.Size(190, 25);
            this.PropertiesExpander.ContentsPanel.TabIndex = 1;
            this.PropertiesExpander.Dock = System.Windows.Forms.DockStyle.Top;
            this.PropertiesExpander.IsExpanded = false;
            this.PropertiesExpander.Location = new System.Drawing.Point(0, 20);
            this.PropertiesExpander.Name = "PropertiesExpander";
            this.PropertiesExpander.Size = new System.Drawing.Size(189, 40);
            this.PropertiesExpander.TabIndex = 2;
            this.PropertiesExpander.Title = "Properties";
            this.PropertiesExpander.Resize += new System.EventHandler(this.PropertiesExpander_Resize);
            // 
            // PropertyEditor
            // 
            this.PropertyEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyEditor.AutoSize = true;
            this.PropertyEditor.KeyFramesEditor = null;
            this.PropertyEditor.Location = new System.Drawing.Point(0, -1);
            this.PropertyEditor.Name = "PropertyEditor";
            this.PropertyEditor.Size = new System.Drawing.Size(189, 20);
            this.PropertyEditor.TabIndex = 0;
            this.PropertyEditor.Target = null;
            this.PropertyEditor.TargetChanged += new System.EventHandler(this.PropertyEditor_TargetChanged);
            this.PropertyEditor.ObjectPropertyChanged += new System.EventHandler<System.Reflection.PropertyInfo>(this.PropertyEditor_ObjectPropertyChanged);
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
            this.RightContainer.Panel1.ResumeLayout(false);
            this.RightContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RightContainer)).EndInit();
            this.RightContainer.ResumeLayout(false);
            this.KeyframesExpander.ContentsPanel.ResumeLayout(false);
            this.KeyframesExpander.ContentsPanel.PerformLayout();
            this.KeyframesExpander.ResumeLayout(false);
            this.KeyframesExpander.PerformLayout();
            this.PropertiesExpander.ContentsPanel.ResumeLayout(false);
            this.PropertiesExpander.ContentsPanel.PerformLayout();
            this.PropertiesExpander.ResumeLayout(false);
            this.PropertiesExpander.PerformLayout();
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
        private GUI.Controls.SequenceTracksEditor SequenceTracksEditor;        
        private System.Windows.Forms.Panel ContentPanel;
        private GUI.Controls.Expander KeyframesExpander;
        private GUI.Controls.Expander PropertiesExpander;
        private GUI.Properties.PropertyEditor PropertyEditor;
        private GUI.Layers.LayersEditor LayersEditor;
        private System.Windows.Forms.Label SelectedElementLabel;
        private GUI.Properties.KeyFramesEditor KeyFramesEditor;
    }
}

