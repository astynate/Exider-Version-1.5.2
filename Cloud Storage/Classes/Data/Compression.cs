using System.Drawing;

namespace Cloud_Storage.Classes.Data
{
    public static class Compression
    {
        public static byte[] ResizeImage(byte[] imageBytes, int height, int width)
        {
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                using (Image image = Image.FromStream(ms))
                {
                    using (Bitmap resizedImage = new Bitmap(width, height))
                    {
                        using (Graphics graphics = Graphics.FromImage(resizedImage))
                        {
                            graphics.DrawImage(image, 0, 0, width, height);
                        }

                        using (MemoryStream resizedMs = new MemoryStream())
                        {
                            resizedImage.Save(resizedMs, image.RawFormat);
                            return resizedMs.ToArray();
                        }
                    }
                }
            }
        }

        public static byte[]? ResizeImageWidth(byte[] imageBytes, int width)
        {

            if (imageBytes != null)
            {

                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    using (Image image = Image.FromStream(ms))
                    {

                        int ratio = image.Width / width;
                        int height = Convert.ToInt32(image.Height / ratio);

                        return ResizeImage(imageBytes, height, width);

                    }
                }

            }

            return null;

        }

    }

}
