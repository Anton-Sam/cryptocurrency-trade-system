using Binance.Net;
using StrategyTester.Enums;
using StrategyTester.Extensions;
using StrategyTester.Models;
using StrategyTester.Models.Test;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyTester.Core
{
    static class VirtualExchange
    {
        internal static event Action<Order> OnOrderStatusChanged;
        internal static event Action<Candle> OnCandleClosed;

        private static BinanceClient _client = new BinanceClient();
        private static ExchangeState _state = new ExchangeState();

        internal static void InitializeExchange(string symbol, CandleInterval candleInterval, int historyRange, decimal startBalance)
        {
            _state.TestingSettings = new TestingSettings
            {
                Symbol = symbol,
                StartBalance = startBalance,
                CandleInterval = candleInterval,
                HitoryRange = historyRange
            };
            InitSymbolInfo();
            InitHistory();
            InitBalance(startBalance);
        }

        private static void InitSymbolInfo()
        {
            var binanceSymbolInfo = _client.Spot.System.GetExchangeInfoAsync().Result.Data.Symbols
                .FirstOrDefault(s => s.Name.Equals(_state.TestingSettings.Symbol)) ?? throw new ArgumentException("Inccorect symbol");
            _state.SymbolInfo = new SymbolInfo
            {
                Name = binanceSymbolInfo.Name,
                BaseAsset = binanceSymbolInfo.BaseAsset,
                QuoteAsset = binanceSymbolInfo.QuoteAsset
            };
        }

        private static void InitHistory()
        {
            var binanceCandles = _client.Spot.Market.GetKlinesAsync(
                _state.TestingSettings.Symbol,
                _state.TestingSettings.CandleInterval.ToBinanceKlineInterval(),
                limit: _state.TestingSettings.HitoryRange).Result.Data ?? throw new NullReferenceException("Incorrect hitory params. History data can't be null");

            _state.HistoryCandles = binanceCandles.Select(bin => new Candle
            {
                Close = bin.Close,
                Open = bin.Open,
                Low = bin.Low,
                High = bin.High,
                Volume = bin.BaseVolume,
                Date = bin.CloseTime.AddHours(3)
            }).ToList();
        }

        private static void InitBalance(decimal startBalance)
        {
            _state.BaseAssetBalance = new Balance();
            _state.QuoteAssetBalance = new Balance
            {
                Free = startBalance
            };
        }

        internal static TestingResult StartTrading()
        {
            var result = new TestingResult();
            foreach (var candle in _state.HistoryCandles)
            {
                _state.LastCandle = candle;
                CheckOrders(_state.LastCandle);
                OnCandleClosed?.Invoke(_state.LastCandle);
                CheckOrders(_state.LastCandle);
                result.BalanceChanges.Add(new BalanceChange
                {
                    Balance = GetCurrentBalance(),
                    Date = _state.LastCandle.Date
                });
            }
            result.HistoryData = _state.HistoryCandles;
            result.OrdersHistory = _state.OrdersHistory;
            
            return result;
        }

        private static decimal GetCurrentBalance()
        {
            return _state.QuoteAssetBalance.Total + _state.BaseAssetBalance.Total * _state.LastCandle.Close;
        }

        private static void CheckOrders(Candle candle)
        {
            var openOrders = new List<Order>(_state.OpenOrders);
            foreach (var order in openOrders)
            {
                if (order.Price <= Math.Max(candle.Close, candle.Open) && order.Price >= Math.Min(candle.Close, candle.Open))
                {
                    var newOrder = (Order)order.Clone();
                    newOrder.Status = OrderStatus.Filled;
                    if (newOrder.Side == OrderSide.Buy)
                    {
                        _state.QuoteAssetBalance.Locked -= newOrder.Quantity * newOrder.Price.Value;
                        _state.BaseAssetBalance.Free += newOrder.Quantity;
                    }
                    else
                    {
                        _state.BaseAssetBalance.Locked -= newOrder.Quantity;
                        _state.QuoteAssetBalance.Free += newOrder.Quantity * newOrder.Price.Value;
                    }
                    _state.OpenOrders.RemoveAll(o => o.Id.Equals(newOrder.Id));
                    _state.OrdersHistory.Add(newOrder);
                    OnOrderStatusChanged?.Invoke(newOrder);
                }
            }
        }


        internal static Order PlaceOrder(string clientOrderId, OrderSide side, OrderType type, decimal quantity, decimal? price = null)
        {
            if (!price.HasValue && type != OrderType.Market)
                throw new ArgumentException("Price can be null only with market order");

            var order = new Order
            {
                Id = _state.OrderId.ToString(),
                Symbol = _state.TestingSettings.Symbol,
                ClientOrderId = clientOrderId,
                Side = side,
                Type = type,
                Price = price,
                Quantity = quantity,
                Status = OrderStatus.Opened,
                CreateTime = _state.LastCandle.Date,
                UpdateTime = _state.LastCandle.Date
            };
            _state.OrderId++;

            if (type == OrderType.Market)
                order.Price = _state.LastCandle.Close;


            if (order.Side == OrderSide.Buy)
                AddCurrencyToOrder(_state.QuoteAssetBalance, order.Quantity * order.Price.Value);
            else
                AddCurrencyToOrder(_state.BaseAssetBalance, order.Quantity);

            _state.OrdersHistory.Add(order);
            _state.OpenOrders.Add(order);
            OnOrderStatusChanged?.Invoke(order);
            return order;
        }

        private static void AddCurrencyToOrder(Balance balance, decimal requiredQuantity)
        {
            if (balance.Free < requiredQuantity)
                throw new ArgumentException("Not enough balance");
            balance.Free -= requiredQuantity;
            balance.Locked += requiredQuantity;
        }

        private static void ReturnCurrencyFromOrder(Balance balance, decimal quantity)
        {
            balance.Free += quantity;
            balance.Locked -= quantity;
        }

        internal static Order CancelOrder(string orderId = null, string clientOrderId = null)
        {
            if (string.IsNullOrEmpty(orderId) && string.IsNullOrEmpty(clientOrderId))
                throw new ArgumentException("Either orderId or origClientOrderId must be sent");

            var order = (Order)_state.OpenOrders.FirstOrDefault(ord => ord.Id.Equals(orderId)).Clone();
            if (order is null)
                return null;

            if (order.Side == OrderSide.Buy)
                ReturnCurrencyFromOrder(_state.QuoteAssetBalance, order.Quantity * order.Price.Value);
            else
                ReturnCurrencyFromOrder(_state.BaseAssetBalance, order.Quantity);

            order.Status = OrderStatus.Canceled;
            order.UpdateTime = _state.LastCandle.Date;
            _state.OpenOrders.RemoveAll(ord => ord.Id.Equals(order.Id));
            _state.OrdersHistory.Add(order);
            OnOrderStatusChanged?.Invoke(order);
            return order;
        }

        internal static IEnumerable<Order> CancelAllOpenOrders()
        {
            var openOrders = new List<Order>(_state.OpenOrders);
            var cancelOrders = new List<Order>();
            foreach (var order in openOrders)
            {
                cancelOrders.Add(CancelOrder(order.Id));
            }
            return cancelOrders;
        }

        internal static IEnumerable<Balance> GetAllBalances()
        {
            return new List<Balance> { _state.BaseAssetBalance, _state.QuoteAssetBalance };
        }

        internal static Balance GetBaseAssetBalance()
        {
            return _state.BaseAssetBalance;
        }

        internal static Balance GetQuoteAssetBalance()
        {
            return _state.QuoteAssetBalance;
        }

        internal static IEnumerable<Order> GetOpenOrders()
        {
            return _state.OpenOrders;
        }

        internal static IEnumerable<Candle> GetLastCandles(int count)
        {
            var candles = _state.HistoryCandles?.Where(c => c.Date <= _state.LastCandle.Date).TakeLast(count) ?? throw new NullReferenceException("History not exist");
            if (candles.Count() < count)
                return null;
            return candles;
        }
    }
}
