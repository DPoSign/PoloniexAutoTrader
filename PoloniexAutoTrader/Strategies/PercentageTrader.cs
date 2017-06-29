using Jojatekok.PoloniexAPI;
using Jojatekok.PoloniexAPI.MarketTools;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PoloniexAutoTrader.Strategies
{
    class PercentageTrader : Strategy
    {
        double topBuyPrice;
        double topSellPrice;

        public PercentageTrader(string strategyName, MarketPeriod marketSeries, CurrencyPair symbol, bool? buy, bool? sell, double volume) : base(strategyName, marketSeries, symbol, buy, sell, volume)
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

            PercentChange(ticker);

        }

        public override void OnStop()
        {

        }

        // Percentage Change
        public void PercentChange(TickerChangedEventArgs ticker)
        {
            DateTime startdate = new DateTime(2017, 1, 1);
            DateTime enddate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0);

            var CandleSeries = Client.PoloniexClient.Markets.GetChartDataAsync(Symbol, MarketSeries, startdate, enddate);
            var candleSeries = CandleSeries.Result;
            var index = candleSeries.Count() - 2;

            //Percent Drop Calc
            if (ticker.CurrencyPair == Symbol)
            {
                double lastClose = candleSeries[index].Close;

                double percentChange = Math.Round(((ticker.MarketData.PriceLast - lastClose) / ticker.MarketData.PriceLast) * 100, 4);

                double tickerPercentChange = ticker.MarketData.PriceChangePercentage;

                // Output IBS to datawindow 

                outputData.Dispatcher.Invoke(new Action(() =>
               {
                   outputData.Strategy1Output.Text += "Percentage Change" + "\n" + percentChange + "\n" + DateTime.Now.ToString() + "\n" + "-------------------" + "\n";
                   outputData.Strategy1Output.Text += "Ticker Change" + "\n" + tickerPercentChange + "\n" + DateTime.Now.ToString() + "\n" + "-------------------" + "\n";
                   outputData.Strategy1Output.Text += "Last Close" + "\n" + lastClose + "\n" + candleSeries[index].Time + "\n" + "-------------------" + "\n";
                   outputData.Strategy1Output.Text += "Last Ticker Price" + "\n" + ticker.MarketData.PriceLast + "\n" + DateTime.Now.ToString() + "\n" + "-------------------" + "\n";
               }));


                //Buy
                try
                {
                    if ((bool)Buy)
                    {
                        //if (CandleSeries[index].Close > CandleSeries[index].Open && percentChange <= 1)
                        if (percentChange <= -1)
                        {
                            //double pricePerCoinBuy = await currentPrice.CurrentMarketPrice(Symbol, OrderType.Buy);
                            var volume = quantity.TotalToQuantity(Total, topBuyPrice);

                            Dispatcher.CurrentDispatcher.Invoke(() =>
                            {
                                var buyOrder = Task.Run(async () => { await marketOrder.ExecuteMarketOrder(Symbol, OrderType.Buy, volume); });
                            //var buyOrder = marketOrder.ExecuteMarketOrder(Symbol, OrderType.Buy, volume);
                            buyOrder.Wait();
                            });

                            // Output IBS to datawindow

                            string tradeOutputBuy = DateTime.Now + "\n" + "Volume = " + volume + "\n" + OrderType.Buy + "\n" + "% Change = " + percentChange + "\n" + "-------------------" + "\n";
                            Debug.WriteLine(tradeOutputBuy);
                            outputData.Strategy1Output.Text += tradeOutputBuy;
                        }
                    }


                    if ((bool)Sell)
                    {
                        if (percentChange >= 100)
                        {
                            var quoteBalance = balance.GetBalance(Symbol.QuoteCurrency);
                            var balanceQuote = quoteBalance.Result;

                            if (balanceQuote > minOrderSize)
                            {
                                if (balanceQuote > minOrderSize)
                                {
                                    //double pricePerCoinSell = await currentPrice.CurrentMarketPrice(Symbol, OrderType.Sell);
                                    var volume = quantity.TotalToQuantity(Total, topSellPrice);

                                    Dispatcher.CurrentDispatcher.Invoke(() =>
                                    {
                                        var sellOrder = Task.Run(async () => { await marketOrder.ExecuteMarketOrder(Symbol, OrderType.Sell, volume); });
                                    sellOrder.Wait();
                                    });

                                    // Output IBS to datawindow
                                    string tradeOutputSell = DateTime.Now + "\n" + "Volume = " + volume + "\n" + OrderType.Sell + "\n" + "% Change = " + percentChange + "\n" + "-------------------" + "\n";
                                    Debug.WriteLine(tradeOutputSell);
                                    outputData.Strategy1Output.Text += tradeOutputSell;
                                }
                            }
                        }
                    }
                }

                catch (Exception error)
                {
                    Console.WriteLine(error.Message);
                    //MessageDialogue.Show("Order Error" + error.Message);
                }
            }
        }
    }
}
