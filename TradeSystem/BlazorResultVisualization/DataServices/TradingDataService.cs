using StrategyTester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorResultVisualization.DataServices
{
    public class TradingDataService
    {
        public IEnumerable<Candle> Candles { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Balance> Balances { get; set; }
    }
}