using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Jojatekok.PoloniexAPI;
using Jojatekok.PoloniexAPI.MarketTools;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Threading;
using PoloniexAutoTrader.Strategies;
using MahApps.Metro.Controls;
using PoloniexAutoTrader.Wallet;
using System.Diagnostics;
using System.Threading;
using PoloniexAutoTrader.Timers;
using System.Collections.ObjectModel;

namespace PoloniexAutoTrader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        
        private Client Client { get; set; }
        ObservableCollection<Strategy> runningStrategyList = new ObservableCollection<Strategy>();

        public MainWindow()
        {
            InitializeComponent();

            // Load Client from app properties
            Client = new Client();

            //Display Current Time
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();            
        }

        async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await GetBalanceAsync();
        }

        //***************************************************************************************

        #region API Keys
        //Set API Keys
        private void APIKeySetButton_Click(object sender, RoutedEventArgs e)
        {
            var ApiKey_Window = new ApiKey_Window();
            ApiKey_Window.Show();
        }
        #endregion

        //***************************************************************************************

        #region Currency Pair Method
        // Get Currency pair from symbol strings
        public CurrencyPair GetSymbolCode(string symbol1, string symbol2)
        {
            CurrencyPair symbol = new CurrencyPair(symbol1, symbol2);
            return symbol;
        }
        #endregion

        //***************************************************************************************

        void timer_Tick(object sender, EventArgs e)
        {
            ShowTime.Text = DateTime.Now.ToShortTimeString();
        }

        //***************************************************************************************

        public async Task GetBalanceAsync()
        {
            GetBalances balance = new GetBalances();

            var btcAvailable = await balance.GetBalance("BTC");
            var btcTotalAll = await balance.GetBalance();

            ShowBalanceStatus.Text = btcAvailable.ToStringNormalized() + " BTC";

            TotalBalanceAll.Text = btcTotalAll.ToString();
        }

        //***************************************************************************************


        //MarketSeries ComboBox
        private void MarketSeriesSelect_Loaded(object sender, RoutedEventArgs e)
        {
            // ... A List.
            List<MarketPeriod> marketseriesList = new List<MarketPeriod>
            {
                MarketPeriod.Minutes5,
                MarketPeriod.Minutes15,
                MarketPeriod.Minutes30,
                MarketPeriod.Hours2,
                MarketPeriod.Hours4,
                MarketPeriod.Day
            };
            var comboBox = sender as ComboBox;

            comboBox.Items.Clear();

            comboBox.ItemsSource = marketseriesList;

            comboBox.SelectedIndex = 5;

        }

        //MarketSeries ComboBox Selection
        private void MarketSeriesSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;
            // ... Set SelectedItem MarketSeries
            MarketPeriod marketseries = (MarketPeriod)(comboBox.SelectedItem);
        }

        //***************************************************************************************


        //Zero balance checkbox - Checkced
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Handle(sender as CheckBox);
        }

        //Zero balance checkbox - Uncheckced
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Handle(sender as CheckBox);
        }

        //
        void Handle(CheckBox checkBox)
        {
            // Refresh balance
            //BalanceGrid.Items.Refresh();
        }

        //***************************************************************************************

        #region Setting Symbols

        
        private async void SymbolSelect_Loaded(object sender, RoutedEventArgs e)
        {
            IDictionary<CurrencyPair, IMarketData> markets = await GetMarketInfo();
            List<CurrencyPair> symbolList = new List<CurrencyPair>();

        foreach (var symbols in (markets.OrderBy(i => i.Key.QuoteCurrency)))
        {
            symbolList.Add(symbols.Key);
        }

            var comboBox = sender as ComboBox;

            comboBox.Items.Clear();

            comboBox.ItemsSource = symbolList;

            comboBox.SelectedIndex = 0;
        }

        #endregion

        //***************************************************************************************

        #region Submit Orders
        // Method to get market data
        private async Task<IDictionary<CurrencyPair, IMarketData>> GetMarketInfo()
        {
            return await Client.PoloniexClient.Markets.GetSummaryAsync();
        }

        //***************************************************************************************

        private async Task DialogBoxStrategy(string strategyName, string state)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "OK",
                ColorScheme = MetroDialogOptions.ColorScheme
            };

            MessageDialogResult result = await this.ShowMessageAsync(strategyName, strategyName + " " + state + " Successfully",
            MessageDialogStyle.Affirmative, mySettings);

            if (result == MessageDialogResult.Affirmative)
            {
                // Run Strategy 1
            }        

        }

        // Get data from XAML fields
        private void GetStrategyData(out string strategyName, out MarketPeriod marketSeries, out CurrencyPair symbol, out double total, out bool? buy, out bool? sell, Label strategyNameTextBox, ComboBox marketSeriesComboBox, ComboBox symbolComboBox, CheckBox buyCheck, CheckBox sellCheck, TextBox volumeTextBox)
        {
            strategyName = strategyNameTextBox.Content.ToString();
            marketSeries = (MarketPeriod)(marketSeriesComboBox.SelectedItem);
            symbol = (CurrencyPair)symbolComboBox.SelectedItem;
            total = double.Parse(volumeTextBox.Text);
            buy = buyCheck.IsChecked;
            sell = sellCheck.IsChecked;
        }

        // Start Strategy 1
        async void Strategy1Start_Click(object sender, RoutedEventArgs e)
        {

            // Get data from textboxes / comboboxes
            GetStrategyData(out string strategyName, out MarketPeriod marketSeries, out CurrencyPair symbol, out double total, out bool? buy, out bool? sell, StrategyName1, MarketSeriesSelect1, Strategy1Symbol, Strategy1Buy, Strategy1Sell, Strategy1Total);

            BlackSwan blackSwan = new BlackSwan(strategyName, marketSeries, symbol, buy, sell, total);

            // Show saved settings in textblock - TEMP
            TestConsoleStrat1.Text = blackSwan.StrategyName + "\n" + blackSwan.Symbol + " | " + blackSwan.Total + " | " + blackSwan.Buy + " | " + blackSwan.Sell + " | " + blackSwan.MarketSeries;

            // Create List
            // ObservableCollection<BlackSwan> list = new ObservableCollection<BlackSwan>();
            runningStrategyList.Add(blackSwan);
            this.RunningStrategies.ItemsSource = runningStrategyList;

            //Show dialogue box to show strategy is running
            await DialogBoxStrategy(StrategyName1.Content.ToString(), "Started");            
        }

        // Start Strategy 2
        async void Strategy2Start_Click(object sender, RoutedEventArgs e)
        {
            // Get data from textboxes / comboboxes
            GetStrategyData(out string strategyName, out MarketPeriod marketSeries, out CurrencyPair symbol, out double total, out bool? buy, out bool? sell, StrategyName2, MarketSeriesSelect2, Strategy2Symbol, Strategy2Buy, Strategy2Sell, Strategy2Total);

            // Save strategy settings to strategy class
            IBS ibs = new IBS(strategyName, marketSeries, symbol, buy, sell, total);

            // Create List
            // ObservableCollection<IBS> list = new ObservableCollection<IBS>();
            runningStrategyList.Add(ibs);
            this.RunningStrategies.ItemsSource = runningStrategyList;

            // Show saved settings in textblock - TEMP
            TestConsoleStrat2.Text = ibs.StrategyName + "\n" + ibs.Symbol + " | " + ibs.Total + " | " + ibs.Buy + " | " + ibs.Sell + " | " + ibs.MarketSeries;

            //Show dialogue box to show strategy is running
            await DialogBoxStrategy(StrategyName2.Content.ToString(), "Started");

            // Start Algo
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ibs.Start();


        }


        // Start Strategy 3
        async void Strategy3Start_Click(object sender, RoutedEventArgs e)
        {
            // Get data from textboxes / comboboxes
            GetStrategyData(out string strategyName, out MarketPeriod marketSeries, out CurrencyPair symbol, out double total, out bool? buy, out bool? sell, StrategyName3, MarketSeriesSelect3, Strategy3Symbol, Strategy3Buy, Strategy3Sell, Strategy3Total);

            // Save strategy settings to strategy class
            PercentageTrader percentageTrader = new PercentageTrader(strategyName, marketSeries, symbol, buy, sell, total);

            // Create List
            // ObservableCollection<PercentageTrader> list = new ObservableCollection<PercentageTrader>();
            runningStrategyList.Add(percentageTrader);
            this.RunningStrategies.ItemsSource = runningStrategyList;

            // Show saved settings in textblock - TEMP
            TestConsoleStrat3.Text = percentageTrader.StrategyName + "\n" + percentageTrader.Symbol + " | " + percentageTrader.Total + " | " + percentageTrader.Buy + " | " + percentageTrader.Sell + " | " + percentageTrader.MarketSeries;

            //Show dialogue box to show strategy is running
            await DialogBoxStrategy(StrategyName3.Content.ToString(), "Started");

            // Start Algo
            percentageTrader.Start();
        }

        // Start Strategy 4
        async void Strategy4Start_Click(object sender, RoutedEventArgs e)
        {
            // Get data from textboxes / comboboxes
            GetStrategyData(out string strategyName, out MarketPeriod marketSeries, out CurrencyPair symbol, out double total, out bool? buy, out bool? sell, StrategyName4, MarketSeriesSelect4, Strategy4Symbol, Strategy4Buy, Strategy4Sell, Strategy4Total);

            // Save strategy settings to strategy class
            Range rangeTrader = new Range(strategyName, marketSeries, symbol, buy, sell, total);

            // Create List
            runningStrategyList.Add(rangeTrader);
            this.RunningStrategies.ItemsSource = runningStrategyList;

            // Show saved settings in textblock - TEMP
            TestConsoleStrat4.Text = rangeTrader.StrategyName + "\n" + rangeTrader.Symbol + " | " + rangeTrader.Total + " | " + rangeTrader.Buy + " | " + rangeTrader.Sell + " | " + rangeTrader.MarketSeries;

            //Show dialogue box to show strategy is running
            await DialogBoxStrategy(StrategyName4.Content.ToString(), "Started");

            // Start Algo
            rangeTrader.Start();
        }

        // Start Strategy 4
        async void Strategy5Start_Click(object sender, RoutedEventArgs e)
        {
            // Get data from textboxes / comboboxes
            GetStrategyData(out string strategyName, out MarketPeriod marketSeries, out CurrencyPair symbol, out double total, out bool? buy, out bool? sell, StrategyName5, MarketSeriesSelect5, Strategy5Symbol, Strategy5Buy, Strategy5Sell, Strategy5Total);

            // Save strategy settings to strategy class
            RangeBreakout rangeBreakoutTrader = new RangeBreakout(strategyName, marketSeries, symbol, buy, sell, total);

            // Create List
            runningStrategyList.Add(rangeBreakoutTrader);
            this.RunningStrategies.ItemsSource = runningStrategyList;

            // Show saved settings in textblock - TEMP
            TestConsoleStrat4.Text = rangeBreakoutTrader.StrategyName + "\n" + rangeBreakoutTrader.Symbol + " | " + rangeBreakoutTrader.Total + " | " + rangeBreakoutTrader.Buy + " | " + rangeBreakoutTrader.Sell + " | " + rangeBreakoutTrader.MarketSeries;

            //Show dialogue box to show strategy is running
            await DialogBoxStrategy(StrategyName5.Content.ToString(), "Started");

            // Start Algo
            rangeBreakoutTrader.Start();
        }

        // Stop Strategy 1
        async void StrategyStop_Click(object sender, RoutedEventArgs e)
        {
            Strategy strategy = (Strategy)RunningStrategies.SelectedItem;
            runningStrategyList.Remove(strategy);

            strategy.outputData.Close();

            strategy.Stop();
            
            // Show stopping dialog
            await DialogBoxStrategy(StrategyName4.Content.ToString(), "Stopped");
        }

        private void Total_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RunningStrategies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = RunningStrategies.SelectedItem;
        }
        #endregion

        //***************************************************************************************


    }
}
