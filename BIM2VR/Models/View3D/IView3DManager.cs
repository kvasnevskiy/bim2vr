using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using BIM2VR.Models.BIM;
using BIM2VR.Models.Materials;
using BIM2VR.ViewModels.BIM;
using HelixToolkit.Wpf.SharpDX;
using Camera = HelixToolkit.Wpf.SharpDX.Camera;
using Material = HelixToolkit.Wpf.SharpDX.Material;

namespace BIM2VR.Models.View3D
{
    public interface IView3DManager
    {
        void UpdateMaterials(IEnumerable<MaterialModel> materials);
        void HighlightMaterialOwners(IEnumerable<Guid> owners);
        void ClearMaterialOwnersHighlight();
        Vector3D UpDirection { get; }
        Camera Camera { get; }
        IEffectsManager EffectsManager { get; }
        Color DirectionalLightColor { get; }
        Color AmbientLightColor { get; }
        Stream BackgroundTexture { get; }
        bool IsWireframeEnabled { get; set; }
        BimStore3DViewModel Model { get; }
        ObservableCollection<BimStoreItem3DViewModel> SelectedItems { get; }
        ObservableCollection<BimGeometry3DViewModel> SelectedGeometries { get; }
        Material HighlightMaterial { get; }
        void Initialize(Viewport3DX inputViewport);
        void SetModel(BimStore3DViewModel inputModel);
        void ZoomToAll();
        void EnableWireframe();
        void DisableWireframe();
        void HighlightItem(BimStoreItem3DViewModel item);
        void ClearItemsHighlight();
    }
}