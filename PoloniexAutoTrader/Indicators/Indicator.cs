using Jojatekok.PoloniexAPI;
using Jojatekok.PoloniexAPI.MarketTools;
using PoloniexAutoTrader.Market;
using PoloniexAutoTrader.Strategies;
using PoloniexAutoTrader.Timers;
using PoloniexAutoTrader.Trading;
using PoloniexAutoTrader.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PoloniexAutoTrader.Indicators
{
    public static class Indicator
    {
        public static double[] GetBollingerBandsWithSimpleMovingAverage(this IList<IMarketChartData> value, int index, int period)
        {
            var closes = new List<double>(period);
            for (var i = index; i > Math.Max(index - period, -1); i--)
            {
                closes.Add(value[i].Close);
            }

            var simpleMovingAverage = closes.Average();
            var stDevMultiplied = Math.Sqrt(closes.Average(x => Math.Pow(x - simpleMovingAverage, 2))) * 2;

            return new[] {
                simpleMovingAverage,
                simpleMovingAverage + stDevMultiplied,
                simpleMovingAverage - stDevMultiplied
            };
        }


        public static double ABR(this IList<IMarketChartData> value, int index, int period)
        {
            double range_adr = 0;

            for (var i = index; i > index - period; i--)
            {
                range_adr += (value[i].High - value[i].Low);                
            }

            range_adr /= period;
            return range_adr;
        }
    }
}
