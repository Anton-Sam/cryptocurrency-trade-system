using StrategyTester.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyTester.Models.Common
{
    public interface IDataProvider
    {
        public abstract Order PlaceOrder(string clientOrderId, OrderSide side, OrderType orderType, decimal quantity, decimal? price = null);
        public abstract Order CancelOrder(string orderId, string clientOrderId);
        public abstract IEnumerable<Order> CancelAllOpenOrders();
        public abstract IEnumerable<Order> GetOpenOrders();
        public abstract Balance GetBaseAssetBalance();
        public abstract Balance GetQuoteAssetBalance();
        public abstract IEnumerable<Balance> GetAllBalances();
        public abstract IEnumerable<Candle> GetLastCandles(int count);

        public event Action<Order> OnOrderStatusChanged;
        public event Action<Candle> OnCandleClosed;
    }
}
