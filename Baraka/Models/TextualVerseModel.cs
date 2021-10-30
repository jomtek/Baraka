using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Models
{
    public class TextualVerseModel
    {
        public int Number { get; set; }
        public VerseEditionModel Arabic { get; set; }
        public VerseEditionModel Transliteration { get; set; }
        public VerseEditionModel Translation1 { get; set; }
        public VerseEditionModel Translation2 { get; set; }
        public VerseEditionModel Translation3 { get; set; }

        public TextualVerseModel()
        {
            Arabic = new VerseEditionModel(false);
            Transliteration = new VerseEditionModel(false);
            Translation1 = new VerseEditionModel(false);
            Translation2 = new VerseEditionModel(false);
            Translation3 = new VerseEditionModel(false);
        }
    }
}
