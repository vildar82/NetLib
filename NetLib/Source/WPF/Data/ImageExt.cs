namespace NetLib.WPF.Data
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using JetBrains.Annotations;

    [PublicAPI]
    public static class ImageExt
    {
        public static BitmapImage ConvertToBitmapImage(this Image image)
        {
            if (image == null)
                return null;

            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }

        public static byte[] ToArray(this ImageSource image)
        {
            var converter = new ImageSourceConverter();
            return (byte[])converter.ConvertTo(image, typeof(byte[]));
        }

        public static ImageSource ToImage(this byte[] array)
        {
            var converter = new ImageSourceConverter();
            return (ImageSource)converter.ConvertFrom(array);
        }
    }
}