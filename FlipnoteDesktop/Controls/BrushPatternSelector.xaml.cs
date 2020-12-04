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
    /// Interaction logic for BrushPatternSelector.xaml
    /// </summary>
    public partial class BrushPatternSelector : UserControl
    {        
        public BrushPatternSelector()
        {
            InitializeComponent();
            ComboBox.ItemsSource = BrushPatterns.NamesList();            
        }

        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Pattern), typeof(BrushPatternSelector),
          new FrameworkPropertyMetadata(BrushPatterns.Mono, new PropertyChangedCallback(ValuePropertyChanged)));

        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var o = d as BrushPatternSelector;
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
            DataContext = new BrushPatternSample
            {                
                PatternName = (string)ComboBox.SelectedItem,
                Padding = new Thickness(3)                
            };
            Value = (DataContext as BrushPatternSample).Pattern;
        }
    }
}
