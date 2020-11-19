using FlipnoteDesktop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlipnoteDesktop.Controls
{
    /// <summary>
    /// Interaction logic for FramesList.xaml
    /// </summary>
    public partial class FramesList : UserControl
    {
        public FramesList()
        {
            InitializeComponent();
        }

        private void AddFrameButton_Click(object sender, RoutedEventArgs e)
        {
            if(List.SelectedItems.Count==0)
            {                
                (List.ItemsSource as List<DecodedFrame>).Add(new DecodedFrame()
                {
                    IsPaperWhite=true,
                });
                List.Items.Refresh();

            }
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(List.SelectedItems.Count==1)
            {
                SingleFrameSelected?.Invoke(this, List.SelectedItem as DecodedFrame);
            }
        }

        public delegate void OnSingleFrameSelected(object o, DecodedFrame frame);
        public event OnSingleFrameSelected SingleFrameSelected;
    }
}
