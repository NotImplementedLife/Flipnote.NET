namespace DitheringTest
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
            this.Color0Button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Canvas = new System.Windows.Forms.Panel();
            this.Color1Button = new System.Windows.Forms.Button();
            this.Color2Button = new System.Windows.Forms.Button();
            this.ZoomButton = new System.Windows.Forms.Button();
            this.DitherButton = new System.Windows.Forms.Button();
            this.ScaleX = new System.Windows.Forms.TrackBar();
            this.ScaleY = new System.Windows.Forms.TrackBar();
            this.ResetButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleY)).BeginInit();
            this.SuspendLayout();
            // 
            // Color0Button
            // 
            this.Color0Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Color0Button.Location = new System.Drawing.Point(496, 12);
            this.Color0Button.Name = "Color0Button";
            this.Color0Button.Size = new System.Drawing.Size(75, 23);
            this.Color0Button.TabIndex = 0;
            this.Color0Button.Text = "Color0";
            this.Color0Button.UseVisualStyleBackColor = true;
            this.Color0Button.Click += new System.EventHandler(this.Color0Button_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.Canvas);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(478, 293);
            this.panel1.TabIndex = 1;
            // 
            // Canvas
            // 
            this.Canvas.Location = new System.Drawing.Point(0, 0);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(353, 216);
            this.Canvas.TabIndex = 0;
            this.Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.Canvas_Paint);
            this.Canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDown);
            this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            this.Canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseUp);
            // 
            // Color1Button
            // 
            this.Color1Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Color1Button.Location = new System.Drawing.Point(496, 41);
            this.Color1Button.Name = "Color1Button";
            this.Color1Button.Size = new System.Drawing.Size(75, 23);
            this.Color1Button.TabIndex = 2;
            this.Color1Button.Text = "Color1";
            this.Color1Button.UseVisualStyleBackColor = true;
            this.Color1Button.Click += new System.EventHandler(this.Color1Button_Click);
            // 
            // Color2Button
            // 
            this.Color2Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Color2Button.Location = new System.Drawing.Point(496, 70);
            this.Color2Button.Name = "Color2Button";
            this.Color2Button.Size = new System.Drawing.Size(75, 23);
            this.Color2Button.TabIndex = 3;
            this.Color2Button.Text = "Color2";
            this.Color2Button.UseVisualStyleBackColor = true;
            this.Color2Button.Click += new System.EventHandler(this.Color2Button_Click);
            // 
            // ZoomButton
            // 
            this.ZoomButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZoomButton.Location = new System.Drawing.Point(496, 99);
            this.ZoomButton.Name = "ZoomButton";
            this.ZoomButton.Size = new System.Drawing.Size(75, 23);
            this.ZoomButton.TabIndex = 4;
            this.ZoomButton.Text = "Zoom";
            this.ZoomButton.UseVisualStyleBackColor = true;
            this.ZoomButton.Click += new System.EventHandler(this.ZoomButton_Click);
            // 
            // DitherButton
            // 
            this.DitherButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DitherButton.Location = new System.Drawing.Point(496, 274);
            this.DitherButton.Name = "DitherButton";
            this.DitherButton.Size = new System.Drawing.Size(75, 23);
            this.DitherButton.TabIndex = 5;
            this.DitherButton.Text = "Dither";
            this.DitherButton.UseVisualStyleBackColor = true;
            this.DitherButton.Click += new System.EventHandler(this.DitherButton_Click);
            // 
            // ScaleX
            // 
            this.ScaleX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScaleX.Location = new System.Drawing.Point(496, 166);
            this.ScaleX.Maximum = 50;
            this.ScaleX.Minimum = 2;
            this.ScaleX.Name = "ScaleX";
            this.ScaleX.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ScaleX.Size = new System.Drawing.Size(45, 102);
            this.ScaleX.TabIndex = 6;
            this.ScaleX.Value = 10;
            this.ScaleX.Scroll += new System.EventHandler(this.Scale_Scroll);
            // 
            // ScaleY
            // 
            this.ScaleY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScaleY.Location = new System.Drawing.Point(547, 166);
            this.ScaleY.Maximum = 50;
            this.ScaleY.Minimum = 2;
            this.ScaleY.Name = "ScaleY";
            this.ScaleY.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ScaleY.Size = new System.Drawing.Size(45, 102);
            this.ScaleY.TabIndex = 7;
            this.ScaleY.Value = 10;
            this.ScaleY.Scroll += new System.EventHandler(this.Scale_Scroll);
            // 
            // ResetButton
            // 
            this.ResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ResetButton.Location = new System.Drawing.Point(496, 128);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 8;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 317);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.ScaleY);
            this.Controls.Add(this.ScaleX);
            this.Controls.Add(this.DitherButton);
            this.Controls.Add(this.ZoomButton);
            this.Controls.Add(this.Color2Button);
            this.Controls.Add(this.Color1Button);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Color0Button);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ScaleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Color0Button;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel Canvas;
        private System.Windows.Forms.Button Color1Button;
        private System.Windows.Forms.Button Color2Button;
        private System.Windows.Forms.Button ZoomButton;
        private System.Windows.Forms.Button DitherButton;
        private System.Windows.Forms.TrackBar ScaleX;
        private System.Windows.Forms.TrackBar ScaleY;
        private System.Windows.Forms.Button ResetButton;
    }
}

