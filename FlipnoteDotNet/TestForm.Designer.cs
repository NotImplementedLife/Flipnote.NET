namespace FlipnoteDotNet
{
    partial class TestForm
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
            propertyEditorControl1 = new PropertyEditor.PropertyEditorControl();
            DoneBox = new CheckBox();
            DoneSetButton = new Button();
            splitContainer1 = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // propertyEditorControl1
            // 
            propertyEditorControl1.BackColor = Color.White;
            propertyEditorControl1.Dock = DockStyle.Fill;
            propertyEditorControl1.Location = new Point(0, 0);
            propertyEditorControl1.Name = "propertyEditorControl1";
            propertyEditorControl1.Object = null;
            propertyEditorControl1.Size = new Size(167, 242);
            propertyEditorControl1.TabIndex = 0;
            propertyEditorControl1.Text = "propertyEditorControl1";
            // 
            // DoneBox
            // 
            DoneBox.AutoSize = true;
            DoneBox.Location = new Point(6, 3);
            DoneBox.Name = "DoneBox";
            DoneBox.Size = new Size(54, 19);
            DoneBox.TabIndex = 1;
            DoneBox.Text = "Done";
            DoneBox.UseVisualStyleBackColor = true;
            // 
            // DoneSetButton
            // 
            DoneSetButton.Location = new Point(66, 3);
            DoneSetButton.Name = "DoneSetButton";
            DoneSetButton.Size = new Size(54, 23);
            DoneSetButton.TabIndex = 2;
            DoneSetButton.Text = "Set";
            DoneSetButton.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(propertyEditorControl1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(DoneSetButton);
            splitContainer1.Panel2.Controls.Add(DoneBox);
            splitContainer1.Size = new Size(462, 242);
            splitContainer1.SplitterDistance = 167;
            splitContainer1.TabIndex = 3;
            // 
            // TestForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(462, 242);
            Controls.Add(splitContainer1);
            Name = "TestForm";
            Text = "TestForm";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PropertyEditor.PropertyEditorControl propertyEditorControl1;
        private CheckBox DoneBox;
        private Button DoneSetButton;
        private SplitContainer splitContainer1;
    }
}