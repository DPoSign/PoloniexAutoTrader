using Jojatekok.PoloniexAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PoloniexAutoTrader.Wallet
{
    public class GetBalances
    {

        Client Client = new Client();

        public async Task<double> GetBalance(string symbol)
        {
            double balance = 0;

            var wallet = await Client.PoloniexClient.Wallet.GetBalancesAsync();

            foreach (var b in wallet)
            {
                if (b.Key == symbol)
                {
                    balance = b.Value.BitcoinValue;
                }
            }
            return balance;
        }

        // Get / Set and return Quote balance
        //public async Task<double> GetTotalBTC()
        public async Task<double> GetBalance()
        {
            double totalAll = 0;
            var wallet = await Client.PoloniexClient.Wallet.GetBalancesAsync();

            // Show quote balances in Balance Datagrid
            foreach (var b in wallet)
            {
                    totalAll += b.Value.BitcoinValue;
            }
            return totalAll;
        }

    }
}

