using Microsoft.Extensions.Logging;
using StrategyTester.Enums;
using StrategyTester.Models;
using StrategyTester.Models.Interfaces;
using StrategyTester.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyTester.DataProviders
{
    public class PaperDataProvider : IDataProvider
    {
        public ILogger Logger { get; set; }

        public event Action<Order> OnOrderStatusChanged;
        public event Action<Candle> OnCandleClosed;

        protected ExchangeState State { get; set; }

        protected virtual void RecalculationBalances(OrderSide side, OrderStatus status) { }
        
        public IEnumerable<Order> CancelAllOpenOrders()
        {
            throw new NotImplementedException();
        }

        public Order CancelOrder(string orderId, string clientOrderId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Balance> GetAllBalances()
        {
            throw new NotImplementedException();
        }

        public Balance GetBaseAssetBalance()
        {
            return null;
        }

        public IEnumerable<Candle> GetLastCandles(int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetOpenOrders()
        {
            throw new NotImplementedException();
        }

        public Balance GetQuoteAssetBalance()
        {
            return State.QuoteAssetBalance;

        }

        public Order PlaceOrder(string clientOrderId, OrderSide side, OrderType orderType, decimal quantity, decimal? price = null)
        {
            throw new NotImplementedException();
        }
    }
}
