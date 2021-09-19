using CryptoTrader.Enums;
using CryptoTrader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Models.Common
{
    class BinancePaperOrder : IBinancePaperOrder
    {
        public IEnumerable<Order> CancelAllOpenOrders()
        {
            throw new NotImplementedException();
        }

        public Order CancelOrder(string orderId, string clientOrderId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetOpenOrders()
        {
            throw new NotImplementedException();
        }

        public Order PlaceOrder(string clientOrderId, OrderSide side, OrderType type, decimal quantity, decimal? price = null)
        {
            throw new NotImplementedException();
        }
    }
}
