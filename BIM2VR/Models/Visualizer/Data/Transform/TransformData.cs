using BIM2VR.Models.Visualizer.Data.CommonParameters;
using SharpDX;
using Quaternion = SharpDX.Quaternion;

namespace BIM2VR.Models.Visualizer.Data.Transform
{
    public class TransformData
    {
        public Vector3DParameter Translation { get; set; }
        public QuaternionParameter Rotation { get; set; }
        public Vector3DParameter Scaling { get; set; }

        public TransformData(Vector3 translation, Quaternion rotation, Vector3 scaling)
        {
            Translation = new Vector3DParameter(translation);
            Rotation = new QuaternionParameter(rotation);
            Scaling = new Vector3DParameter(scaling);
        }
    }
}
