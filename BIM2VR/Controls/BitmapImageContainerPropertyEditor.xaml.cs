using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BIM2VR.Models.Images;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace BIM2VR.Controls
{
    public partial class BitmapImageContainerPropertyEditor : UserControl, ITypeEditor
    {
        private readonly Dictionary<string, string> extensions = new Dictionary<string, string>
        {
            {"Textures", "*.jpg;*.jpeg;*.png"}
        };

        public BitmapImageContainerPropertyEditor()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(BitmapImageContainer), typeof(BitmapImageContainerPropertyEditor),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public BitmapImageContainer Value
        {
            get => (BitmapImageContainer)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private void SelectTextureClickHandler(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.OpenFileDialog
            {
                Filter = extensions.Select(x => x.Key + "|" + x.Value).Aggregate((a, b) => a + "|" + b)
            };

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var bitmapImage = ImageManager.LoadBitmap(dlg.FileName);
                Value = new BitmapImageContainer(dlg.FileName, bitmapImage);
            }
        }

        private void RemoveTextureClickHandler(object sender, RoutedEventArgs e)
        {
            Value = new BitmapImageContainer(string.Empty, null);
        }

        public FrameworkElement ResolveEditor(Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem propertyItem)
        {
            var binding = new Binding("Value")
            {
                Source = propertyItem,
                Mode = propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay
            };

            BindingOperations.SetBinding(this, ValueProperty, binding);

            return this;
        }
    }
}
