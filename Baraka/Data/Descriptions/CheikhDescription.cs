using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Baraka.Data.Descriptions
{
    public class CheikhDescription
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public ImageSource Photo { get; private set; }

        public CheikhDescription(string firstName, string lastName, ImageSource photo)
        {
            FirstName = firstName;
            LastName = lastName;
            Photo = photo;
        }

        public override string ToString()
        {
            return FirstName + ' ' + LastName;
        }
    }
}
