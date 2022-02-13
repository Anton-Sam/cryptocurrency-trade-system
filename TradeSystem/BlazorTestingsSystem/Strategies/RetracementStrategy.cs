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
    public class RetracementStrategy : IStrategy
    {
        public IDataProvider DataProvider { get; set; }

        private IEnumerable<Candle> lastCandles;
        private decimal entryPrice;
        private decimal quantity;
        private decimal initBalance = 100;
        private decimal balance = 10000;
        private decimal ema20Angle = 0.001m;
        private decimal ema80Angle = 0.0004m;
        private decimal percentRetr = 0.45m;
        private decimal acc = 0.0003m;
        private decimal risk = 1.5m;
        private Candle entryCandle;
        private int candleCounter = 0;
        private int maxCounter = 5;
        public void OnCandleClosed(Candle candle)
        {
            var orders = DataProvider.GetOpenOrders();
            if (orders.Any())
            {
                var entryOrders = orders.Where(o => o.ClientOrderId.Contains("entry"));
                if (entryOrders.Any())
                {

                    if (candleCounter < maxCounter)
                        candleCounter++;
                    else
                    {
                        DataProvider.CancelAllOpenOrders();
                        candleCounter = 0;
                    }
                }
                return;
            }
            lastCandles = DataProvider.GetLastCandles(500);
            if (lastCandles is null)
                return;

            var ema80 = lastCandles.GetEma(80).TakeLast(2).Select(e => e.Ema);
            var ema20 = lastCandles.GetEma(20).TakeLast(2).Select(e => e.Ema);
            quantity = balance / candle.Close;
            var angle20 = ema20.Last() / ema20.First();
            var angle80 = ema80.Last() / ema80.First();
            if (ema20.Last() > ema80.Last() &&
                angle20 >= 1 + ema20Angle &&
                angle80 >= 1 + ema80Angle &&
                candle.IsBearishRetracement(percentRetr))
            {
                DataProvider.PlaceOrder("entry_long", OrderSide.Buy, OrderType.Limit, quantity, candle.High * (1 + acc));
                entryCandle = candle;
                candleCounter++;
            }
            else if (ema20.Last() < ema80.Last() &&
                angle20 <= 1 - ema20Angle &&
                angle80 <= 1 - ema80Angle &&
                candle.IsBullishhRetracement(percentRetr))
            {
                DataProvider.PlaceOrder("entry_short", OrderSide.Sell, OrderType.Limit, quantity, candle.Low * (1 - acc));
                entryCandle = candle;
                candleCounter++;
            }

        }

        public void OnOrderStatusChanged(Order order)
        {
            if (order.Status == OrderStatus.Filled)
            {
                if (order.ClientOrderId.Equals("entry_long"))
                {
                    var slPrice = entryCandle.Low * (1 - acc);
                    var tpPrice = ((order.Price - slPrice) * risk + order.Price) * (1 + acc);
                    DataProvider.PlaceOrder("long_tp", OrderSide.Sell, OrderType.Limit, quantity, tpPrice);
                    DataProvider.PlaceOrder("long_sl", OrderSide.Sell, OrderType.Limit, quantity, slPrice);
                }
                else if (order.ClientOrderId.Equals("entry_short"))
                {
                    var slPrice = entryCandle.High * (1 + acc);
                    var tpPrice = ((order.Price - slPrice) * risk + order.Price) * (1 - acc);
                    DataProvider.PlaceOrder("short_tp", OrderSide.Buy, OrderType.Limit, quantity, tpPrice);
                    DataProvider.PlaceOrder("short_sl", OrderSide.Buy, OrderType.Limit, quantity, slPrice);
                }
                else if (order.ClientOrderId.Contains("tp"))
                {
                    //balance = initBalance;
                    DataProvider.CancelAllOpenOrders();
                }
                else if (order.ClientOrderId.Contains("sl"))
                {
                    //balance *= 2;
                    DataProvider.CancelAllOpenOrders();
                }


            }
        }
    }
    public static class CandleExtensions
    {
        public static bool IsBearishRetracement(this Candle candle, decimal percent)
        {
            var candleSize = candle.High - candle.Low;
            var topShadow = candle.High - Math.Max(candle.Close, candle.Open);

            return topShadow / candleSize > percent;
        }
        public static bool IsBullishhRetracement(this Candle candle, decimal percent)
        {
            var candleSize = candle.High - candle.Low;
            var botShadow = Math.Min(candle.Close, candle.Open) - candle.Low;

            return botShadow / candleSize > percent;
        }
    }
}


