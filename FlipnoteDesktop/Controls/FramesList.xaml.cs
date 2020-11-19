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
            var lst = List.ItemsSource as List<DecodedFrame>;
            if(List.SelectedItems.Count==0)
            {
                lst.Add(new DecodedFrame());
                List.Items.Refresh();
            }
            else
            {
                var index = List.Items.IndexOf(List.SelectedItems[List.SelectedItems.Count - 1]);
                lst.Insert(index + 1, new DecodedFrame());
                List.SelectedIndex = index + 1;
                List.Items.Refresh();
            }        
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(List.SelectedItems.Count==0)
            {
                CopyFrameButton.IsEnabled = false;
            }
            else if(List.SelectedItems.Count==1)
            {
                SingleFrameSelected?.Invoke(this, List.SelectedItem as DecodedFrame);
                CopyFrameButton.IsEnabled = true;
            }
        }

        public delegate void OnSingleFrameSelected(object o, DecodedFrame frame);
        public event OnSingleFrameSelected SingleFrameSelected;

        private void CopyFrameButton_Click(object sender, RoutedEventArgs e)
        {
            var lst = List.ItemsSource as List<DecodedFrame>;
            if (List.SelectedItems.Count == 1)
            {
                int index = List.Items.IndexOf(List.SelectedItems[List.SelectedItems.Count - 1]);
                lst.Insert(index + 1, new DecodedFrame(lst[index]));
                List.SelectedIndex = index + 1;
                List.Items.Refresh();
            }
        }

        private void TossFrameButton_Click(object sender, RoutedEventArgs e)
        {
            var lst = List.ItemsSource as List<DecodedFrame>;            
            int index = List.Items.IndexOf(List.SelectedItems[List.SelectedItems.Count - 1]);
            for (int i=0,cnt=List.SelectedItems.Count;i<cnt;i++)
            {
                lst.Remove(List.SelectedItems[i] as DecodedFrame);
            }
            bool addedFrame = false;
            if (lst.Count == 0)
            {
                lst.Add(new DecodedFrame());
                addedFrame = true;
            } 
            List.Items.Refresh();
            List.SelectedIndex = index < lst.Count ? index : 0;
            if (addedFrame)
            {
                SingleFrameSelected?.Invoke(this, List.SelectedItem as DecodedFrame);
            }
        }
    }
}
