using SharpDX;

namespace BIM2VR.Models.Visualizer.Data.CommonParameters
{
    public class Vector2DParameter
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2DParameter(Vector2 vector)
        {
            X = vector.X;
            Y = vector.Y;
        }
    }
}
