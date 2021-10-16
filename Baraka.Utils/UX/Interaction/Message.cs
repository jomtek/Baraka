using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Baraka.Utils.UX.Interaction
{
    public abstract class Message
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            private set { _title = value; }
        }

        private string _content;
        public string Content
        {
            get { return _content; }
            private set { _content = value; }
        }

        public Message(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public abstract void Show();
    }
}
