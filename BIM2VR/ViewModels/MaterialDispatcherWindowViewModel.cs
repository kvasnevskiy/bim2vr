using System.Collections.ObjectModel;
using System.Linq;
using BIM2VR.Extensions;
using BIM2VR.Models.Materials;
using BIM2VR.Models.View3D;
using BIM2VR.ViewModels.Materials;
using Prism.Mvvm;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace BIM2VR.ViewModels
{
    public class MaterialDispatcherWindowViewModel : BindableBase
    {
        #region Title

        public string Title => "Диспетчер материалов";

        #endregion

        #region Assistants

        public IMaterialManager MaterialManager { get; protected set; }
        public IView3DManager View3DManager { get; protected set; }

        #endregion

        private MaterialType materialType;
        [ItemsSource(typeof(HelixMaterialModelTypeItemsSource))]
        public MaterialType MaterialType
        {
            get => materialType;
            set => SetProperty(ref materialType, value);
        }

        private bool isSelectedMaterialChanged;
        private MaterialViewModel selectedMaterial;
        public MaterialViewModel SelectedMaterial
        {
            get => selectedMaterial;
            set
            {
                if (SetProperty(ref selectedMaterial, value))
                {
                    if (selectedMaterial != null)
                    {
                        isSelectedMaterialChanged = true;

                        MaterialType = selectedMaterial.Model.Type;
                        View3DManager.HighlightMaterialOwners(selectedMaterial.Model.Owners);

                        isSelectedMaterialChanged = false;
                    }
                }
            }
        }

        private ObservableCollection<MaterialViewModel> materials;
        public ObservableCollection<MaterialViewModel> Materials
        {
            get => materials;
            set => SetProperty(ref materials, value);
        }

        #region Constructors

        public void Initialize(IMaterialManager materialManager, IView3DManager view3DManager)
        {
            MaterialManager = materialManager;
            View3DManager = view3DManager;
            Materials = new ObservableCollection<MaterialViewModel>(MaterialManager.Materials.Select(m => MaterialViewCreator.Create(m.Value)));
        }

        #endregion

        #region Event Handlers

        public void MaterialTypePropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (!isSelectedMaterialChanged)
            {
                if (e.NewValue is MaterialType mType)
                {
                    var newViewModel = MaterialViewCreator.ChangeType(SelectedMaterial, mType);
                    MaterialManager.Replace(SelectedMaterial.Model, newViewModel.Model);
                    Materials.ReplaceItem(x => Equals(x, SelectedMaterial), newViewModel);
                    SelectedMaterial = SelectedMaterial = newViewModel;
                }
            }
        }

        public void MaterialPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (e.OriginalSource is PropertyItem source)
            {
                if (source.Instance is MaterialViewModel instance)
                {
                    
                }
            }
        }

        public void Loaded()
        {
            SelectedMaterial = Materials.FirstOrDefault();
        }

        public void Closed()
        {
            View3DManager.ClearMaterialOwnersHighlight();
        }

        public void SaveClickHandler()
        {
            View3DManager.UpdateMaterials(Materials.Where(m => m.IsChanged).Select(x => x.Model));
        }

        public void CancelClickHandler()
        {
            View3DManager.ClearMaterialOwnersHighlight();
        }

        #endregion
    }
}
