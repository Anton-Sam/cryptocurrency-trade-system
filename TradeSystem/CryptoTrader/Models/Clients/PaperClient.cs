using CryptoTrader.Core;
using CryptoTrader.Interfaces;
using CryptoTrader.Models.Futures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Models.Clients
{
    class PaperClient : IPaperClient
    {
        public IBinancePaperSpot Spot { get; }
        public IBinacePaperFuturesUsdt FuturesUsdt { get; }
        internal VirtualExchange VirtualExchange { get; }
        
        public PaperClient()
        {
            FuturesUsdt = new BinancePaperFuturesUsdt();

        }
    }
}
