using Skender.Stock.Indicators;
using StrategyTester.Enums;
using StrategyTester.Models;
using StrategyTester.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BlazorTestingsSystem.Strategies
{
    public class HullMaStrategy : IStrategy
    {
        public IDataProvider DataProvider { get; set; }

        private IEnumerable<Candle> lastCandles;
        private decimal entryPrice;
        private decimal quantity;
        private decimal balance = 1000;
        private decimal initBalance = 1000;
        public void OnCandleClosed(Candle candle)
        {
            if (DataProvider.GetOpenOrders().Any())
                return;
            lastCandles = DataProvider.GetLastCandles(500);
            if (lastCandles is null)
                return;

            var ema200 = lastCandles.GetEma(200).Last();
            var hullMa = lastCandles.GetHma(16).TakeLast(2);
            quantity = balance / candle.Close;

            if (candle.Close > ema200.Ema && hullMa.Last().Hma > hullMa.First().Hma)
            {
                DataProvider.PlaceOrder("entry_long", OrderSide.Buy, OrderType.Market, quantity);
            }
            else if (candle.Close < ema200.Ema && hullMa.Last().Hma < hullMa.Last().Hma)
            {
                DataProvider.PlaceOrder("entry_short", OrderSide.Sell, OrderType.Market, quantity);
            }

        }

        public void OnOrderStatusChanged(Order order)
        {
  
            if (order.Status == OrderStatus.Filled)
            {
                if (order.ClientOrderId.Equals("entry_long"))
                {
                    DataProvider.PlaceOrder("long_tp", OrderSide.Sell, OrderType.Limit, quantity, 1.01m * order.Price);
                    DataProvider.PlaceOrder("long_sl", OrderSide.Sell, OrderType.Limit, quantity, 0.99m * order.Price);
                }
                else if (order.ClientOrderId.Equals("entry_short"))
                {
                    DataProvider.PlaceOrder("short_tp", OrderSide.Buy, OrderType.Limit, quantity, 0.99m * order.Price);
                    DataProvider.PlaceOrder("short_sl", OrderSide.Buy, OrderType.Limit, quantity, 1.01m * order.Price);
                }
                else if (order.ClientOrderId.Contains("sl"))
                {
                    balance *= 2;
                    DataProvider.CancelAllOpenOrders();
                }
                else if (order.ClientOrderId.Contains("tp"))
                {
                    balance = initBalance;
                    DataProvider.CancelAllOpenOrders();
                }
            }
        }
    }
}
