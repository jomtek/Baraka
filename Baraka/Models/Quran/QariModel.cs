using System;

namespace Baraka.Models.Quran
{
    public class QariModel : IEquatable<QariModel>
    {
        public string Id { get; set; } // The ID which identifies the Qari on the reciter API
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public readonly bool AcModelAvailable; // Acoustic model trained with sphinxtrain
        //public readonly bool TwoModes;

        public string FullName
        {
            get { return FirstName + ' ' + LastName; }
        }

        public string Url
        {
            get
            {
                var api = (string)App.Current.FindResource("API_PATH");
                return $@"{api}\reciters\{Id}";
            }
        }

        public string ImageUrl
        {
            get { return Url + @"\image.png";  }
        }


        public QariModel()
        {
            
        }
        public QariModel(string id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public bool Equals(QariModel other)
        {
            return Id == other.Id;
        }
    }
}
