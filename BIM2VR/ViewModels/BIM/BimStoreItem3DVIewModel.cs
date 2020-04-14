using System.Collections.ObjectModel;
using System.Linq;
using BIM2VR.Models.BIM;
using BIM2VR.Models.Materials;
using Prism.Mvvm;
using Xbim.Common;

namespace BIM2VR.ViewModels.BIM
{
    public class BimStoreItem3DViewModel : BindableBase
    {
        public BimStoreItem3DModel Model { get; }

        public IPersistEntity IfcEntity => Model.IfcEntity;

        public ObservableCollection<BimGeometry3DViewModel> Geometries { get; set; }

        public BimStoreItem3DViewModel(BimStoreItem3DModel model, IMaterialManager materialManager)
        {
            Model = model;
            Geometries = new ObservableCollection<BimGeometry3DViewModel>(model.Geometries.Select(x => new BimGeometry3DViewModel(x)));
        }
    }
}
