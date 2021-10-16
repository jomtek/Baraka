using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Models
{
    public class QariModel
    {
        public readonly string FirstName;
        public readonly string LastName;
        public readonly string Id; // The ID which identifies the Qari on the reciter API

        public string FullName
        {
            get { return FirstName + ' ' + LastName; }
        }

        public QariModel(string firstName, string lastName, string id)
        {
            FirstName = firstName;
            LastName = lastName;
            Id = id;
        }
    }
}
