using System;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace BIM2VR.Models.Materials
{
    public enum MaterialType
    {
        Basic,
        Color
    }

    public class HelixMaterialModelTypeItemsSource : IItemsSource
    {
        public ItemCollection GetValues()
        {
            var types = new ItemCollection();

            var values = Enum.GetValues(typeof(MaterialType));

            foreach (MaterialType value in values)
            {
                types.Add(value);
            }

            return types;
        }
    }
}
