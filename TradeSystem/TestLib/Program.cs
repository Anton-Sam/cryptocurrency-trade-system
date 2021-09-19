using Binance.Net;
using System;

namespace TestLib
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new BinanceClient();
            var data=client.FuturesUsdt.Market.GetKlinesAsync("BTCUSDT", Binance.Net.Enums.KlineInterval.OneMinute, limit: 3).Result.Data;
        }
    }
}
