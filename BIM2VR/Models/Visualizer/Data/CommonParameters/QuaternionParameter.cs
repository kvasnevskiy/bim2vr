using SharpDX;

namespace BIM2VR.Models.Visualizer.Data.CommonParameters
{
    public class QuaternionParameter
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        public QuaternionParameter(Quaternion quaternion)
        {
            X = quaternion.X;
            Y = quaternion.Y;
            Z = quaternion.Z;
            W = quaternion.W;
        }
    }
}
