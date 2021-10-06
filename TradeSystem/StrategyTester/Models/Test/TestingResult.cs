using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyTester.Models.Test
{
    public class TestingResult
    {
        public List<Candle> HistoryData { get; set; } = new List<Candle>();
        public List<Order> OrdersHistory { get; set; } = new List<Order>();
        public List<BalanceChange> BalanceChanges { get; set; } = new List<BalanceChange>();
    }
}
