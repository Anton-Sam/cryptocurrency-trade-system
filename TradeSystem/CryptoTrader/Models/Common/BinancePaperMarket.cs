using CryptoTrader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Models.Common
{
    class BinancePaperMarket : IBinancePaperMarket
    {
        public IEnumerable<Candle> GetLastCandles(int count)
        {
            return null;
        }
    }
}
