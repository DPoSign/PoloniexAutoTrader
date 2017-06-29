using Jojatekok.PoloniexAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoloniexAutoTrader.Symbols
{
    public class SymbolCode
    {

        #region Currency Pair Method
        // Get Currency pair from symbol strings
        public CurrencyPair GetSymbolCode(string symbol1, string symbol2)
        {
            CurrencyPair symbol = new CurrencyPair(symbol1, symbol2);
            return symbol;
        }

        /*
        // Split Currency pair to symbol1
        public static string GetSymbol1(CurrencyPair symbol)
        {
            string symbol1 = symbol.ToString().Substring(0, 3);
            return symbol1;
        }

        // Split Currency pair to symbol2
        public static string GetSymbol2(CurrencyPair symbol)
        {
            string symbol2 = symbol.ToString().Substring(4, 3);
            return symbol2;
        }
        */
        #endregion
    }
}
