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

        public void OnCandleClosed(Candle candle)
        {
            if (DataProvider.GetOpenOrders().Any())
                return;
            var lastCandles = DataProvider.GetLastCandles(500);
            if (lastCandles is null)
                return;
            
            var ema200 = lastCandles.GetEma(200).Last();
            var mfi = lastCandles.GetMfi().Last();

            if (candle.Close>ema200.Ema && mfi.Mfi<20)
            {
                DataProvider.PlaceOrder("entry_long", OrderSide.Buy, OrderType.Market, 1);
            }
            else if (candle.Close < ema200.Ema && mfi.Mfi > 80)
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
                    DataProvider.PlaceOrder("long_tp", OrderSide.Sell, OrderType.Limit, 1, 1.006m * order.Price);
                    DataProvider.PlaceOrder("long_sl", OrderSide.Sell, OrderType.Limit, 1, 0.998m * order.Price);
                }
                else if (order.ClientOrderId.Equals("entry_short"))
                {
                    DataProvider.PlaceOrder("short_tp", OrderSide.Buy, OrderType.Limit, 1, 0.994m * order.Price);
                    DataProvider.PlaceOrder("short_sl", OrderSide.Buy, OrderType.Limit, 1, 1.002m * order.Price);
                }
                else
                    DataProvider.CancelAllOpenOrders();
            }
        }
    }
}
