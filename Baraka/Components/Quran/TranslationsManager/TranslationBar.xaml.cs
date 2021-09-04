using Baraka.Data.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Baraka.Theme.UserControls.Quran.TranslationsManager
{
    /// <summary>
    /// Logique d'interaction pour TranslationBar.xaml
    /// </summary>
    public partial class TranslationBar : UserControl
    {
        public TranslationDescription Description { get; set; }

        public bool Selected { get; set; } = false;
        public int Index { get; set; }

        public TranslationBar(TranslationDescription description, int index)
        {
            InitializeComponent();
            
            Description = description;
            Index = index;

            CountryFlagIMG.Source = description.GetFlag();
            LanguageTB.Text = description.LanguageName_EN;
            AuthorsTB.Text = description.Translators;
        }

        #region UI Reactivity
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            BottomSplitterPath.Stroke = (SolidColorBrush)App.Current.Resources["MediumBrush"];
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            BottomSplitterPath.Stroke = Brushes.LightGray;
        }
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e) // debug
        {
            CountryFlagIMG.Source = Description.GetFlag();
            LanguageTB.Text = Description.LanguageName_EN;
            AuthorsTB.Text = Description.Translators;
        }
    }
}