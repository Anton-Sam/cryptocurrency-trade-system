﻿using CryptoTrader.Enums;
using CryptoTrader.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Core
{
    class VirtualExchange
    {
        internal ObservableCollection<Order> Orders { get; set; }
        internal IEnumerable<Candle> History { get; set; }
        internal IEnumerable<Balance> Balances { get; set; }
        internal IEnumerable<SymbolInfo> SymbolsInfo { get; set; }
        public decimal MakerFee { get; set; }
        public decimal TakerFee { get; set; }

        internal event Action<Order> OnOrderStatusChanged;
        internal event Action<Candle> OnNewCandle;

        private Candle _lastCandle;

        internal VirtualExchange()
        {
            Orders.CollectionChanged += Orders_CollectionChanged;
        }

        private void Orders_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action==NotifyCollectionChangedAction.Add)
            {
                OnOrderStatusChanged?.Invoke(sender as Order);
                CheckOrders(_lastCandle);
            }
        }

        internal virtual void StartSimulation()
        {
            foreach (var cadle in History)
            {
                OnNewCandle?.Invoke(cadle);
                _lastCandle = cadle;
            }
        }

        private void CheckOrders(Candle candle)
        {
            var openOrders = Orders.Where(o => o.Status == OrderStatus.Opened);
            foreach (var order in openOrders)
            {
                if (order.Price <= Math.Max(candle.Close, candle.Open) && order.Price >= Math.Min(candle.Close, candle.Open))
                {
                    var newOrder = (Order)order.Clone();
                    newOrder.Status = OrderStatus.Filled;
                    RecalculateBalances(newOrder);
                    Orders.Add(newOrder);
                }
            }
        }

        protected virtual void RecalculateBalances(Order order) { }
    }
}
