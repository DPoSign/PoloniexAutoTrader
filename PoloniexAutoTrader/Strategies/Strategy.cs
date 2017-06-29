using Jojatekok.PoloniexAPI;
using Jojatekok.PoloniexAPI.LiveTools;
using Jojatekok.PoloniexAPI.MarketTools;
using PoloniexAutoTrader.Market;
using PoloniexAutoTrader.Timers;
using PoloniexAutoTrader.Trading;
using PoloniexAutoTrader.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PoloniexAutoTrader
{
    public abstract class Strategy
    {
        #region Fields
        #region Internal
        SmartDispatcherTimer dispatcherTimer;
        #endregion

        #region External
        public string StrategyName { get; set; }
        public MarketPeriod MarketSeries { get; set; }
        public CurrencyPair Symbol { get; set; }
        public bool? Buy { get; set; }
        public bool? Sell { get; set; }
        public double Total { get; set; }
        public double minOrderSize = 0.001;
        public Client Client;
        public GetBalances balance;
        public MarketOrder marketOrder;
        public TotalToVolume quantity;
        public GetCurrentPrice currentPrice;
        public Strategy1Data outputData;
        #endregion
        #endregion

        #region Constructor
        public Strategy(string strategyName, MarketPeriod marketSeries, CurrencyPair symbol, bool? buy, bool? sell, double total)
        {
            StrategyName = strategyName;
            MarketSeries = marketSeries;
            Symbol = symbol;
            Buy = buy;
            Sell = sell;
            Total = total;

            // Timer
            dispatcherTimer = new SmartDispatcherTimer();
            dispatcherTimer.IsReentrant = false;
            //Test interval
            dispatcherTimer.Interval = TimeSpan.FromSeconds(10);
            //dispatcherTimer.Interval = TimeSpan.FromSeconds((int)marketSeries);
            
        }
        #endregion

        #region Public methods

        public async Task Start()
        {
            Client = new Client();
            balance = new GetBalances();
            marketOrder = new MarketOrder();
            quantity = new TotalToVolume();
            currentPrice = new GetCurrentPrice();
            // Output window
            outputData = new Strategy1Data();

            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                Client.PoloniexClient.Live.Start();
            });           

            // Override this method on sub classes
            dispatcherTimer.TickTask = async () =>
            {
                await OnBar();
            };
            dispatcherTimer.Start();

            
            await GetTickerData();

            // Start method
            OnStart();
        }

        public async Task GetTickerData()
        {
            await Client.PoloniexClient.Live.SubscribeToTickerAsync();
            Client.PoloniexClient.Live.OnTickerChanged += TickChanged;
        }

        public void TickChanged(object sender, TickerChangedEventArgs ticker)
        {
            OnTick(ticker);           
        }

        public void Stop()
        {
            // you can call this method from your stratigies instances
            dispatcherTimer.Stop();
           // Client.PoloniexClient.Live.Stop();

            OnStop();
        }
        #endregion

        #region Methods for overridding on sub classes


        #endregion

        #region Methods for overridding on sub classes
        public virtual void OnStart()
        {
            // Override this method on sub classes

        }

        #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task OnBar()
        {
            // Override this method on sub classes
        }

        #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual void OnTick(TickerChangedEventArgs ticker)
        {
            // Override this method on sub classes
            // Find a way to call this method after each tick change on price(bid or ask)
        }

        public virtual void OnStop()
        {
            // Override this method on sub classes
        }
        #endregion
    }
}