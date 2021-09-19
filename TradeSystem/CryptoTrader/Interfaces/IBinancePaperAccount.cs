using CryptoTrader.Models;
using CryptoTrader.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Interfacesаа
{
    interface IBinancePaperAccount
    {
        public IEnumerable<Balance> GetBalances { get; set; }
    }
}
