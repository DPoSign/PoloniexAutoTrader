using Jojatekok.PoloniexAPI;
using Jojatekok.PoloniexAPI.MarketTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoloniexAutoTrader.Trading
{
    public class LimitOrder
    {

       Client Client = new Client();

        // Execute Limit Order
        async Task ExecuteLimitOrder(CurrencyPair symbol, OrderType orderType, double priceLimit, double quantity)
        {
            if (orderType == OrderType.Buy)
            {
                var ExecuteBuyOrder = await Client.PoloniexClient.Trading.PostOrderAsync(symbol, OrderType.Buy, priceLimit, quantity);
            }
            else if (orderType == OrderType.Sell)
            {
                var ExecuteSellOrder = await Client.PoloniexClient.Trading.PostOrderAsync(symbol, OrderType.Sell, priceLimit, quantity);
            }
        }
    }
}
