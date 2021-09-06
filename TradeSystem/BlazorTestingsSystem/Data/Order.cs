using BlazorTestingsSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTestingsSystem.Data
{
    public class Order
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string ClientOrderId { get; set; }
        public OrderStatus Status { get; set; }
        public OrderType Type { get; set; }
        public OrderSide Side { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
