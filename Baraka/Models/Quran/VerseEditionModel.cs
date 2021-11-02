using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Models.Quran
{
    public class VerseEditionModel<T>
    {
        public bool IsActive { get; }
        public string Id { get; }
        public T Content { get; }

        public VerseEditionModel(bool isActive, T content, string id = null)
        {
            IsActive = isActive;
            Id = id;
            Content = content;
        }
    }
}
