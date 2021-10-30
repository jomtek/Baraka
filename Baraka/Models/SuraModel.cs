using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Models
{
    public class SuraModel
    {
        public int Number { get; } // Starts from one
        public int Length { get; }
        public string PhoneticName { get; }
        public string TranslatedName { get; }
        public SuraRevelationType RevelationType { get; }

        public SuraModel(
            int number,
            int length,
            string phoneticName,
            string translatedName, // TODO: temporary value
            SuraRevelationType revelationType)
        {
            Number = number;
            Length = length;
            PhoneticName = phoneticName;
            TranslatedName = translatedName;
            RevelationType = revelationType;
        }
    }
}
