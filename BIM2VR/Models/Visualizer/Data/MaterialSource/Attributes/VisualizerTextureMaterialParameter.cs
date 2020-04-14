using System;

namespace BIM2VR.Models.Visualizer.Data.MaterialSource.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class VisualizerTextureMaterialParameter : VisualizerMaterialParameter
    {
        public bool IsNormalMap { get; set; }

        public VisualizerTextureMaterialParameter(bool isNormalMap = false)
        {
            IsNormalMap = true;
        }
    }
}
