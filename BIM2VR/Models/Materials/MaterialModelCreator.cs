using BIM2VR.Extensions;
using System.Linq;
using System.Windows.Media;
using Xbim.Common;
using Xbim.Ifc;

namespace BIM2VR.Models.Materials
{
    public static class MaterialModelCreator
    {
        private static readonly XbimColourMap colourMap = new XbimColourMap();

        public static MaterialModel Create(IModel model, short typeId)
        {
            var prodType = model.Metadata.ExpressType(typeId);
            var texture = XbimTexture.Create(colourMap[prodType.Name]);
            var xBimColor = texture.ColourMap.FirstOrDefault();
            return Create(Color.FromScRgb(xBimColor.Alpha, xBimColor.Red, xBimColor.Green, xBimColor.Blue));
        }

        public static ColorMaterialModel Create(Color color)
        {
            return new ColorMaterialModel(color);
        }
    }
}
