using Skender.Stock.Indicators;
using StrategyTester.Core;
using StrategyTester.DataProviders;
using StrategyTester.Enums;
using StrategyTester.Interfaces;
using StrategyTester.Models.Test;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyTester.Models
{
    public abstract class BaseStrategy
    {
        public IDataProvider DataProvider { get; private set; }
        public abstract string Symbol { get; set; }
        public abstract CandleInterval CandleInterval { get; set; }
        public abstract void OnOrderStatusChanged(Order order);
        public abstract void OnCandleClosed(Candle candle);

        public TestingResult StartTest(string symbol, CandleInterval candleInterval, int historyRange, decimal startBalance)
        {
            DataProvider = new PaperDataProvider();
            VirtualExchange.OnCandleClosed += OnCandleClosed;
            VirtualExchange.OnOrderStatusChanged += OnOrderStatusChanged;
            VirtualExchange.InitializeExchange(symbol, candleInterval, historyRange, startBalance);
            return VirtualExchange.StartTrading();

            //For Debug
            
        }
    }
}
