using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties
{
    public partial class PropertyEditor : UserControl
    {
        public PropertyEditor()
        {
            InitializeComponent();
        }

        private object _Target = null;

        public object Target
        {
            get => _Target;
            set
            {
                _Target = value;
                ReloadFields();
            }
        }


        private void ReloadFields()
        {


        }

    }
}
