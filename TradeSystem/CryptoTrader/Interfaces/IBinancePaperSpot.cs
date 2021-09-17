using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Interfaces
{
    interface IBinancePaperSpot
    {
        IBinancePaperMarket Market { get; }
        IBinancePaperOrder Order { get; }
        IBinancePaperAccount Account { get; }
    }
}
