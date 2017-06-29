using Jojatekok.PoloniexAPI;
using Jojatekok.PoloniexAPI.MarketTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoloniexAutoTrader.Market
{
    public class GetCurrentPrice
    {
        Client Client = new Client();

        //Get Top OrderBook Quote
        public async Task<double> CurrentMarketPrice(CurrencyPair symbol, OrderType orderType)
        {
            double price = 0;

            var marketInfo = await Client.PoloniexClient.Markets.GetSummaryAsync();

            foreach (var m in marketInfo)
            {
                //if (m.Key.BaseCurrency == symbol1 && m.Key.QuoteCurrency == symbol2)
                if (m.Key == symbol)
                {
                    if (orderType == OrderType.Buy)
                    {
                        price = m.Value.OrderTopBuy;
                    }
                    else if (orderType == OrderType.Sell)
                    {
                        price = m.Value.OrderTopBuy;
                    }
                }
            }
            return price;
        }
    }

}

