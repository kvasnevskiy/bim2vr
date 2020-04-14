using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Xbim.ModelGeometry.Scene;

namespace BIM2VR.Models.BIM
{
    public class BimStore3DModel// : ICloneable
    {
        public Guid Id { get; }

        private IfcStore ifc;
        public IfcStore Ifc
        {
            get
            {
                var context = new Xbim3DModelContext(ifc);
                context.CreateContext(null, true);
                return ifc;
            }
            set => ifc = value;
        }

        public List<BimStoreItem3DModel> Items { get; set; }

        public void Add(BimStoreItem3DModel item)
        {
            Items.Add(item);
        }

        public void AddRange(IEnumerable<BimStoreItem3DModel> items)
        {
            Items.AddRange(items);
        }

        public void Clear()
        {
            Items.Clear();
        }

        #region Constructors
 
        public BimStore3DModel(IfcStore ifc)
        {
            Id = Guid.NewGuid();
            Ifc = ifc;
            Items = new List<BimStoreItem3DModel>();
        }

        //public BimStore3D(BimStore3D bimModel3D)
        //{
        //    Id = bimModel3D.Id;
        //    Ifc = bimModel3D.Ifc;
        //    Items = new List<BimStoreItem3D>(bimModel3D.Items.Select(item => item.Clone() as BimStoreItem3D));
        //}

        #endregion

        //public object Clone()
        //{
        //    return new BimStore3D(this);
        //}
    }
}
