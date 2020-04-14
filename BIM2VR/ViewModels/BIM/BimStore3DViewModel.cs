using System.Collections.ObjectModel;
using System.Linq;
using BIM2VR.Models.BIM;
using BIM2VR.Models.Materials;
using Prism.Mvvm;

namespace BIM2VR.ViewModels.BIM
{
    public class BimStore3DViewModel : BindableBase
    {
        public BimStore3DModel Model { get; }

        public ObservableCollection<BimStoreItem3DViewModel> Items { get; set; }

        public BimStore3DViewModel(BimStore3DModel model, IMaterialManager materialManager)
        {
            Model = model;
            Items = new ObservableCollection<BimStoreItem3DViewModel>(model.Items.Select(item => new BimStoreItem3DViewModel(item, materialManager)));
        }
    }
}
