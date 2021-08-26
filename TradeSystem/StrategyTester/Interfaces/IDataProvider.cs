using Skender.Stock.Indicators;
using StrategyTester.Enums;
using StrategyTester.Models;
using StrategyTester.Models.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyTester.Interfaces
{
    public interface IDataProvider
    {
        Order PlaceOrder(string clientOrderId, OrderSide side, OrderType orderType, decimal quantity, decimal? price = null);
        Order CancelOrder(string orderId,string clientOrderId);
        IEnumerable<Order> CancelAllOpenOrders();
        IEnumerable<Order> GetOpenOrders();
        Balance GetBaseAssetBalance();
        Balance GetQuoteAssetBalance();
        IEnumerable<Balance> GetAllBalances();
        IEnumerable<Candle> GetLastCandles(int count);
    }
}
