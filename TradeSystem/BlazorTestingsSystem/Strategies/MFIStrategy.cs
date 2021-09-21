using Skender.Stock.Indicators;
using StrategyTester.Enums;
using StrategyTester.Models;
using StrategyTester.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTestingsSystem.Strategies
{
    public class MFIStrategy : IStrategy
    {
        public IDataProvider DataProvider { get; set; }

        private decimal quantity;
        private decimal balance = 1000;
        public void OnCandleClosed(Candle candle)
        {
            if (DataProvider.GetOpenOrders().Any())
                return;
            var lastCandles = DataProvider.GetLastCandles(500);
            if (lastCandles is null)
                return;
            
            var ema200 = lastCandles.GetEma(200).Last();
            var mfis = lastCandles.GetMfi().TakeLast(2);

            if (candle.Close>ema200.Ema && mfis.First().Mfi>20 && mfis.Last().Mfi < 20)
            {
                DataProvider.PlaceOrder("entry_long", OrderSide.Buy, OrderType.Market, 1);
            }
            else if (candle.Close < ema200.Ema && mfis.First().Mfi < 80 && mfis.Last().Mfi > 80)
            {
                DataProvider.PlaceOrder("entry_short", OrderSide.Sell, OrderType.Market, 1);
            }

        }

        public void OnOrderStatusChanged(Order order)
        {
            //if (order.Status == OrderStatus.Filled)
            //{
            //    if (order.ClientOrderId.Equals("entry_long"))
            //    {
            //        _entryPrice = order.Price.Value;
            //        DataProvider.PlaceOrder("long_tp_low", OrderSide.Sell, OrderType.Limit, 0.5m, 1.002m * order.Price);
            //        DataProvider.PlaceOrder("long_tp_high", OrderSide.Sell, OrderType.Limit, 0.5m, 1.008m * order.Price);
            //        DataProvider.PlaceOrder("long_sl", OrderSide.Sell, OrderType.Limit, 1, 0.996m * order.Price);
            //    }
            //    else if (order.ClientOrderId.Equals("entry_short"))
            //    {
            //        _entryPrice = order.Price.Value;
            //        DataProvider.PlaceOrder("short_tp_low", OrderSide.Buy, OrderType.Limit, 0.5m, 0.998m * order.Price);
            //        DataProvider.PlaceOrder("short_tp_high", OrderSide.Buy, OrderType.Limit, 0.5m, 0.992m * order.Price);
            //        DataProvider.PlaceOrder("short_sl", OrderSide.Buy, OrderType.Limit, 1, 1.004m * order.Price);
            //    }
            //    else if (order.ClientOrderId.Equals("long_tp_low"))
            //    {
            //        DataProvider.CancelOrder(clientOrderId: "long_sl");
            //        DataProvider.PlaceOrder("long_sl", OrderSide.Sell, OrderType.Limit, 0.5m,_entryPrice);
            //    }
            //    else if (order.ClientOrderId.Equals("short_tp_low"))
            //    {
            //        DataProvider.CancelOrder(clientOrderId: "short_sl");
            //        DataProvider.PlaceOrder("short_sl", OrderSide.Buy, OrderType.Limit, 0.5m, _entryPrice);
            //    }
            //    else
            //        DataProvider.CancelAllOpenOrders();
            //}

            if (order.Status==OrderStatus.Filled)
            {
                if (order.ClientOrderId.Equals("entry_long"))
                {
                    DataProvider.PlaceOrder("long_tp", OrderSide.Sell, OrderType.Limit, 1, 1.005m * order.Price);
                    DataProvider.PlaceOrder("long_sl", OrderSide.Sell, OrderType.Limit, 1, 0.996m * order.Price);
                }
                else if (order.ClientOrderId.Equals("entry_short"))
                {
                    DataProvider.PlaceOrder("short_tp", OrderSide.Buy, OrderType.Limit, 1, 0.995m * order.Price);
                    DataProvider.PlaceOrder("short_sl", OrderSide.Buy, OrderType.Limit, 1, 1.004m * order.Price);
                }
                else
                    DataProvider.CancelAllOpenOrders();
            }
        }
    }
}
