using System.Windows.Media;

namespace BIM2VR.Models.Visualizer.Data.CommonParameters
{
    public class RGBAParameter
    {
        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }
        public double A { get; set; }

        public RGBAParameter(Color color)
        {
            R = color.ScR;
            G = color.ScG;
            B = color.ScB;
            A = color.ScA;
        }
    }
}
