using Jojatekok.PoloniexAPI;
using Jojatekok.PoloniexAPI.MarketTools;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PoloniexAutoTrader.Strategies
{
    class IBS : Strategy
    {

        double topBuyPrice;
        double topSellPrice;

        public IBS(string strategyName, MarketPeriod marketSeries, CurrencyPair symbol, bool? buy, bool? sell, double volume) : base(strategyName, marketSeries, symbol, buy, sell, volume)
        {
        }

        public override void OnStart()
        {
            // Output window
            outputData = new Strategy1Data();
            outputData.Show();
            outputData.Title = StrategyName + " " + Symbol;
        }

        public override async Task OnBar()
        {
            await IbsBot();
        }

        public override void OnTick(TickerChangedEventArgs ticker)
        {
            // Last ticker price


            if (ticker.CurrencyPair == Symbol)
            {

                Debug.WriteLine("TOP BUY " + ticker.MarketData.OrderTopBuy);

                Debug.WriteLine("TOP SELL " + ticker.MarketData.OrderTopSell);

                topBuyPrice = ticker.MarketData.OrderTopBuy;
                topSellPrice = ticker.MarketData.OrderTopSell;
            }
        }

        public override void OnStop()
        {

        }

        public async Task IbsBot()
        {           

            DateTime startdate = new DateTime(2017, 1, 1);
            DateTime enddate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0);

            var canldeinfo = await Client.PoloniexClient.Markets.GetChartDataAsync(Symbol, MarketSeries, startdate, enddate);
            var candleindex = canldeinfo.Count() - 1;

            var ibs = ((canldeinfo[candleindex].Close - canldeinfo[candleindex - 1].Low) / (canldeinfo[candleindex].High - canldeinfo[candleindex].Low)) * 100;

            // Output IBS to datawindow
            outputData.Strategy1Output.Text += "IBS" + "\n" + ibs + "\n" + DateTime.Now.ToString() + "\n" + "-------------------" + "\n";

            // 0.15% fee
            if ((bool)Buy)
            {
                    if (ibs < 10)
                    {
                        //double pricePerCoinBuy = await currentPrice.CurrentMarketPrice(Symbol, OrderType.Buy);
                        var volume = quantity.TotalToQuantity(Total, topBuyPrice);
                        await marketOrder.ExecuteMarketOrder(Symbol, OrderType.Buy, volume);
                        // Output IBS to datawindow

                        string tradeOutputBuy = DateTime.Now + "\n" + "Volume = " + volume + "\n" + OrderType.Buy + "\n" + "IBS = " + ibs + "\n" + "-------------------" + "\n";
                        Debug.WriteLine(tradeOutputBuy);
                        outputData.Strategy1Output.Text += tradeOutputBuy;
                    }
            }
            

            // 0.25% fee
            if ((bool)Sell)
            {
                if (ibs > 90)
                {
                    var quoteBalance = balance.GetBalance(Symbol.QuoteCurrency);
                    var balanceQuote = quoteBalance.Result;

                    if (balanceQuote > minOrderSize)
                    {
                        //double pricePerCoinSell = await currentPrice.CurrentMarketPrice(Symbol, OrderType.Sell);
                        var volume = quantity.TotalToQuantity(Total, topSellPrice);
                        await marketOrder.ExecuteMarketOrder(Symbol, OrderType.Sell, volume);

                        // Output IBS to datawindow
                        string tradeOutputSell = DateTime.Now + "\n" + "Volume = " + volume + "\n" + OrderType.Sell + "\n" + "IBS = " + ibs + "\n" + "-------------------" + "\n";
                        Debug.WriteLine(tradeOutputSell);
                        outputData.Strategy1Output.Text += tradeOutputSell;
                    }
                }
            }
        }

    }
}
