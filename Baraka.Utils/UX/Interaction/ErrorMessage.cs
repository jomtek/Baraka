using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Utils.UX.Interaction
{
    public class ErrorMessage : Message
    {
        public ErrorMessage(string content, string title = "Baraka")
            : base(title, content) {}

        public override void Show()
        {
            MessageBox.Show(Content, Title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
