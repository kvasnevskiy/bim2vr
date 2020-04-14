using HelixToolkit.Wpf.SharpDX;
using System.Linq;
using BIM2VR.Models.Geometry;

namespace BIM2VR.Extensions
{
    public static class MeshExtensions
    {
        public static MeshModel ToMeshModel(this System.Windows.Media.Media3D.MeshGeometry3D meshGeometry3D)
        {
            return new MeshModel(
                new Vector3Collection(meshGeometry3D.Positions.Select(p => p.ToVector3())),
                new Vector3Collection(meshGeometry3D.Normals.Select(n => n.ToVector3())),
                new IntCollection(meshGeometry3D.TriangleIndices),
                true);
        }

        public static MeshGeometry3D ToSharpDxMeshGeometry3D(this MeshModel meshModel)
        {
            return new MeshGeometry3D
            {
                Positions = new Vector3Collection(meshModel.Positions),
                Normals = new Vector3Collection(meshModel.Normals),
                TextureCoordinates = new Vector2Collection(meshModel.TextureCoordinates),
                TriangleIndices = new IntCollection(meshModel.TriangleIndices)
            };
        }
    }
}
