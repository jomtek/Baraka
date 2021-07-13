using Baraka.Data.Descriptions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Baraka.Theme.UserControls.Quran.Player
{
    /// <summary>
    /// Logique d'interaction pour CheikhCard.xaml
    /// </summary>
    public partial class CheikhCard : UserControl
    {
        private CheikhDescription _cheikh;
        private BarakaPlayer _parentPlayer;

        private bool _selected = false;

        #region Settings
        public CheikhDescription Cheikh
        {
            get { return _cheikh; }
        }
        #endregion

        public CheikhCard(CheikhDescription cheikh, BarakaPlayer parent)
        {
            InitializeComponent();

            _cheikh = cheikh;
            Initialize();

            _parentPlayer = parent;
        }

        public void Initialize()
        {
            FirstNameTB.Text = _cheikh.FirstName;
            LastNameTB.Text = _cheikh.LastName;
            PhotoRect.Fill = new ImageBrush(_cheikh.GetPhoto());
        }

        private void UserControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Select();
            _parentPlayer.ChangeSelectedCheikhCard(this);
        }

        public void Select()
        {
            SeparatorPath.Stroke = (SolidColorBrush)App.Current.Resources["MediumBrush"];
            SeparatorPath.StrokeThickness = 5.5;

            if (!_selected)
            {
                Height += 10;
                _selected = true;
            }
        }
        public void Unselect()
        {
            SeparatorPath.Stroke = Brushes.Gray;
            SeparatorPath.StrokeThickness = 3;

            if (_selected)
            {
                Height -= 10;
                _selected = false;
            }
        }

        #region UI Reactivity
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            PhotoDropShadow.Opacity = 0.60;
            PhotoDropShadow.ShadowDepth = 4;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            PhotoDropShadow.Opacity = 0.35;
            PhotoDropShadow.ShadowDepth = 2;
        }
        #endregion
    }
}