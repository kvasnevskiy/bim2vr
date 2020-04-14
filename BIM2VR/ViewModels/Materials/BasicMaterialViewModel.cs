using System.ComponentModel;
using System.Windows.Media;
using BIM2VR.Controls;
using BIM2VR.Models.Images;
using BIM2VR.Models.Materials;
using HelixToolkit.Wpf.SharpDX;

namespace BIM2VR.ViewModels.Materials
{
    public class BasicMaterialViewModel : MaterialViewModel
    {
        [Browsable(false)]
        public new BasicMaterialModel Model => base.Model as BasicMaterialModel;

        #region Albedo

        [Category("Albedo")]
        public Color AlbedoColor
        {
            get => Model.AlbedoColor;
            set
            {
                Model.AlbedoColor = value;
                IsChanged = true;
            }
        }

        private BitmapImageContainer albedoMap;

        [Category("Albedo")]
        [Editor(typeof(BitmapImageContainerPropertyEditor), typeof(BitmapImageContainerPropertyEditor))]
        public BitmapImageContainer AlbedoMap
        {
            get => albedoMap;
            set
            {
                albedoMap = value;
                Model.AlbedoMap = new TextureModelEx(new TextureModel(albedoMap.Source.ToMemoryStream()), albedoMap.Path);
                IsChanged = true;
            }
        }

        #endregion

        #region Normal

        [Category("Normal")]
        public float NormalFactor
        {
            get => Model.NormalFactor;
            set
            {
                Model.NormalFactor = value;
                IsChanged = true;
            }
        }

        [Category("Normal")]
        public float NormalFlatness
        {
            get => Model.NormalFlatness;
            set
            {
                Model.NormalFlatness = value;
                IsChanged = true;
            }
        }

        private BitmapImageContainer normalMap;

        [Category("Normal")]
        [Editor(typeof(BitmapImageContainerPropertyEditor), typeof(BitmapImageContainerPropertyEditor))]
        public BitmapImageContainer NormalMap
        {
            get => normalMap;
            set
            {
                normalMap = value;
                Model.NormalMap = new TextureModelEx(new TextureModel(normalMap.Source.ToMemoryStream()), normalMap.Path);
                IsChanged = true;
            }
        }

        #endregion

        #region Emissive

        [Category("Emissive")]
        public Color EmissiveColor
        {
            get => Model.EmissiveColor;
            set
            {
                Model.EmissiveColor = value;
                IsChanged = true;
            }
        }

        [Category("Emissive")]
        public float EmissiveFactor
        {
            get => Model.EmissiveFactor;
            set
            {
                Model.EmissiveFactor = value;
                IsChanged = true;
            }
        }


        private BitmapImageContainer emissiveMap;

        [Category("Emissive")]
        [Editor(typeof(BitmapImageContainerPropertyEditor), typeof(BitmapImageContainerPropertyEditor))]
        public BitmapImageContainer EmissiveMap
        {
            get => emissiveMap;
            set
            {
                emissiveMap = value;
                Model.EmissiveMap = new TextureModelEx(new TextureModel(emissiveMap.Source.ToMemoryStream()), emissiveMap.Path);
                IsChanged = true;
            }
        }

        #endregion

        #region Roughness / Metallic

        [Category("Roughness / Metallic")]
        public float MetallicFactor
        {
            get => Model.MetallicFactor;
            set
            {
                Model.MetallicFactor = value;
                IsChanged = true;
            }
        }

        [Category("Roughness / Metallic")]
        public float RoughnessFactor
        {
            get => Model.RoughnessFactor;
            set
            {
                Model.RoughnessFactor = value;
                IsChanged = true;
            }
        }

        private BitmapImageContainer roughnessMetallicMap;

        [Category("Roughness / Metallic")]
        [Editor(typeof(BitmapImageContainerPropertyEditor), typeof(BitmapImageContainerPropertyEditor))]
        public BitmapImageContainer RoughnessMetallicMap
        {
            get => roughnessMetallicMap;
            set
            {
                roughnessMetallicMap = value;
                Model.RoughnessMetallicMap = new TextureModelEx(new TextureModel(roughnessMetallicMap.Source.ToMemoryStream()), roughnessMetallicMap.Path);
                IsChanged = true;
            }
        }

        #endregion

        #region Constructors

        public BasicMaterialViewModel(MaterialModel model)
            : base(model)
        {
            albedoMap = albedoMap != null ? new BitmapImageContainer(Model.AlbedoMap.TexturePath, ImageManager.LoadBitmap(Model.AlbedoMap.TexturePath)) : null;
            normalMap = normalMap != null ? new BitmapImageContainer(Model.NormalMap.TexturePath, ImageManager.LoadBitmap(Model.NormalMap.TexturePath)) : null;
            emissiveMap = emissiveMap != null ? new BitmapImageContainer(Model.EmissiveMap.TexturePath, ImageManager.LoadBitmap(Model.EmissiveMap.TexturePath)) : null;
            roughnessMetallicMap = roughnessMetallicMap != null ? new BitmapImageContainer(Model.RoughnessMetallicMap.TexturePath, ImageManager.LoadBitmap(Model.RoughnessMetallicMap.TexturePath)) : null;
        }

        #endregion

    }
}
