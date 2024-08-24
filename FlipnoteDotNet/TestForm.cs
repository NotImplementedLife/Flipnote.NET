using FlipnoteDotNet.Canvas;
using FlipnoteDotNet.PropertyEditor;
using System.ComponentModel;
using System.Diagnostics;

namespace FlipnoteDotNet
{
    public partial class TestForm : Form
    {
        class A : INotifyPropertyChanged
        {
            private string fName = "name of A";
            public string Name
            {
                get => fName;
                set { fName = value; OnPropertyChanged(nameof(Name)); }
            }

            public int Value { get; } = 2;

            private bool fDone = true;
            public bool Done
            {
                get => fDone;
                set { fDone = value; OnPropertyChanged(nameof(Done)); }
            }
            public int Number { get; set; } = 15;

            private CanvasTransform fTransform = new CanvasTransform();
            public CanvasTransform Transform
            {
                get => fTransform;
                set { fTransform = value; OnPropertyChanged(nameof(Transform)); }                
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName)
                => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TestForm()
        {
            InitializeComponent();
            var a = new A();
            a.PropertyChanged += (o, e) => { Debug.WriteLine($"PropChanged: {e.PropertyName}"); };
            propertyEditorControl1.Object = a;
            DoneSetButton.Click += (_, _) => a.Done = DoneBox.Checked;
        }
        
    }
}
