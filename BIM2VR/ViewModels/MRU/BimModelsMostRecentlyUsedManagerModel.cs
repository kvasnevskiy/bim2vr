using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;

namespace BIM2VR.ViewModels.MRU
{
    public class BimModelsMostRecentlyUsedManagerModel : IBimModelsMostRecentlyUsedManager
    {
        private readonly int maxCount = 5;

        public ObservableCollection<string> BimModels { get; set; }

        public bool IsContains(string bimModelPath)
        {
            return BimModels.Contains(bimModelPath);
        }

        public void Add(string bimModelPath)
        {
            if (BimModels.Count >= maxCount)
            {
                BimModels.RemoveAt(0);
            }

            if (!IsContains(bimModelPath))
            {
                BimModels.Add(bimModelPath);
                SaveToRegistry();
            }
        }

        public void Remove(string bimModelPath)
        {
            if (BimModels.Remove(bimModelPath))
            {
                SaveToRegistry();
            }
        }

        #region Constructors

        public BimModelsMostRecentlyUsedManagerModel()
        {
            BimModels = new ObservableCollection<string>();
            LoadFromRegistry();
        }

        #endregion

        #region Registry

        private void SaveToRegistry()
        {
            var rootSoftwareKey = Registry.CurrentUser.OpenSubKey("Software", true);

            if (rootSoftwareKey != null)
            {
                var appKey = rootSoftwareKey.OpenSubKey("BIM2VR", true) ?? rootSoftwareKey.CreateSubKey("BIM2VR");

                if (appKey != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(ms, BimModels);
                        var data = ms.ToArray();
                        appKey.SetValue("BimModelsMRU", data, RegistryValueKind.Binary);
                    }
                }
            }
        }

        private void LoadFromRegistry()
        {
            var rootSoftwareKey = Registry.CurrentUser.OpenSubKey("Software", true);

            var appKey = rootSoftwareKey?.OpenSubKey("BIM2VR", false);

            var data = (byte[]) appKey?.GetValue("BimModelsMRU");

            if (data != null)
            {
                using (var ms = new MemoryStream(data))
                {
                    var formatter = new BinaryFormatter();
                    BimModels = formatter.Deserialize(ms) as ObservableCollection<string>;
                }
            }
        }

        #endregion
    }
}
