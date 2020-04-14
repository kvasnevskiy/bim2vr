using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BIM2VR.Extensions;
using BIM2VR.Models.BIM;
using BIM2VR.Models.Materials;
using Xbim.Common;
using Xbim.Common.Geometry;
using Xbim.Common.Metadata;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Xbim.IO;
using Xbim.ModelGeometry.Scene;
using Xbim.Presentation;
using MeshGeometry3D = System.Windows.Media.Media3D.MeshGeometry3D;

namespace BIM2VR.Models.Import
{
    public class ImportManagerModel : IImportManager
    {
        private static readonly List<Type> DefaultExcludedTypes = new List<Type>()
        {
            typeof(Xbim.Ifc2x3.ProductExtension.IfcSpace),
            typeof(Xbim.Ifc4.ProductExtension.IfcSpace),
            typeof(Xbim.Ifc2x3.ProductExtension.IfcFeatureElement),
            typeof(Xbim.Ifc4.ProductExtension.IfcFeatureElement)
        };

        private static HashSet<short> GenerateDefaultExclusions(IModel model, List<Type> exclude)
        {
            var excludedTypes = new HashSet<short>();
            if (exclude == null)
                exclude = new List<Type>()
                {
                    typeof(IIfcSpace),
                    typeof(IIfcFeatureElement)
                };
            foreach (var excludedT in exclude)
            {
                ExpressType ifcT;
                if (excludedT.IsInterface && excludedT.Name.StartsWith("IIfc"))
                {
                    var concreteTypename = excludedT.Name.Substring(1).ToUpper();
                    ifcT = model.Metadata.ExpressType(concreteTypename);
                }
                else
                    ifcT = model.Metadata.ExpressType(excludedT);
                if (ifcT == null) // it could be a type that does not belong in the model schema
                    continue;
                foreach (var exIfcType in ifcT.NonAbstractSubTypes)
                {
                    excludedTypes.Add(exIfcType.TypeId);
                }
            }
            return excludedTypes;
        }

        private IEnumerable<XbimShapeInstance> GetShapeInstancesToRender(IGeometryStoreReader geometryReader, HashSet<short> excludedTypes)
        {
            var shapeInstances = geometryReader.ShapeInstances
                .Where(s => s.RepresentationType == XbimGeometryRepresentationType.OpeningsAndAdditionsIncluded
                            &&
                            !excludedTypes.Contains(s.IfcTypeId));
            return shapeInstances;
        }

        private readonly Action<int, object> reportProgress;

        private IMaterialManager MaterialManager { get; }

        public ImportManagerModel(Action<int, object> reportProgress, IMaterialManager materialManager)
        {
            this.reportProgress = reportProgress;
            MaterialManager = materialManager;
        }

        public BimStore3DModel Load(string fileName)
        {
            var bimStoreItems = new Dictionary<IPersistEntity, List<Tuple<MeshGeometry3D, MaterialModel, XbimMatrix3D>>> (); //ID - ifcProductLabel

            var ifcModel = IfcStore.Open(fileName, null, null,
                (progress, state) => { reportProgress?.Invoke(progress, state); },
                XbimDBAccess.Exclusive);

            //Prepare
            var modelPosition = new XbimModelPositioning(ifcModel);
            var context = new Xbim3DModelContext(ifcModel);
            context.CreateContext();

            //Clear materials
            MaterialManager.Clear();

            var materialsByStyleId = new Dictionary<int, MaterialModel>();

            var excludedTypes = GenerateDefaultExclusions(ifcModel, DefaultExcludedTypes);

            using (var geometryStore = ifcModel.ReferencingModel.GeometryStore)
            {
                using (var geometryReader = geometryStore.BeginRead())
                {
                    var shapeInstances = GetShapeInstancesToRender(geometryReader, excludedTypes);

                    foreach (var shapeInstance in shapeInstances)
                    {
                        #region Material

                        var styleId = shapeInstance.StyleLabel > 0
                            ? shapeInstance.StyleLabel
                            : shapeInstance.IfcTypeId * -1;

                        MaterialModel material;
                        if (!materialsByStyleId.ContainsKey(styleId))
                        {
                            material = MaterialModelCreator.Create(ifcModel, shapeInstance.IfcTypeId);
                            materialsByStyleId.Add(styleId, material);
                            MaterialManager.Add(material);
                        }
                        else
                        {
                            material = materialsByStyleId[styleId];
                        }

                        #endregion

                        #region Geometry

                        IXbimShapeGeometryData shapeGeometry =
                            geometryReader.ShapeGeometry(shapeInstance.ShapeGeometryLabel);

                        var geometry3D = new MeshGeometry3D();
                        switch ((XbimGeometryType) shapeGeometry.Format)
                        {
                            case XbimGeometryType.PolyhedronBinary:
                                geometry3D.Read(shapeGeometry.ShapeData);
                                break;
                            case XbimGeometryType.Polyhedron:
                                geometry3D.Read(((XbimShapeGeometry) shapeGeometry).ShapeData);
                                break;
                        }

                        #endregion

                        var ifcProduct = ifcModel.Model.Instances.FirstOrDefault(i => i.EntityLabel == shapeInstance.IfcProductLabel);

                        if (ifcProduct != null)
                        {
                            if (!bimStoreItems.ContainsKey(ifcProduct))
                            {
                                bimStoreItems.Add(ifcProduct, new List<Tuple<MeshGeometry3D, MaterialModel, XbimMatrix3D>>(new []
                                {
                                    new Tuple<MeshGeometry3D, MaterialModel, XbimMatrix3D>(geometry3D, material, XbimMatrix3D.Multiply(modelPosition.Transform, shapeInstance.Transformation)), 
                                }));
                            }
                            else
                            {
                                bimStoreItems[ifcProduct].Add(new Tuple<MeshGeometry3D, MaterialModel, XbimMatrix3D>(geometry3D, material, XbimMatrix3D.Multiply(modelPosition.Transform, shapeInstance.Transformation)));
                            }
                        }
                    }
                }
            }

            //Generate model
            var bimStore3D = new BimStore3DModel(context.Model as IfcStore);

            foreach (var bimStoreItem in bimStoreItems)
            {
                bimStore3D.Add(
                    new BimStoreItem3DModel(bimStoreItem.Key,
                        new List<BimGeometry3DModel>(
                            bimStoreItem.Value.Select(x =>
                                new BimGeometry3DModel(
                                    x.Item1.ToMeshModel(),
                                    x.Item2,
                                    x.Item3.ToMatrix3D())))));
            }

            return bimStore3D;
        }

        public Task<BimStore3DModel> LoadAsync(string fileName)
        {
            return Task.Factory.StartNew(() => Load(fileName));
        }
    }
}
