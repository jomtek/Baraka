using Baraka.Models.Quran;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Baraka.Singletons;

namespace Baraka.Converters.Player
{
    public class RecitationModeToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string mode)
            {
                switch (mode)
                {
                    case "":
                        return "classique";
                    case "mujawwad":
                        return "mujawwad مجود";
                    case "murattal":
                        return "murattal مرتل";
                }
            }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
