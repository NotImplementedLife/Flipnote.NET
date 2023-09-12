using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Designers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Controls
{
    [Designer(typeof(ExpanderDesigner))]
    public partial class Expander : UserControl
    {
        public Expander()
        {
            InitializeComponent();
            this.EnableDoubleBuffer();
            Content.EnableDoubleBuffer();            
        }

        [Browsable(true)]
        public string Title
        {
            get => Header.Text;
            set => Header.Text = value;
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
    }
}
