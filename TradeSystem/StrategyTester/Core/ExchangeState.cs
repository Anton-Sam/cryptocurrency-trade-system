﻿using Binance.Net;
using StrategyTester.Models.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrategyTester.Extensions;

namespace StrategyTester.Core
{
    class ExchangeState
    {
        internal int OrderId { get; set; }
        internal TestingSettings TestingSettings { get; set; }
        internal List<Order> OpenOrders { get; set; } = new List<Order>();
        internal List<Order> OrdersHistory { get; set; } = new List<Order>();
        internal List<Candle> HistoryCandles { get; set; } = new List<Candle>();
        internal Balance BaseAssetBalance { get; set; } = new Balance();
        internal Balance QuoteAssetBalance { get; set; } = new Balance();
        internal SymbolInfo SymbolInfo { get; set; } = new SymbolInfo();
        internal Candle LastCandle { get; set; }
        //internal TradeSettings TradeSettings { get; set; }
    }
}
