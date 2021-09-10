using Skender.Stock.Indicators;
using StrategyTester.Core;
using StrategyTester.DataProviders;
using StrategyTester.Enums;
using StrategyTester.Models.Test;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyTester.Models.Common
{
    public abstract class BaseStrategy
    {
        public IDataProvider DataProvider { get; internal set; }
        public abstract void OnOrderStatusChanged(Order order);
        public abstract void OnCandleClosed(Candle candle);

        //public TestingResult StartTest(string symbol, CandleInterval candleInterval, int historyRange, decimal startBalance)
        //{
        //    DataProvider = new PaperDataProvider();
        //    VirtualExchange.OnCandleClosed += OnCandleClosed;
        //    VirtualExchange.OnOrderStatusChanged += OnOrderStatusChanged;
        //    VirtualExchange.InitializeExchange(symbol, candleInterval, historyRange, startBalance);
        //    return VirtualExchange.StartTrading();
        //}

        //public async Task<TestingResult> StartTestAsync(string symbol, CandleInterval candleInterval, int historyRange, decimal startBalance)
        //{
        //    return await Task.Run(() => StartTest(symbol, candleInterval, historyRange, startBalance));
        //}
    }
}
