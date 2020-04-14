using System;
using System.Windows.Media;
using BIM2VR.Models.Visualizer.Data.MaterialSource.Attributes;
using HelixToolkit.Wpf.SharpDX;

namespace BIM2VR.Models.Materials
{
    [VisualizerMaterial("H_Color")]
    public class ColorMaterialModel : MaterialModel
    {
        #region Albedo (Only Color)

        [VisualizerColorMaterialParameter]
        public Color AlbedoColor
        {
            get => Representation.AlbedoColor.ToColor();
            set => Representation.AlbedoColor = value.ToColor4();
        }

        #endregion

        #region Constructors

        public ColorMaterialModel(Guid id, string name)
            : base(id, name, MaterialType.Basic)
        {
            AlbedoColor = Colors.White;
        }

        public ColorMaterialModel(Color albedoColor)
            : base(MaterialType.Color)
        {
            AlbedoColor = albedoColor;
        }

        public ColorMaterialModel()
            : base(MaterialType.Color)
        {
            AlbedoColor = Colors.White;
        }

        #endregion
    }
}
