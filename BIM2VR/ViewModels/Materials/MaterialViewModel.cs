using System;
using System.ComponentModel;
using BIM2VR.Models.Materials;
using Prism.Mvvm;
using SharpDX;

namespace BIM2VR.ViewModels.Materials
{
    public abstract class MaterialViewModel : BindableBase
    {
        [Browsable(false)]
        public MaterialModel Model { get; }

        [Browsable(false)]
        public bool IsChanged { get; set; }

        #region Common

        [Category("Common")]
        public string Name
        {
            get => Model.Name;
            set
            {
                Model.Name = value;
                IsChanged = true;
                RaisePropertyChanged(); //Need to update name in ListBox
            }
        }

        #endregion

        #region UV

        [Category("UV Rotation")]
        public float Rotation
        {
            get => Model.UVRotation;
            set
            {
                Model.UVRotation = value;
                IsChanged = true;
            }
        }

        [Category("UV Translation"), DisplayName("X")]
        public float UVTranslationX
        {
            get => Model.UVTranslation.X;
            set
            {
                Model.UVTranslation = new Vector2(value, Model.UVTranslation.Y);
                IsChanged = true;
            }
        }

        [Category("UV Translation"), DisplayName("Y")]
        public float UVTranslationY
        {
            get => Model.UVTranslation.Y;
            set
            {
                Model.UVTranslation = new Vector2(Model.UVTranslation.X, value);
                IsChanged = true;
            }
        }

        [Category("UV Scaling"), DisplayName("X")]
        public float UVScalingX
        {
            get => Model.UVScaling.X;
            set
            {
                Model.UVScaling = new Vector2(value, Model.UVScaling.Y);
                IsChanged = true;
            }
        }

        [Category("UV Scaling"), DisplayName("Y")]
        public float UVScalingY
        {
            get => Model.UVScaling.Y;
            set
            {
                Model.UVScaling = new Vector2(Model.UVScaling.X, value);
                IsChanged = true;
            }
        }

        #endregion

        #region Constructors

        protected MaterialViewModel(MaterialModel model)
        {
            Model = model;
        }

        #endregion

        #region Overrided

        public override bool Equals(object obj)
        {
            if (obj is MaterialViewModel materialViewModel)
            {
                return Model == materialViewModel.Model;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Model.GetHashCode();
        }

        public override string ToString()
        {
            return Model.Name;
        }

        #endregion

    }
}
