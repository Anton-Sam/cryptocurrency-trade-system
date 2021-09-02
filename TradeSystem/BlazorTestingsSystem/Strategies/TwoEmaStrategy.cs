using Skender.Stock.Indicators;
using StrategyTester.Enums;
using StrategyTester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTestingsSystem.Strategies
{
    public class TwoEmaStrategy : BaseStrategy
    {
        public override void OnCandleClosed(Candle candle)
        {
            var lastCandles = DataProvider.GetLastCandles(20);
            if (lastCandles is null)
                return;
            var sma9 = lastCandles.GetSma(9).TakeLast(2);
            var sma15 = lastCandles.GetSma(15).TakeLast(2);

            var openOrders = DataProvider.GetOpenOrders();
            if (openOrders.Any())
            {
                var openOrder = openOrders.First();
                if (openOrder.Side == OrderSide.Sell && sma9.First().Sma > sma15.First().Sma && sma9.Last().Sma < sma15.Last().Sma)
                {
                    DataProvider.CancelAllOpenOrders();
                    DataProvider.PlaceOrder("sl", OrderSide.Sell, OrderType.Market, openOrder.Quantity);
                }
            }


            if (sma9.First().Sma < sma15.First().Sma && sma9.Last().Sma > sma15.Last().Sma)
            {
                DataProvider.PlaceOrder("entry", OrderSide.Buy, OrderType.Market, 1);
            }

        }

        public override void OnOrderStatusChanged(Order order)
        {
            if (order.Status == OrderStatus.Filled && order.ClientOrderId.Equals("entry"))
            {
                DataProvider.PlaceOrder("tp", OrderSide.Sell, OrderType.Limit, order.Quantity, 1.004m * order.Price);
            }
        }
    }
}
