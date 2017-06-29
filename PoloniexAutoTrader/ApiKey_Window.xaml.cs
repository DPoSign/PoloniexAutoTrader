using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using System.Windows.Input;

namespace PoloniexAutoTrader
{
    /// <summary>
    /// Interaction logic for ApiKey_Window.xaml
    /// </summary>
    public partial class ApiKey_Window
    {
        public ApiKey_Window()
        {
            InitializeComponent();
            PublicKey_Text.Clear();
            PrivateKey_Text.Clear();
            PublicKey_Text.Text = Properties.Settings.Default.PublicKey;
            PrivateKey_Text.Text = Properties.Settings.Default.PrivateKey;
        }

        private async void SaveApiKeys_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.PublicKey = PublicKey_Text.Text.ToString();
            Properties.Settings.Default.PrivateKey = PrivateKey_Text.Text.ToString();
            Properties.Settings.Default.Save();


            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "OK",
                ColorScheme = MetroDialogOptions.ColorScheme
            };

            MessageDialogResult result = await this.ShowMessageAsync("User API Key's", "API Keys Saved Successfully",
            MessageDialogStyle.Affirmative, mySettings);

            if (result == MessageDialogResult.Affirmative)
            {
                // Yes code here
                Visibility = Visibility.Collapsed;
            }

        }
    }
}
