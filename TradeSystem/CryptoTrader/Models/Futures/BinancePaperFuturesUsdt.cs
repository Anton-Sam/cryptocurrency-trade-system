using CryptoTrader.Interfaces;
using CryptoTrader.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Models.Futures
{
    class BinancePaperFuturesUsdt : IBinacePaperFuturesUsdt
    {
        public IBinancePaperMarket Market { get; }

        public IBinancePaperOrder Order { get; }

        public BinancePaperFuturesUsdt()
        {
            Market = new BinancePaperMarket();
            Order = new BinancePaperOrder();
        }
    }
}
