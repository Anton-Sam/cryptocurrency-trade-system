using StrategyTester.Enums;
using StrategyTester.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyTester.Models
{
    public class Order : ICloneable
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string ClientOrderId { get; set; }
        public OrderStatus Status { get; set; }
        public OrderType Type { get; set; }
        public OrderSide Side { get; set; }
        public decimal? Price { get; set; }
        public decimal Quantity { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{Id} {Symbol} {ClientOrderId} {Status} {Type} {Price} {Quantity} {CreateTime.ToString("dd/MM/yyyy HH:mm")} {UpdateTime.ToString("dd/MM/yyyy HH:mm")}";
        }
    }
}
