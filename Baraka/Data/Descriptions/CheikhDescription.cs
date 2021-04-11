using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Baraka.Data.Descriptions
{
    [Serializable]
    public class CheikhDescription
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string StreamingUrl { get; private set; }

        private byte[] _photo;

        private byte[] ConvertToBytes(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        public BitmapImage GetPhoto()
        {
            using (var ms = new MemoryStream(_photo))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        public CheikhDescription(string firstName, string lastName, string streamingUrl, BitmapImage photo)
        {
            FirstName = firstName;
            LastName = lastName;
            StreamingUrl = streamingUrl;
            _photo = ConvertToBytes(photo);
        }

        public override string ToString()
        {
            return FirstName + ' ' + LastName;
        }
    }
}
