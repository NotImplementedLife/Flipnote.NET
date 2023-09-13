using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Designers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Brushes = System.Drawing.Brushes;

namespace FlipnoteDotNet.GUI.Controls
{
    [Designer(typeof(ExpanderDesigner))]
    public partial class Expander : UserControl
    {
        public Expander()
        {
            InitializeComponent();
            this.EnableDoubleBuffer();
            HeaderPanel.EnableDoubleBuffer();
            Content.EnableDoubleBuffer();
            HeaderPanel.Paint += Panel_Paint;
            LocationChanged += Expander_LocationChanged;
        }

        private void Expander_LocationChanged(object sender, EventArgs e)
        {
            Invalidate(true);
        }

        private string _Title = "Expander";

        [Browsable(true)]
        public string Title
        {
            get => _Title;
            set
            {
                _Title = value;
                HeaderPanel.Invalidate();
            }
        }              

        private bool _IsExpanded;

        [DefaultValue(true)]
        public bool IsExpanded 
        {
            get => _IsExpanded;
            set => SetExpanded(value);
        }        

        private void SetExpanded(bool expanded)
        {            
            _IsExpanded = expanded;

            if(IsExpanded)
            {                
                Height = HeaderPanel.Height + Content.Height;                
            }
            else
            {
                Height = HeaderPanel.Height;
            }            
        }        

        private void Header_MouseClick(object sender, MouseEventArgs e)
        {
            IsExpanded = !IsExpanded;            
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            var c = e.ClipRectangle;
            e.Graphics.DrawLine(Colors.FlipnoteThemeMainColor.GetPen(2), c.Left, c.Bottom, c.Right, c.Bottom);
        }       

        private void Expander_Load(object sender, EventArgs e)
        {
            IsExpanded = IsExpanded;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel ContentsPanel => Content;


        bool IsHovered = false;
        private void HeaderPanel_MouseHover(object sender, EventArgs e)
        {
            IsHovered = true;
            HeaderPanel.Invalidate();
        }

        private void HeaderPanel_MouseLeave(object sender, EventArgs e)
        {
            IsHovered = false;
            HeaderPanel.Invalidate();
        }

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {            
            if(IsHovered)
            {
                e.Graphics.FillRectangle(Color.Black.Alpha(64).GetBrush(), e.ClipRectangle);
            }

            var sz = e.Graphics.MeasureString(Title, Font);

            e.Graphics.DrawString(Title, Font, Brushes.Black, 20, (e.ClipRectangle.Height - sz.Height) / 2);
        }
    }
}
