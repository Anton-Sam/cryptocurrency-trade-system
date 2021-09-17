using Binance.Net.Interfaces;
using CryptoTrader.Interfaces;
using System;

namespace CryptoTrader
{
    public class Trader
    {
        IBinanceClient BinanceClient { get; }
        IPaperClient PaperClient { get; }
    }
}
