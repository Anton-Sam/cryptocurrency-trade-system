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
    public class MacdEmaAtrFractstrategy : IStrategy
    {
        public IDataProvider DataProvider { get; set; }

        IEnumerable<Candle> lastCandles;
        public void OnCandleClosed(Candle candle)
        {
            if (DataProvider.GetOpenOrders().Any())
                return;
            lastCandles = DataProvider.GetLastCandles(500);
            if (lastCandles is null)
                return;

            var ema200 = lastCandles.GetEma(200).Last();
            var macd = lastCandles.GetMacd().TakeLast(2);
            var atr = lastCandles.GetAtr().Last();

            if (candle.Close > ema200.Ema && macd.First().Histogram < 0 && macd.Last().Histogram > 0 && atr.Atr < 500)
            {
                DataProvider.PlaceOrder("entry_long", OrderSide.Buy, OrderType.Market, 1);
            }
            else if (candle.Close < ema200.Ema && macd.First().Histogram > 0 && macd.Last().Histogram < 0 && atr.Atr < 500)
            {
                DataProvider.PlaceOrder("entry_short", OrderSide.Sell, OrderType.Market, 1);
            }

        }

        public void OnOrderStatusChanged(Order order)
        {
            if (order.Status == OrderStatus.Filled)
            {
                if (order.ClientOrderId.Equals("entry_long"))
                {
                    var fract = lastCandles.GetFractal(5).Select(f => f.FractalBull).Where(f => f is not null).Last();
                    var slPrice = fract;
                    var tpPrice = order.Price + 2m * (order.Price - slPrice);
                    DataProvider.PlaceOrder("long_tp", OrderSide.Sell, OrderType.Limit, 1, tpPrice);
                    DataProvider.PlaceOrder("long_sl", OrderSide.Sell, OrderType.Limit, 1, slPrice);
                }
                else if (order.ClientOrderId.Equals("entry_short"))
                {
                    var fract = lastCandles.GetFractal(5).Select(f=>f.FractalBear).Where(f=>f is not null).Last();
                    var slPrice = fract;
                    var tpPrice = order.Price - 2m * (slPrice - order.Price);
                    DataProvider.PlaceOrder("short_tp", OrderSide.Buy, OrderType.Limit, 1, tpPrice);
                    DataProvider.PlaceOrder("short_sl", OrderSide.Buy, OrderType.Limit, 1, slPrice);
                }
                else
                    DataProvider.CancelAllOpenOrders();
            }
        }
    }
}
