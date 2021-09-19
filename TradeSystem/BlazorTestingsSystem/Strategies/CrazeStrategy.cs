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
    public class CrazeStrategy : IStrategy
    {
        public IDataProvider DataProvider { get; set; }

        public void OnCandleClosed(Candle candle)
        {
            if (DataProvider.GetOpenOrders().Any())
                return;
            var lastCandles = DataProvider.GetLastCandles(500);
            if (lastCandles is null)
                return;
            var ema5 = lastCandles.GetEma(5).Last();
            var ema21 = lastCandles.GetEma(21).Last();
            var ema50 = lastCandles.GetEma(50).Last();
            var ema200 = lastCandles.GetEma(200).Last();
            var bb = lastCandles.GetBollingerBands().Last();
            var adx = lastCandles.GetAdx().Last();
            var ao = lastCandles.GetAwesome().Last();

            if (ema5.Ema > ema21.Ema && ema50.Ema > ema200.Ema && bb.PercentB.Value > 0.75m && adx.Adx > 15 && ao.Oscillator.Value > 20m)
            {
                DataProvider.PlaceOrder("entry_long", OrderSide.Buy, OrderType.Market, 1);
            }
            else if (ema5.Ema < ema21.Ema && ema50.Ema < ema200.Ema && bb.PercentB.Value < 0.25m && adx.Adx > 15 && ao.Oscillator.Value < -20m)
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
                    DataProvider.PlaceOrder("long_tp", OrderSide.Sell, OrderType.Limit, 1, 1.01m * order.Price);
                    DataProvider.PlaceOrder("long_sl", OrderSide.Sell, OrderType.Limit, 1, 0.98m * order.Price);
                }
                else if (order.ClientOrderId.Equals("entry_short"))
                {
                    DataProvider.PlaceOrder("short_tp", OrderSide.Buy, OrderType.Limit, 1, 0.99m * order.Price);
                    DataProvider.PlaceOrder("short_sl", OrderSide.Sell, OrderType.Limit, 1, 1.02m * order.Price);
                }
                else
                    DataProvider.CancelAllOpenOrders();
            }
        }
    }
}
