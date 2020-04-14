using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using BIM2VR.Extensions;
using BIM2VR.Models.BIM;
using HelixToolkit.Wpf.SharpDX;
using Prism.Mvvm;
using Material = HelixToolkit.Wpf.SharpDX.Material;
using MeshGeometry3D = HelixToolkit.Wpf.SharpDX.MeshGeometry3D;

namespace BIM2VR.ViewModels.BIM
{
    public class BimGeometry3DViewModel : BindableBase
    {
        public Guid Id => Model.Id;

        public BimGeometry3DModel Model { get; }

        private MeshGeometry3D geometry;
        public MeshGeometry3D Geometry
        {
            get => geometry;
            set => SetProperty(ref geometry, value);
        }

        private Material material;
        public Material Material
        {
            get => material;
            set => SetProperty(ref material, value);
        }

        private Transform3D transform;
        public Transform3D Transform
        {
            get => transform;
            set => SetProperty(ref transform, value);
        }

        public BimGeometry3DViewModel(BimGeometry3DModel model)
        {
            Model = model;
            Geometry = Model.Geometry.ToSharpDxMeshGeometry3D();
            Material = Model.Material.ToPBR();
            Transform = new MatrixTransform3D(Model.Transform.ToMatrix3D());
        }
    }
}
