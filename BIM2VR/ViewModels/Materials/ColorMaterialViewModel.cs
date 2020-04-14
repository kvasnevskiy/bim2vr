using System.Windows.Media;
using BIM2VR.Models.Materials;
using System.ComponentModel;

namespace BIM2VR.ViewModels.Materials
{
    public class ColorMaterialViewModel : MaterialViewModel
    {
        [Browsable(false)]
        public new ColorMaterialModel Model => base.Model as ColorMaterialModel;

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

        #endregion

        #region Constructors

        public ColorMaterialViewModel(MaterialModel model)
            : base(model)
        {
        }

        #endregion
    }
}
