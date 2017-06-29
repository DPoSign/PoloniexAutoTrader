using Jojatekok.PoloniexAPI;
using System.Windows;

namespace PoloniexAutoTrader
{
    public class Client
    {
        public PoloniexClient PoloniexClient { get; }

        public Client()
        {
            PoloniexClient = new PoloniexClient(Properties.Settings.Default.PublicKey, Properties.Settings.Default.PrivateKey);

            if (!Application.Current.Properties.Contains("PoloniexClient"))
            {
                Application.Current.Properties.Add("PoloniexClient", PoloniexClient);
            }

            else
            {
                PoloniexClient = (PoloniexClient)Application.Current.Properties["PoloniexClient"];
            }
        }
    }
}