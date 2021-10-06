using Skender.Stock.Indicators;
using StrategyTester.Enums;
using StrategyTester.Models;
using StrategyTester.Models.Interfaces;
using System.Linq;

namespace BlazorTestingsSystem.Strategies
{
    public class TwoEmaStrategy : IStrategy
    {
        public IDataProvider DataProvider { get; set; }

        public void OnCandleClosed(Candle candle)
        {
            var lastCandles = DataProvider.GetLastCandles(265);
            if (lastCandles is null)
                return;
            var ema9 = lastCandles.GetEma(9).TakeLast(2);
            var ema15 = lastCandles.GetEma(15).TakeLast(2);

            var openOrders = DataProvider.GetOpenOrders();
            if (openOrders.Any())
            {
                var openOrder = openOrders.First();
                if (openOrder.Side == OrderSide.Sell && ema9.First().Ema > ema15.First().Ema && ema9.Last().Ema < ema15.Last().Ema)
                {
                    DataProvider.CancelAllOpenOrders();
                    DataProvider.PlaceOrder("sl", OrderSide.Sell, OrderType.Market, openOrder.Quantity);
                }
            }

            if (ema9.First().Ema < ema15.First().Ema && ema9.Last().Ema > ema15.Last().Ema)
            {
                DataProvider.PlaceOrder("entry", OrderSide.Buy, OrderType.Market, 1);
            }
        }

        public void OnOrderStatusChanged(Order order)
        {
            if (order.Status == OrderStatus.Filled && order.ClientOrderId.Equals("entry"))
            {
                DataProvider.PlaceOrder("tp", OrderSide.Sell, OrderType.Limit, order.Quantity, 1.004m * order.Price);
            }
        }
    }
}
