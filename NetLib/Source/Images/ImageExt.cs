using System.Drawing;
using System.IO;
using JetBrains.Annotations;

namespace NetLib.Images
{
    [PublicAPI]
    public static class ImageExt
    {
        public static byte[] ImageToByteArray(this Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }

        public static Image ByteArrayToImage(this byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                var image = Image.FromStream(ms);
                return image;
            }
        }
    }
}
