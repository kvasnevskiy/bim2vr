using System;
using System.Windows.Media;
using BIM2VR.Models.Visualizer.Data.MaterialSource.Attributes;
using HelixToolkit.Wpf.SharpDX;

namespace BIM2VR.Models.Materials
{
    [VisualizerMaterial("H_Basic")]
    public class BasicMaterialModel : MaterialModel
    {
        #region Albedo

        [VisualizerColorMaterialParameter]
        public Color AlbedoColor
        {
            get => Representation.AlbedoColor.ToColor();
            set => Representation.AlbedoColor = value.ToColor4();
        }

        private TextureModelEx albedoMap;
        [VisualizerTextureMaterialParameter]
        public TextureModelEx AlbedoMap
        {
            get => albedoMap;
            set => Representation.AlbedoMap = albedoMap = value;
        }

        #endregion

        #region Emissive

        [VisualizerColorMaterialParameter]
        public Color EmissiveColor
        {
            get => Representation.EmissiveColor.ToColor();
            set => Representation.EmissiveColor = value.ToColor4();
        }

        private TextureModelEx emissiveMap;
        [VisualizerTextureMaterialParameter]
        public TextureModelEx EmissiveMap
        {
            get => emissiveMap;
            set => Representation.EmissiveMap = emissiveMap = value;
        }

        [VisualizerScalarMaterialParameter]
        public float EmissiveFactor { get; set; } = 1.0f;

        #endregion

        #region Normal

        private TextureModelEx normalMap;
        [VisualizerTextureMaterialParameter(isNormalMap: true)]
        public TextureModelEx NormalMap
        {
            get => normalMap;
            set => Representation.NormalMap = normalMap = value;
        }

        [VisualizerScalarMaterialParameter]
        public float NormalFactor { get; set; } = 1.0f;

        [VisualizerScalarMaterialParameter]
        public float NormalFlatness { get; set; } = 0.0f;

        #endregion

        #region Roughness / Metallic

        [VisualizerScalarMaterialParameter]
        public float MetallicFactor
        {
            get => Representation.MetallicFactor;
            set => Representation.MetallicFactor = value;
        }

        [VisualizerScalarMaterialParameter]
        public float RoughnessFactor
        {
            get => Representation.RoughnessFactor;
            set => Representation.RoughnessFactor = value;
        }

        private TextureModelEx roughnessMetallicMap;
        [VisualizerTextureMaterialParameter]
        public TextureModelEx RoughnessMetallicMap
        {
            get => roughnessMetallicMap;
            set => Representation.RoughnessMetallicMap = roughnessMetallicMap = value;
        }

        #endregion
        
        #region Constructors

        public BasicMaterialModel(Guid id, string name)
            : base(id, name, MaterialType.Basic)
        {
            
        }

        public BasicMaterialModel()
            : base(MaterialType.Basic)
        {

        }

        #endregion
    }
}
