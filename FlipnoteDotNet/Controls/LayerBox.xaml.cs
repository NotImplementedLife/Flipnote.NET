using FlipnoteDotNet.Data;
using FlipnoteDotNet.Drawable;
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

namespace FlipnoteDotNet.Controls
{
    /// <summary>
    /// Interaction logic for LayerBox.xaml
    /// </summary>
    public partial class LayerBox : UserControl
    {
        public LayerBox()
        {
            DataContext = new Layer1Icon();
            InitializeComponent();
            if (Layer == 1)
                DataContext = new Layer1Icon();
            else
                DataContext = new Layer2Icon();            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = ComboBox.SelectedIndex;
            changedFromControl = true;
            Value = (LayerColor)index;
            changedFromControl = false;
            switch (index)
            {
                case 1: (DataContext as UserControl).Foreground = Brushes.Red; break;
                case 2: (DataContext as UserControl).Foreground = Brushes.Blue; break;
                default: (DataContext as UserControl).Foreground = Brushes.Black; break;
            }            
        }

        public static DependencyProperty LayerProperty = DependencyProperty.Register("Layer", typeof(int), typeof(LayerBox),
            new FrameworkPropertyMetadata(1, new PropertyChangedCallback(LayerPropertyChanged)));
        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(LayerColor), typeof(LayerBox),
            new FrameworkPropertyMetadata(LayerColor.BlackWhite, new PropertyChangedCallback(ValuePropertyChanged)));

        private bool changedFromControl = false;
        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var lb = d as LayerBox;
            if (!lb.changedFromControl)
                lb.ComboBox.SelectedIndex = (int)e.NewValue;
            lb.ValueChanged?.Invoke(lb);
        }

        private static void LayerPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var el = d as LayerBox;
            if (el.Layer == 1)
                el.DataContext = new Layer1Icon();
            else
                el.DataContext = new Layer2Icon();
        }

        public int Layer
        {
            get => (int)GetValue(LayerProperty);
            set => SetValue(LayerProperty, value);
        }

        public LayerColor Value
        {
            get => (LayerColor)ComboBox.SelectedIndex;
            set
            {
                SetValue(ValueProperty, value);                                
            }
        }

        public delegate void OnValueChanged(object o);
        public event OnValueChanged ValueChanged;       
    }
}
