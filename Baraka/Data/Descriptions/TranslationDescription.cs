using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Baraka.Data.Descriptions
{
    [Serializable]
    public class TranslationDescription
    {
        public string FlagName_ISO { get; set; }
        public string LanguageName_ISO { get; set; }
        public string LanguageName_EN { get; set; }
        public string Translators { get; set; }
        public string TanzilEditionName { get; set; }
        public string Identifier
        {
            get
            {
                return $"{LanguageName_ISO}.{TanzilEditionName}";
            }
        }

        private byte[] _flag;

        public TranslationDescription(
            string flagnameIso,
            string langnameIso,
            string langnameEn,
            string translators,
            string tanzilEditionName,
            BitmapImage flag)
        {
            FlagName_ISO = flagnameIso;
            LanguageName_ISO = langnameIso;
            LanguageName_EN = langnameEn;
            Translators = translators;
            TanzilEditionName = tanzilEditionName;
            _flag = ConvertToBytes(flag);
        }

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

        public BitmapImage GetFlag()
        {
            using (var ms = new MemoryStream(_flag))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
    }
}
