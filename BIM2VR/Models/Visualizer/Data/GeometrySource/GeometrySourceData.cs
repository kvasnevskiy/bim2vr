using System;
using BIM2VR.Models.Visualizer.Data.Mesh;

namespace BIM2VR.Models.Visualizer.Data.GeometrySource
{
    public class GeometrySourceData
    {
        public Guid Id { get; set; }
        public string Path { get; set; }

        public GeometrySourceData(MeshData meshData, string geometrySourcePath)
        {
            Id = meshData.Id;
            Path = System.IO.Path.Combine(geometrySourcePath, $"{meshData.Id}.json");
        }
    }
}
