using System;

namespace BIM2VR.Models.Visualizer.Data.MaterialSource.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class VisualizerMaterial : Attribute
    {
        public string Name { get; set; }

        public VisualizerMaterial(string name)
        {
            Name = name;
        }
    }
}
