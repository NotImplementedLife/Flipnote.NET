using FlipnoteDesktop.Drawable;
using FlipnoteDesktop.Environment.Canvas;
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
    /// Interaction logic for PatternSelector.xaml
    /// </summary>
    public partial class PatternSelector : UserControl
    {        
        public PatternSelector()
        {
            InitializeComponent();
            ComboBox.ItemsSource = Patterns.NamesList();            
        }

        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Pattern), typeof(PatternSelector),
          new FrameworkPropertyMetadata(Patterns.Mono, new PropertyChangedCallback(ValuePropertyChanged)));

        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var o = d as PatternSelector;
            o.ValueChanged?.Invoke(o);
        }

        public Pattern Value
        {
            get => GetValue(ValueProperty) as Pattern;
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public delegate void OnValueChanged(object o);
        public event OnValueChanged ValueChanged;

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataContext = new PatternSample
            {
                PatternName = (string)ComboBox.SelectedItem,
                Padding = new Thickness(3)                
            };
            Value = (DataContext as PatternSample).Pattern;
        }
    }
}
