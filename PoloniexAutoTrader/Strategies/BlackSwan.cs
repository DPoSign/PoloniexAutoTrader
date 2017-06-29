using Jojatekok.PoloniexAPI;
using Jojatekok.PoloniexAPI.MarketTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoloniexAutoTrader.Strategies
{
    class BlackSwan : Strategy
    {

        public BlackSwan(string strategyName, MarketPeriod marketSeries, CurrencyPair symbol, bool? buy, bool? sell, double volume) : base(strategyName, marketSeries, symbol, buy, sell, volume)
        {
        }
    }
}
