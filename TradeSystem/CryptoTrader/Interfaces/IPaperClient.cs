using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Interfaces
{
    interface IPaperClient
    {
        IBinancePaperSpot Spot { get; }
        IBinacePaperFuturesUsdt FuturesUsdt { get; }
    }
}
