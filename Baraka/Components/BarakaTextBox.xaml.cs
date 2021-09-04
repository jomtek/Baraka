using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Baraka.Theme.UserControls
{
    /// <summary>
    /// Logique d'interaction pour BarakaTextBox.xaml
    /// </summary>
    public partial class BarakaTextBox : UserControl
    {
        private string _placeholder = "rechercher...";
        private bool _placeholderEnabled = true;

        #region Settings
        [Category("Baraka")]
        public string Text
        {
            get { return TextBoxComponent.Text; }
            set
            {
                if (!PlaceholderEnabled)
                {
                    TextBoxComponent.Text = value;
                    if (value.Trim() == "")
                    {
                        TextBoxComponent_LostFocus(null, null);
                    }
                }
            }
        }

        [Category("Baraka")]
        public string Placeholder
        {
            get { return _placeholder; }
            set
            {
                _placeholder = value;
                RefreshPlaceholder();
            }
        }

        [Category("Baraka")]
        public bool PlaceholderEnabled
        {
            get { return _placeholderEnabled; }
        }
        #endregion

        #region Events
        [Category("Baraka")]
        public event EventHandler TextChanged;
        #endregion

        public BarakaTextBox()
        {
            InitializeComponent();
        }

        private void ImageComponent_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ImageComponent.IsMouseOver)
            {
                ImageComponent.Opacity = 0.7;
                Cursor = Cursors.Hand;
            }
            else
            {
                ImageComponent.Opacity = 1;
                Cursor = Cursors.Arrow;
            }
        }

        #region Placeholder
        public void RefreshPlaceholder()
        {
            if (_placeholderEnabled)
            {
                TextBoxComponent.Text = _placeholder;
                TextBoxComponent.Opacity = 0.65;
            }
        }

        private void TextBoxComponent_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxComponent.Opacity == 0.65)
            {
                TextBoxComponent.Clear();
                TextBoxComponent.Opacity = 1;
                _placeholderEnabled = false;
            }
        }

        private void TextBoxComponent_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxComponent.Text.Trim().Length == 0)
            {
                _placeholderEnabled = true;
            }

            RefreshPlaceholder();
        }
        #endregion

        private void TextBoxComponent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_placeholderEnabled)
            {
                TextChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
