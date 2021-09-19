using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Interfaces
{
    interface IBinacePaperFuturesUsdt
    {
        IBinancePaperMarket Market { get; }
        IBinancePaperOrder Order { get; }
        //IBinancePaperAccount Account { get; }
    }
}
