using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Models
{
    public class VerseEditionModel
    {
        public bool IsActive { get; }
        public string Id { get; }
        public string Text { get; }

        public VerseEditionModel(bool isActive, string id = null, string text = null)
        {
            IsActive = isActive;
            Id = id;
            Text = text;
        }
    }
}
