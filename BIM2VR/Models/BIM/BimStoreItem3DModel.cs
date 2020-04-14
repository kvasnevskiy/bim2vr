using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using BIM2VR.Models.Geometry;
using BIM2VR.Models.Materials;
using Xbim.Common;
using MeshGeometry3D = HelixToolkit.Wpf.SharpDX.MeshGeometry3D;

namespace BIM2VR.Models.BIM
{
    public class BimStoreItem3DModel// : ICloneable
    {
        public Guid Id { get; }

        public IPersistEntity IfcEntity { get; }

        public List<BimGeometry3DModel> Geometries { get; }

        public void AddGeometry(MeshModel geometry, MaterialModel material, Matrix3D transform)
        {
            Geometries.Add(new BimGeometry3DModel(geometry, material, transform));
        }

        public BimStoreItem3DModel(IPersistEntity ifcEntity, IEnumerable<BimGeometry3DModel> geometries)
        {
            Id = Guid.NewGuid();
            IfcEntity = ifcEntity;
            Geometries = new List<BimGeometry3DModel>(geometries);
        }

        //public BimStoreItem3D(BimStoreItem3D bimModelItem3D)
        //{
        //    Id = bimModelItem3D.Id;
        //}

        public override bool Equals(object obj)
        {
            if (obj is BimStoreItem3DModel bimModelItem3D)
            {
                return bimModelItem3D.Id == this.Id;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        //public object Clone()
        //{
        //    return new BimStoreItem3D(this);
        //}
    }
}
