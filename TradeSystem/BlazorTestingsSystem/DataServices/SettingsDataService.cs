using StrategyTester.Enums;
using StrategyTester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTestingsSystem.DataServices
{
    public class SettingsDataService
    {
        public Dictionary<string,BaseStrategy> StrategiesDict { get; set; }
        public BaseStrategy Strategy { get; set; }
        public string Symbol { get; set; }
        public CandleInterval Interval { get; set; }
        public decimal StartBalance { get; set; }
        public int HistoryRange { get; set; }

        public string Test { get; set; }
    }
}
