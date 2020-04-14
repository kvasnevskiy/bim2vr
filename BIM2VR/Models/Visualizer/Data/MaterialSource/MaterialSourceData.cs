using System;
using System.Collections.Generic;
using System.Linq;
using BIM2VR.Models.Materials;
using BIM2VR.Models.Visualizer.Data.CommonParameters;
using BIM2VR.Models.Visualizer.Data.MaterialSource.Attributes;
using SharpDX;
using Color = System.Windows.Media.Color;

namespace BIM2VR.Models.Visualizer.Data.MaterialSource
{
    public class MaterialSourceData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public List<Dictionary<string, dynamic>> Parameters { get; set; }

        public MaterialSourceData(MaterialModel material)
        {
            Id = material.Id;
            Name = material.Name;

            //Set type
            var visualizerMaterialAttribute = (VisualizerMaterial)material.GetType().GetCustomAttributes(typeof(VisualizerMaterial), true).First();
            Type = visualizerMaterialAttribute.Name;

            //Set parameters
            Parameters = new List<Dictionary<string, dynamic>>();

            var propertyInfos = material.GetType().GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                var attributes = propertyInfo.GetCustomAttributes(typeof(VisualizerMaterialParameter), true);

                if (attributes.Length > 0)
                {
                    switch (attributes.First())
                    {
                        case VisualizerScalarMaterialParameter _:
                            var scalar = (float)propertyInfo.GetValue(material);
                            Parameters.Add(new Dictionary<string, dynamic>
                            {
                                {"Name", propertyInfo.Name},
                                {"Type", "Scalar"},
                                {"Value", scalar}
                            });
                            break;
                        case VisualizerColorMaterialParameter _:
                            var color = (Color)propertyInfo.GetValue(material);
                            Parameters.Add(new Dictionary<string, dynamic>
                            {
                                {"Name", propertyInfo.Name},
                                {"Type", "RGBA"},
                                {"Value", new RGBAParameter(color)}
                            });
                            break;
                        case VisualizerTextureMaterialParameter _:
                            var texture = (TextureModelEx)propertyInfo.GetValue(material);
                            if (texture != null)
                            {
                                Parameters.Add(new Dictionary<string, dynamic>
                                {
                                    {"Name", propertyInfo.Name},
                                    {"Type", "TextureId"},
                                    {"Value", texture.Id}
                                });
                            }
                            break;
                        case VisualizerVector2DMaterialParameter _:
                            var vector2 = (Vector2)propertyInfo.GetValue(material);
                            Parameters.Add(new Dictionary<string, dynamic>
                            {
                                {"Name", propertyInfo.Name},
                                {"Type", "V2"},
                                {"Value", new Vector2DParameter(vector2)}
                            });
                            break;
                    }
                }
            }
        }
    }
}
