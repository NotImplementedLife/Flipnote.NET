namespace FlipnoteDotNet.App.Forms
{
    partial class FlipnoteEditorForm
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
            FlipnoteEditorContainer = new Controls.FlipnoteEditorControl();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            ToolStripContainer = new ToolStripContainer();
            GeneralToolStrip = new ToolStrip();
            UndoButton = new ToolStripButton();
            RedoButton = new ToolStripButton();
            NewFrameButton = new ToolStripButton();
            CopyCurrentFrameButton = new ToolStripButton();
            RemoveCurrentFrameButton = new ToolStripButton();
            menuStrip1.SuspendLayout();
            ToolStripContainer.ContentPanel.SuspendLayout();
            ToolStripContainer.TopToolStripPanel.SuspendLayout();
            ToolStripContainer.SuspendLayout();
            GeneralToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // FlipnoteEditorContainer
            // 
            FlipnoteEditorContainer.Dock = DockStyle.Fill;
            FlipnoteEditorContainer.Location = new Point(0, 0);
            FlipnoteEditorContainer.Name = "FlipnoteEditorContainer";
            FlipnoteEditorContainer.Size = new Size(567, 294);
            FlipnoteEditorContainer.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(567, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // ToolStripContainer
            // 
            // 
            // ToolStripContainer.ContentPanel
            // 
            ToolStripContainer.ContentPanel.Controls.Add(FlipnoteEditorContainer);
            ToolStripContainer.ContentPanel.Size = new Size(567, 294);
            ToolStripContainer.Dock = DockStyle.Fill;
            ToolStripContainer.LeftToolStripPanelVisible = false;
            ToolStripContainer.Location = new Point(0, 24);
            ToolStripContainer.Name = "ToolStripContainer";
            ToolStripContainer.RightToolStripPanelVisible = false;
            ToolStripContainer.Size = new Size(567, 321);
            ToolStripContainer.TabIndex = 2;
            ToolStripContainer.Text = "toolStripContainer1";
            // 
            // ToolStripContainer.TopToolStripPanel
            // 
            ToolStripContainer.TopToolStripPanel.Controls.Add(GeneralToolStrip);
            // 
            // GeneralToolStrip
            // 
            GeneralToolStrip.Dock = DockStyle.None;
            GeneralToolStrip.ImageScalingSize = new Size(20, 20);
            GeneralToolStrip.Items.AddRange(new ToolStripItem[] { UndoButton, RedoButton, NewFrameButton, CopyCurrentFrameButton, RemoveCurrentFrameButton });
            GeneralToolStrip.Location = new Point(3, 0);
            GeneralToolStrip.Name = "GeneralToolStrip";
            GeneralToolStrip.Size = new Size(163, 27);
            GeneralToolStrip.TabIndex = 0;
            // 
            // UndoButton
            // 
            UndoButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            UndoButton.Image = Properties.Resources.ic_undo;
            UndoButton.ImageTransparentColor = Color.Magenta;
            UndoButton.Name = "UndoButton";
            UndoButton.Size = new Size(24, 24);
            UndoButton.Text = "Undo";
            // 
            // RedoButton
            // 
            RedoButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            RedoButton.Image = Properties.Resources.ic_redo;
            RedoButton.ImageTransparentColor = Color.Magenta;
            RedoButton.Name = "RedoButton";
            RedoButton.Size = new Size(24, 24);
            RedoButton.Text = "Redo";
            // 
            // NewFrameButton
            // 
            NewFrameButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            NewFrameButton.Image = Properties.Resources.ic_new_frame;
            NewFrameButton.ImageTransparentColor = Color.Magenta;
            NewFrameButton.Name = "NewFrameButton";
            NewFrameButton.Size = new Size(24, 24);
            NewFrameButton.Text = "New Frame";
            NewFrameButton.Click += NewFrameButton_Click;
            // 
            // CopyCurrentFrameButton
            // 
            CopyCurrentFrameButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            CopyCurrentFrameButton.Image = Properties.Resources.ic_copy_frame;
            CopyCurrentFrameButton.ImageTransparentColor = Color.Magenta;
            CopyCurrentFrameButton.Name = "CopyCurrentFrameButton";
            CopyCurrentFrameButton.Size = new Size(24, 24);
            CopyCurrentFrameButton.Text = "Duplicate Frame";
            CopyCurrentFrameButton.Click += CopyCurrentFrameButton_Click;
            // 
            // RemoveCurrentFrameButton
            // 
            RemoveCurrentFrameButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            RemoveCurrentFrameButton.Image = Properties.Resources.ic_remove_frame;
            RemoveCurrentFrameButton.ImageTransparentColor = Color.Magenta;
            RemoveCurrentFrameButton.Name = "RemoveCurrentFrameButton";
            RemoveCurrentFrameButton.Size = new Size(24, 24);
            RemoveCurrentFrameButton.Text = "Remove Frame";
            RemoveCurrentFrameButton.Click += RemoveCurrentFrameButton_Click;
            // 
            // FlipnoteEditorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(567, 345);
            Controls.Add(ToolStripContainer);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "FlipnoteEditorForm";
            Text = "FlipnoteEditorForm";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ToolStripContainer.ContentPanel.ResumeLayout(false);
            ToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            ToolStripContainer.TopToolStripPanel.PerformLayout();
            ToolStripContainer.ResumeLayout(false);
            ToolStripContainer.PerformLayout();
            GeneralToolStrip.ResumeLayout(false);
            GeneralToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controls.FlipnoteEditorControl FlipnoteEditorContainer;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripContainer ToolStripContainer;
        private ToolStrip GeneralToolStrip;
        private ToolStripButton UndoButton;
        private ToolStripButton RedoButton;
        private ToolStripButton NewFrameButton;
        private ToolStripButton CopyCurrentFrameButton;
        private ToolStripButton RemoveCurrentFrameButton;
    }
}