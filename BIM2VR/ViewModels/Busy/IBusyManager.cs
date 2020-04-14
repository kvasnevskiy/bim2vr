namespace BIM2VR.ViewModels.Busy
{
    public interface IBusyManager
    {
        bool IsBusy { get; }
        int CurrentValue { get; }
        int Minimum { get; }
        int Maximum { get; }
        string CurrentMessage { get; }
        void SetBusy(int min, int max, string message);
        void SetFree();
        void SetValue(int value);
        void SetMessage(string message);
    }
}