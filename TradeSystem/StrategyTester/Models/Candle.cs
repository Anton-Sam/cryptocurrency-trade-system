using Skender.Stock.Indicators;
using StrategyTester.Interfaces;
using System;

namespace StrategyTester.Models.Test
{
    public class Candle : IQuote
    {
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Volume { get; set; }
    }
}
