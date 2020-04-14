using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace BIM2VR.Models.Images
{
    public static class ImageManager
    {
        private static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

        private static byte[] ReadAllBytes(string fileName)
        {
            var byteArray = new byte[] { };

            var chunkSize = 16384;
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream, new ASCIIEncoding()))
                {
                    var chunk = reader.ReadBytes(chunkSize);
                    while (chunk.Length > 0)
                    {
                        byteArray = Combine(byteArray, chunk);
                        chunk = reader.ReadBytes(chunkSize);
                    }
                }
            }

            return byteArray;
        }

        public static BitmapImage LoadBitmap(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    var imageByteArray = ReadAllBytes(fileName);

                    if (imageByteArray != null)
                    {
                        try
                        {
                            using (var stream = new MemoryStream(imageByteArray))
                            {
                                stream.Position = 0;
                                var picture = new BitmapImage();
                                picture.BeginInit();
                                picture.StreamSource = stream;
                                picture.CacheOption = BitmapCacheOption.OnLoad;
                                picture.EndInit();
                                picture.Freeze();

                                return picture;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    return null;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
