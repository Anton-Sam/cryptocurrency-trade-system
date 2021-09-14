using Skender.Stock.Indicators;
using StrategyTester.Enums;
using StrategyTester.Models;
using StrategyTester.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTestingsSystem.Strategies
{
    public class MacdEmaStrategy : IStrategy
    {
        public IDataProvider DataProvider { get; set; }

        private decimal tp = 0.01m;
        private decimal sl = 0.01m;

        public void OnCandleClosed(Candle candle)
        {
            var lastCandles = DataProvider.GetLastCandles(400);
            if (lastCandles is null)
                return;
            var ema100 = lastCandles.GetEma(200).TakeLast(1);
            var macd = lastCandles.GetMacd().TakeLast(2);

            var openOrders = DataProvider.GetOpenOrders();
            if (!openOrders.Any())
            {
                if (candle.Close > ema100.First().Ema && macd.First().Histogram > 0 && macd.Last().Histogram < 0)
                    DataProvider.PlaceOrder("buy_entry", OrderSide.Buy, OrderType.Market, 1);
                else if (candle.Close < ema100.First().Ema && macd.First().Histogram < 0 && macd.Last().Histogram > 0)
                    DataProvider.PlaceOrder("sell_entry", OrderSide.Buy, OrderType.Market, 1);
            }
        }

        public void OnOrderStatusChanged(Order order)
        {
            if (order.Status == OrderStatus.Filled)
            {
                switch (order.ClientOrderId)
                {
                    case "buy_entry":
                        DataProvider.PlaceOrder("buy_tp", OrderSide.Sell, OrderType.Limit, 1, order.Price * (1 + tp));
                        DataProvider.PlaceOrder("buy_sl", OrderSide.Sell, OrderType.Limit, 1, order.Price * (1 - sl));
                        break;
                    case "sell_entry":
                        DataProvider.PlaceOrder("sell_tp", OrderSide.Buy, OrderType.Limit, 1, order.Price * (1 - tp));
                        DataProvider.PlaceOrder("sell_sl", OrderSide.Buy, OrderType.Limit, 1, order.Price * (1 + sl));
                        break;
                    default:
                        DataProvider.CancelAllOpenOrders();
                        return;
                }
            }
        }
    }
}
