using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using BIM2VR.Models.Visualizer.Data.MaterialSource.Attributes;
using SharpDX;

namespace BIM2VR.Models.Materials
{
    public abstract class MaterialModel
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public MaterialType Type { get;  } 
        public PBRMaterialCore Representation { get; }
        public List<Guid> Owners { get; }

        public void SetOwners(IEnumerable<Guid> owners)
        {
            Owners.AddRange(owners);
        }

        #region UV

        [VisualizerScalarMaterialParameter]
        public float UVRotation
        {
            get => Representation.UVTransform.Rotation;
            set => Representation.UVTransform = new UVTransform(value, UVScaling, UVTranslation);
        }

        [VisualizerVector2DMaterialParameter]
        public Vector2 UVTranslation
        {
            get => Representation.UVTransform.Translation;
            set => Representation.UVTransform = new UVTransform(UVRotation, UVScaling, value);
        }

        [VisualizerVector2DMaterialParameter]
        public Vector2 UVScaling
        {
            get => Representation.UVTransform.Scaling;
            set => Representation.UVTransform = new UVTransform(UVRotation, value, UVTranslation);
        }

        #endregion

        #region Constructors

        protected MaterialModel(Guid id, string name, MaterialType type)
        {
            Id = id;
            Name = name;
            Type = type;
            Owners = new List<Guid>();
            Representation = new PBRMaterialCore()
            {
                UVTransform = new UVTransform(0.0f, new Vector2(0.001f, 0.001f), new Vector2()),
                RenderEnvironmentMap = true,
                EnableAutoTangent = true,
                EnableTessellation = true,
                MetallicFactor = 0.2f,
                RoughnessFactor = 0.5f,
                MaxDistanceTessellationFactor = 2,
                MinDistanceTessellationFactor = 4,
                RenderShadowMap = true,
                EnableFlatShading = true,
                RenderAmbientOcclusionMap = false //Disable AO for SSAO
            };
        }

        protected MaterialModel(MaterialType type)
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Type = type;
            Owners = new List<Guid>();
            Representation = new PBRMaterialCore()
            {
                UVTransform = new UVTransform(0.0f, new Vector2(0.001f, 0.001f), new Vector2()),
                RenderEnvironmentMap = true,
                EnableAutoTangent = true,
                EnableTessellation = true,
                MetallicFactor = 0.2f,
                RoughnessFactor = 0.5f,
                MaxDistanceTessellationFactor = 2,
                MinDistanceTessellationFactor = 4,
                RenderShadowMap = true,
                EnableFlatShading = true,
                RenderAmbientOcclusionMap = false //Disable AO for SSAO
            };
        }

        #endregion

        #region Overrided

        public override bool Equals(object obj)
        {
            if (obj is MaterialModel material)
            {
                return Id == material.Id;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        #region PBR

        public PBRMaterial ToPBR()
        {
            return Representation.ConvertToPBRMaterial();
        }

        #endregion
    }
}