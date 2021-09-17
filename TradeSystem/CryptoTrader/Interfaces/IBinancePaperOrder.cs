using CryptoTrader.Enums;
using CryptoTrader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Interfaces
{
    interface IBinancePaperOrder
    {
        Order PlaceOrder(string clientOrderId, OrderSide side, OrderType type, decimal quantity, decimal? price = null);
        Order CancelOrder(string orderId, string clientOrderId);
        IEnumerable<Order> CancelAllOpenOrders();
        IEnumerable<Order> GetOpenOrders();
    }
}
