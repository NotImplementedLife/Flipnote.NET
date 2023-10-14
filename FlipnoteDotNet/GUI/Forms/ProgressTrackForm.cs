using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.Utils;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Forms
{
    public partial class ProgressTrackForm : Form, IProgressTracker
    {
        public ProgressTrackForm()
        {
            InitializeComponent();
            ProgressBar.EnableDoubleBuffer();
        }

        #region IProgressTracker

        public object Result { get; set; }
        public bool Success { get; set; }
        public bool CancellationPending { get; set; }
        public int MaximumStepsCount { get => GetMaximumStepsCount(); set => SetMaximumStepsCount(value); }
        public int CurrentStep { get => GetCurrentStep(); set => SetCurrentStep(value); }

        public int GetMaximumStepsCount() => ProgressBar.Maximum;
        public int GetCurrentStep() => ProgressBar.Value;

        public void SetMaximumStepsCount(int value)
        {
            if (InvokeRequired) Invoke(new Action(() => ProgressBar.Maximum = value));
            else ProgressBar.Maximum = value;
        }

        public void SetCurrentStep(int value)
        {
            value = value.Clamp(0, ProgressBar.Maximum);
            if (InvokeRequired) Invoke(new Action(() => ProgressBar.Value = value));
            else ProgressBar.Value = value;
        }

        public void ResetCurrentStep()
        {
            CurrentStep = 0;
        }

        public void IncrementCurrentStep()
        {
            CurrentStep++;
        }

        #endregion        

        public delegate void PorgressTrackFormEventHandler(ProgressTrackForm tracker);
        public event PorgressTrackFormEventHandler DoWork;
        public event PorgressTrackFormEventHandler WorkSucceeded;
        public event PorgressTrackFormEventHandler WorkError;
        

        public void SetCaption(string value)
        {
            if (InvokeRequired)
                Invoke(new Action(() => ProgressBar.Caption = value));
            else
                ProgressBar.Caption = value;
        }        

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ProgressBar.Minimum = 0;
            DoWork?.Invoke(this);
            Thread.Sleep(500);
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Invalidate();            
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (Success)
                WorkSucceeded?.Invoke(this);
            else
                WorkError?.Invoke(this);            
            Close();
        }

        public void Run()
        {            
            ShowDialog();
        }        

        private void ProgressTrackForm_Load(object sender, EventArgs e)
        {
            BackgroundWorker.RunWorkerAsync();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Invoke(new Action(() => CancellationPending = true));
        }
    }
}
