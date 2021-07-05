using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Baraka.Utils
{
    public static class Emergency
    {
        public static string ShowExMessage(Exception ex)
        {
            switch (ex)
            {
                case WebException wex:
                    var sb = new StringBuilder("Impossible de récupérer l'information à partir du serveur distant.");
                    sb.AppendLine();
                    sb.AppendLine($"Message: {wex.Message}");
                    sb.AppendLine();
                    sb.AppendLine($"- Veuillez vérifier votre connexion internet");
                    sb.AppendLine($"- Si le problème persiste, contactez-nous : TODO");


                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show(
                            Application.Current.MainWindow,
                            sb.ToString(),
                            "Baraka",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation
                        );
                    }));
                    

                    return sb.ToString();
            }

            throw new NotImplementedException();
        }
    }
}
