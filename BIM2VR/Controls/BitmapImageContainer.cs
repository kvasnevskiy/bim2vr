using System.Windows.Media.Imaging;

namespace BIM2VR.Controls
{
    public class BitmapImageContainer
    {
        public string Path { get; set; }
        public BitmapImage Source { get; set; }

        #region Constructors

        public BitmapImageContainer(string path, BitmapImage source)
        {
            Path = path;
            Source = source;
        }

        public BitmapImageContainer(BitmapImageContainer container)
        {
            Path = container.Path;
            Source = container.Source?.Clone();
        }

        #endregion
    }
}
