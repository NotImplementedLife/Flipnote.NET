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
    /// Interaction logic for NumericInput.xaml
    /// </summary>
    public partial class NumericInput : UserControl
    {
        public NumericInput()
        {
            InitializeComponent();
            Input.Text = Value.ToString();
        }

        private bool changedFromControl = false;

        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(NumericInput),
            new FrameworkPropertyMetadata(0, new PropertyChangedCallback(ValuePropertyChanged)));

        public static DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(int), typeof(NumericInput),
            new FrameworkPropertyMetadata(100, new PropertyChangedCallback(MaxValuePropertyChanged)));

        public static DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(int), typeof(NumericInput),
           new FrameworkPropertyMetadata(0, new PropertyChangedCallback(MinValuePropertyChanged)));
        
        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ni = d as NumericInput;
            if (!ni.changedFromControl) 
            {                
                ni.Input.Text = ((int)e.NewValue).ToString();
            }            
            ni.ValueChanged?.Invoke(ni);
            //MessageBox.Show("Here2");
        }

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public delegate void OnValueChanged(object o);
        public event OnValueChanged ValueChanged;

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (Value < MaxValue)
                Value++;
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (Value > MinValue) 
                Value--; 
        }

        private static void MaxValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ni = d as NumericInput;
            if(ni.Value>ni.MaxValue)
            {                
                ni.Value = ni.MaxValue;                
            }
        }

        public int MaxValue
        {
            get => (int)GetValue(MaxValueProperty);
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        private static void MinValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ni = d as NumericInput;
            if (ni.Value < ni.MinValue)
            {                
                ni.Value = ni.MinValue;                
            }
        }

        public int MinValue
        {
            get => (int)GetValue(MinValueProperty);
            set
            {
                SetValue(MinValueProperty, value);
            }
        }       

        private void UpdateValue()
        {
            if (int.TryParse(Input.Text, out int val))
            {
                if (val < MinValue) val = MinValue;
                else if (val > MaxValue) val = MaxValue;
                Value = val;
            }
            else if (Input.Text != "")
            {
                int i = Input.CaretIndex;
                Input.Text = Value.ToString();
                Input.CaretIndex = Math.Min(i - 1, Input.Text.Length);
            }
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateValue();            
        }
    }
}
