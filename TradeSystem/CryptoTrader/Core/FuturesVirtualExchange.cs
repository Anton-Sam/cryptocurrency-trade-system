using CryptoTrader.Enums;
using CryptoTrader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Core
{
    class FuturesVirtualExchange : VirtualExchange
    {
        //protected override void RecalculateBalances(Order order)
        //{
        //    var symbolInfo = SymbolsInfo.FirstOrDefault(s => s.Name.Equals(order.Symbol));
        //    var balance = Balances.FirstOrDefault(b => b.Asset.Equals(symbolInfo.QuoteAsset));
        //    var quoteAmmount = order.Quantity * order.Price;

        //    if (order.Status == OrderStatus.Opened)
        //    {
        //        balance.Free -= quoteAmmount;
        //        balance.Locked += quoteAmmount;
        //    }
        //    else if (order.Status == OrderStatus.Filled)
        //    {
        //        balance.Locked -= quoteAmmount;
        //        if (order.Side==OrderSide.Buy)
        //        {
        //            var shortPosition=Positions.FirstOrDefault(p => p.Symbol.Equals(order.Symbol) && p.PositionSide == PositionSide.Short);
        //            if (shortPosition is null)
        //            {
        //                var longPosition = Positions.FirstOrDefault(p => p.Symbol.Equals(order.Symbol) && p.PositionSide == PositionSide.Long);
        //                if (longPosition is null)
        //                {
        //                    Positions.Add(new FuturePosition
        //                    {
        //                        Symbol = order.Symbol,
        //                        EntryPrice = order.Price,
        //                        PositionSide = PositionSide.Long,
        //                        Quantity = order.Quantity
        //                    });
        //                }
        //                else
        //                {
        //                    longPosition.Quantity += order.Quantity;
        //                    longPosition.EntryPrice = (longPosition.EntryPrice * longPosition.Quantity + order.Price * order.Quantity) / (longPosition.Quantity + order.Quantity);
        //                }
        //            }
        //            else
        //            {

        //            }
        //        }
        //        else
        //        {

        //        }
        //    }
        //}
        internal override void StartSimulation(string symbol)
        {
            base.StartSimulation();
            CalculateProfit
        }

        private decimal CalculateProfit(string symbol,decimal balance)
        {
            var filledOrders = Orders.Where(o => o.Symbol.Equals(symbol) && o.Status == OrderStatus.Filled);
            foreach (var order in filledOrders)
            {
                var assetQuantity = order.Quantity * order.Price;
                if (order.Type == OrderType.Market)
                    balance -= assetQuantity * TakerFee;
                else
                    balance -= assetQuantity * MakerFee;

                if (order.Side==OrderSide.Buy)
                    balance -= assetQuantity;
                else
                    balance += assetQuantity;
            }
            return balance;
        }

    }
}
