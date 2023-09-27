namespace FlipnoteDotNet.Utils
{
    public interface IProgressTracker
    {
        int MaximumStepsCount { get; set; }
        int CurrentStep { get; set; }

        void SetMaximumStepsCount(int value);
        void ResetCurrentStep();
        void IncrementCurrentStep();

        bool CancellationPending { get; }
        object Result { get; set; }
        bool Success { get; set; }
    }
}
