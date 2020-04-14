using System;
using System.Windows.Media.Media3D;
using BIM2VR.Models.Geometry;
using BIM2VR.Models.Materials;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using MeshGeometry3D = HelixToolkit.Wpf.SharpDX.MeshGeometry3D;
using Quaternion = SharpDX.Quaternion;

namespace BIM2VR.Models.BIM
{
    public class BimGeometry3DModel
    {
        public Guid Id { get; }

        public MeshModel Geometry { get; }
        public MaterialModel Material { get; set; }
        public Matrix Transform { get; set; }

        private readonly Vector3 translation;
        public Vector3 Translation => translation;

        private readonly Quaternion rotation;
        public Quaternion Rotation => rotation;

        private readonly Vector3 scale;
        public Vector3 Scale => scale;

        public BimGeometry3DModel(MeshModel geometry, MaterialModel material, Matrix3D transform)
        {
            Id = Guid.NewGuid();
            Geometry = geometry;
            Material = material;
            Transform = transform.ToMatrix();
            Transform.Decompose(out scale, out rotation, out translation);

            //Add owner to material
            Material.Owners.Add(Id);
        }
    }
}