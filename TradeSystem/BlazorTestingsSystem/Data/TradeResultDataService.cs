using System.Collections.Generic;

namespace BlazorTestingsSystem.Data
{
    public class TradeResultDataService
    {
        public IEnumerable<Candle> Candles { get; set; }
        public IEnumerable<Balance> Balances { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
