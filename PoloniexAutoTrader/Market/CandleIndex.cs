using System;
using System.Linq;
using System.Threading.Tasks;
using Jojatekok.PoloniexAPI.MarketTools;
using Jojatekok.PoloniexAPI;

namespace PoloniexAutoTrader.Market
{
    public class CandleIndex
    {
        Client Client = new Client();

        public async Task<int> GetHistorialData(CurrencyPair symbol, MarketPeriod marketseries)
        {
            DateTime startdate = new DateTime(2017, 1, 1);
            DateTime enddate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0);

            var canldeinfo = await Client.PoloniexClient.Markets.GetChartDataAsync(symbol, marketseries, startdate, enddate);
            var candleindex = canldeinfo.Count() - 1;
            return candleindex;           
        }
    }
}
