using System;
using BIM2VR.Models.BIM;
using BIM2VR.Models.Visualizer.Data.Transform;

namespace BIM2VR.Models.Visualizer.Data.ConstructiveMesh
{
    public class ConstructiveMeshData
    {
        public Guid Id { get; set; }
        public TransformData Transform { get; set; }
        public Guid GeometryId { get; set; }
        public Guid MaterialId { get; set; }

        public ConstructiveMeshData(BimGeometry3DModel geometry, Guid geometryId, Guid materialId)
        {
            Id = Guid.NewGuid();
            Transform = new TransformData(geometry.Translation, geometry.Rotation, geometry.Scale);
            GeometryId = geometryId;
            MaterialId = materialId;
        }
    }
}
