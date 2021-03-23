using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for SpeedSelector.xaml
    /// </summary>
    public partial class SpeedSelector : UserControl
    {
        public SpeedSelector()
        {
            InitializeComponent();
        }

        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(SpeedSelector),
            new PropertyMetadata(1, new PropertyChangedCallback(ValuePropertyChanged)));

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var o = d as SpeedSelector;
            o.ValueDisplay.Width = o.Value * 10;
            o.ValueText.Text = o.Value.ToString();
            if (e.OldValue != e.NewValue)
                o.ValueChanged?.Invoke(o);
        }

        int msOverValue = 1;
        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            msOverValue = (int)(e.GetPosition(Canvas).X * 8 / Canvas.ActualWidth) + 1;// >> 3;
            ValueDisplay.Width = Math.Max(10, msOverValue * 10);
            ValueText.Text = msOverValue.ToString();
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            ValueDisplay.Width = Value * 10;
            ValueText.Text = Value.ToString();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Value = msOverValue;
        }

        public delegate void OnValueChanged(object sender);
        public event OnValueChanged ValueChanged;
    }
}
