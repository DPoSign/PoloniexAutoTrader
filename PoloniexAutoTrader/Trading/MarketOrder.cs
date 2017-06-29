using Jojatekok.PoloniexAPI;
using Jojatekok.PoloniexAPI.MarketTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoloniexAutoTrader.Market;


namespace PoloniexAutoTrader.Trading
{
    public class MarketOrder
    {

        Client Client = new Client();
        GetCurrentPrice currentPrice = new GetCurrentPrice();

        // Execute Market Order from top of order book
        public async Task ExecuteMarketOrder(CurrencyPair symbol, OrderType orderType, double quantity)
        {
            if (orderType == OrderType.Buy)
            {

                double topOrderBuy = await currentPrice.CurrentMarketPrice(symbol, OrderType.Buy);
                var ExecuteBuyOrder = await Client.PoloniexClient.Trading.PostOrderAsync(symbol, OrderType.Buy, topOrderBuy, quantity);

            }
            else if (orderType == OrderType.Sell)
            {
                double topOrderSell = await currentPrice.CurrentMarketPrice(symbol, OrderType.Sell);
                var ExecuteSellOrder = await Client.PoloniexClient.Trading.PostOrderAsync(symbol, OrderType.Sell, topOrderSell, quantity);
            }
        }

    }
}
