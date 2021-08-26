using StrategyTester.Core;
using StrategyTester.Enums;
using StrategyTester.Interfaces;
using StrategyTester.Models;
using StrategyTester.Models.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyTester.DataProviders
{
    class PaperDataProvider : IDataProvider
    {
        public Balance GetBaseAssetBalance()
        {
            return VirtualExchange.GetBaseAssetBalance();
        }

        public Balance GetQuoteAssetBalance()
        {
            return VirtualExchange.GetQuoteAssetBalance();
        }
        public IEnumerable<Balance> GetAllBalances()
        {
            return VirtualExchange.GetAllBalances();
        }

        public Order PlaceOrder(string clientOrderId, OrderSide side, OrderType type, decimal quantity, decimal? price = null)
        {
            return VirtualExchange.PlaceOrder(clientOrderId, side, type, quantity, price);
        }

        public Order CancelOrder(string orderId=null,string clientOrderId=null)
        {
            return VirtualExchange.CancelOrder(orderId);
        }

        public IEnumerable<Order> CancelAllOpenOrders()
        {
            return VirtualExchange.CancelAllOpenOrders();
        }

        public IEnumerable<Order> GetOpenOrders()
        {
            return VirtualExchange.GetOpenOrders();
        }

        public IEnumerable<Candle> GetLastCandles(int count)
        {
            return VirtualExchange.GetLastCandles(count);
        }
    }
}
