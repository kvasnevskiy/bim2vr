using System;
using System.Linq;
using BIM2VR.Models.BIM;
using BIM2VR.Models.Visualizer.Data.CommonParameters;

namespace BIM2VR.Models.Visualizer.Data.Mesh
{
    public class MeshData
    {
        public Guid Id { get; set; }
        public Vector3DParameter[] Vertices { get; set; }
        public int[] Triangles { get; set; }
        public Vector3DParameter[] Normals { get; set; }
        public Vector2DParameter[] TextureCoordinates { get; set; }

        public MeshData(BimGeometry3DModel geometry)
        {
            Id = geometry.Id;
            Vertices = geometry.Geometry.Positions.Select(i => new Vector3DParameter(i)).ToArray();
            Triangles = geometry.Geometry.TriangleIndices.ToArray();
            Normals = geometry.Geometry.Normals.Select(i => new Vector3DParameter(i)).ToArray();
            TextureCoordinates = geometry.Geometry.TextureCoordinates.Select(i => new Vector2DParameter(i)).ToArray();
        }
    }
}
