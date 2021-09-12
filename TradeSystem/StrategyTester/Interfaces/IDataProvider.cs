using Microsoft.Extensions.Logging;
using StrategyTester.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyTester.Models.Interfaces
{
    public interface IDataProvider
    {
        ILogger Logger { get; set; }
        Order PlaceOrder(string clientOrderId, OrderSide side, OrderType orderType, decimal quantity, decimal? price = null);
        Order CancelOrder(string orderId, string clientOrderId);
        IEnumerable<Order> CancelAllOpenOrders();
        IEnumerable<Order> GetOpenOrders();
        Balance GetBaseAssetBalance();
        Balance GetQuoteAssetBalance();
        IEnumerable<Balance> GetAllBalances();
        IEnumerable<Candle> GetLastCandles(int count);

        event Action<Order> OnOrderStatusChanged;
        event Action<Candle> OnCandleClosed;
    }
}
