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
    public class MacdEmaStrategy : IStrategy
    {
        public IDataProvider DataProvider { get; set; }

        private IEnumerable<Candle> lastCandles;
        private decimal entryPrice;
        private decimal quantity;
        private decimal balance = 1000;
        public void OnCandleClosed(Candle candle)
        {
            if (DataProvider.GetOpenOrders().Any())
                return;
            lastCandles = DataProvider.GetLastCandles(500);
            if (lastCandles is null)
                return;

            var ema200 = lastCandles.GetEma(200).Last();
            var macds = lastCandles.GetMacd().TakeLast(2);
            quantity = balance / candle.Close;

            if (candle.Close > ema200.Ema && macds.First().Macd < macds.Last().Macd && macds.Last().Macd<-20)
            {
                DataProvider.PlaceOrder("entry_long", OrderSide.Buy, OrderType.Market, quantity);
            }
            else if (candle.Close < ema200.Ema && macds.First().Macd > macds.Last().Macd && macds.Last().Macd>20)
            {
                DataProvider.PlaceOrder("entry_short", OrderSide.Sell, OrderType.Market, quantity);
            }

        }

        public void OnOrderStatusChanged(Order order)
        {
            //var fractals = lastCandles.GetFractal(10);
            //if (order.Status == OrderStatus.Filled)
            //{
            //    if (order.ClientOrderId.Equals("entry_long"))
            //    {
            //        entryPrice = order.Price.Value;

            //        var fracalBull = fractals.Select(f => f.FractalBull).Where(f => f.HasValue && f.Value < order.Price).Last();
            //        var slPrice = fracalBull;
            //        var tpLowPrice = order.Price + (order.Price - slPrice);
            //        var tpHighPrice = order.Price + 2m * (order.Price - slPrice);

            //        DataProvider.PlaceOrder("long_tp_low", OrderSide.Sell, OrderType.Limit, quantity / 2, tpLowPrice);
            //        DataProvider.PlaceOrder("long_tp_high", OrderSide.Sell, OrderType.Limit, quantity / 2, tpHighPrice);
            //        DataProvider.PlaceOrder("long_sl", OrderSide.Sell, OrderType.Limit, quantity, slPrice);
            //    }
            //    else if (order.ClientOrderId.Equals("entry_short"))
            //    {
            //        entryPrice = order.Price.Value;

            //        var fractalBear = fractals.Select(f => f.FractalBear).Where(f => f.HasValue && f.Value > order.Price).Last();
            //        var slPrice = fractalBear;
            //        var tpLowPrice = order.Price - (slPrice - order.Price);
            //        var tpHighPrice = order.Price - 2m * (slPrice - order.Price);

            //        DataProvider.PlaceOrder("short_tp_low", OrderSide.Buy, OrderType.Limit, quantity / 2, tpLowPrice);
            //        DataProvider.PlaceOrder("short_tp_high", OrderSide.Buy, OrderType.Limit, quantity / 2, tpHighPrice);
            //        DataProvider.PlaceOrder("short_sl", OrderSide.Buy, OrderType.Limit, quantity, slPrice);
            //    }
            //    else if (order.ClientOrderId.Equals("long_tp_low"))
            //    {
            //        DataProvider.CancelOrder(clientOrderId: "long_sl");
            //        DataProvider.PlaceOrder("long_sl", OrderSide.Sell, OrderType.Limit, quantity / 2, entryPrice);
            //    }
            //    else if (order.ClientOrderId.Equals("short_tp_low"))
            //    {
            //        DataProvider.CancelOrder(clientOrderId: "short_sl");
            //        DataProvider.PlaceOrder("short_sl", OrderSide.Buy, OrderType.Limit, quantity / 2, entryPrice);
            //    }
            //    else
            //        DataProvider.CancelAllOpenOrders();
            //}

            if (order.Status == OrderStatus.Filled)
            {
                if (order.ClientOrderId.Equals("entry_long"))
                {
                    DataProvider.PlaceOrder("long_tp", OrderSide.Sell, OrderType.Limit, quantity, 1.005m * order.Price);
                    DataProvider.PlaceOrder("long_sl", OrderSide.Sell, OrderType.Limit, quantity, 0.995m * order.Price);
                }
                else if (order.ClientOrderId.Equals("entry_short"))
                {
                    DataProvider.PlaceOrder("short_tp", OrderSide.Buy, OrderType.Limit, quantity, 0.995m * order.Price);
                    DataProvider.PlaceOrder("short_sl", OrderSide.Buy, OrderType.Limit, quantity, 1.005m * order.Price);
                }
                else
                    DataProvider.CancelAllOpenOrders();
            }
        }
    }
}
