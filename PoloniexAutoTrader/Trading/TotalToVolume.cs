namespace PoloniexAutoTrader.Trading
{
    public class TotalToVolume
    {
        public double TotalToQuantity(double total, double pricePerCoin)
        {
            double quantity = 0;

            quantity = total / pricePerCoin;

            return quantity;
        }
    }
}
