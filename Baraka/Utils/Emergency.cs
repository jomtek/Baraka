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
        public static void ShowExMessage(Exception ex)
        {
            var sb = new StringBuilder();

            switch (ex)
            {
                case WebException wex:
                    sb.Append("Impossible de récupérer l'information à partir du serveur distant.");
                    sb.AppendLine();
                    sb.AppendLine($"Message: {wex.Message}");
                    sb.AppendLine();
                    sb.AppendLine($"- Veuillez vérifier votre connexion internet");
                    sb.AppendLine($"- Si le problème persiste, contactez-nous (Paramètres -> Aide)");
                    break;

                case NAudio.MmException mmex:
                    sb.Append("Impossible de jouer le verset demandé.");
                    sb.AppendLine();
                    sb.AppendLine($"Message: {mmex.Message}");
                    sb.AppendLine();
                    sb.AppendLine($"- Veuillez vérifier l'état de votre périphérique de sortie audio");
                    sb.AppendLine($"- Veuillez sélectionner un autre périphérique de sortie audio (Paramètres -> Lecture -> Audio)");
                    sb.AppendLine($"- Si le problème persiste, contactez-nous (Paramètres -> Aide)");
                    break;

                default:
                    throw new NotImplementedException();
            }

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
        }
    }
}
