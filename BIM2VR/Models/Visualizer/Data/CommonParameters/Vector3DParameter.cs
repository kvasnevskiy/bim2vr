using SharpDX;

namespace BIM2VR.Models.Visualizer.Data.CommonParameters
{
    public class Vector3DParameter
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3DParameter(Vector3 vector)
        {
            X = vector.X;
            Y = vector.Y;
            Z = vector.Z;
        }
    }
}
