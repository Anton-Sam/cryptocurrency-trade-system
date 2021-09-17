using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Interfaces
{
    interface IBinancePaperMarket
    {
        public void GetLastCandles(int count);
    }
}
