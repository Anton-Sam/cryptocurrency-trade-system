using Binance.Net.Enums;
using StrategyTester.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyTester.Extensions
{
    static class CandleIntervalToBinanceKlineInterval
    {
        public static KlineInterval ToBinanceKlineInterval(this CandleInterval value)
        {
            if (Enum.TryParse(value.ToString(), out KlineInterval outValue))
                return outValue;
            throw new InvalidCastException($"Can't parse to {typeof(KlineInterval)}");
        }
    }
}
