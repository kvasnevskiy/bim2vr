using System.Collections.ObjectModel;

namespace BIM2VR.ViewModels.MRU
{
    public interface IBimModelsMostRecentlyUsedManager
    {
        ObservableCollection<string> BimModels { get; }
        bool IsContains(string bimModelPath);
        void Add(string bimModelPath);
        void Remove(string bimModelPath);
    }
}
