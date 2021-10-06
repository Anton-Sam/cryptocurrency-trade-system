using Binance.Net;
using Microsoft.Extensions.Logging;
using StrategyTester.Enums;
using StrategyTester.Extensions;
using StrategyTester.Models;
using StrategyTester.Models.Interfaces;
using StrategyTester.Models.Test;
using StrategyTester.Test;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrategyTester.DataProviders
{
    class BinanceSpotPaperDataProvider : IDataProvider
    {
        public event Action<Order> OnOrderStatusChanged;
        public event Action<Candle> OnCandleClosed;

        private static BinanceClient _client;
        private static ExchangeState _state;

        public ILogger Logger { get; set; }

        private void InitializeData(TestingSettings settings)
        {
            _state = new ExchangeState();
            _client = new BinanceClient();

            _state.TestingSettings = settings;

            InitSymbolInfo();
            InitHistory();
            InitBalance(_state.TestingSettings.StartBalance);

            _client.Dispose();
        }

        private void InitSymbolInfo()
        {
            var binanceSymbolInfo = _client.Spot.System.GetExchangeInfoAsync().Result.Data.Symbols
                .FirstOrDefault(s => s.Name.Equals(_state.TestingSettings.Symbol));
            if (binanceSymbolInfo is null)
            {
                Logger.LogError("Symbols info is null. Incorrect symbol.");
                throw new ArgumentNullException("Symbols info is null. Incorrect symbol.");
            }
            _state.SymbolInfo = new SymbolInfo
            {
                Name = binanceSymbolInfo.Name,
                BaseAsset = binanceSymbolInfo.BaseAsset,
                QuoteAsset = binanceSymbolInfo.QuoteAsset
            };
        }

        private void InitHistory()
        {
            var binanceCandles = _client.Spot.Market.GetKlinesAsync(
                _state.TestingSettings.Symbol,
                _state.TestingSettings.CandleInterval.ToBinanceKlineInterval(),
                limit: _state.TestingSettings.HitoryRange).Result.Data;
            if (binanceCandles is null)
            {
                Logger.LogError("Incorrect hitory params. History data can't be null");
                throw new NullReferenceException("Incorrect hitory params. History data can't be null");
            }

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

        private void InitBalance(decimal startBalance)
        {
            _state.BaseAssetBalance = new Balance();
            _state.QuoteAssetBalance = new Balance
            {
                Free = startBalance
            };
        }

        internal TestingResult StartTrading(TestingSettings settings)
        {
            Logger.LogInformation("Testing is started");
            InitializeData(settings);
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

        private decimal GetCurrentBalance()
        {
            return _state.QuoteAssetBalance.Total + _state.BaseAssetBalance.Total * _state.LastCandle.Close;
        }

        private void CheckOrders(Candle candle)
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

        public Balance GetBaseAssetBalance()
        {
            return _state.BaseAssetBalance;
        }

        public Balance GetQuoteAssetBalance()
        {
            return _state.QuoteAssetBalance;
        }
        public IEnumerable<Balance> GetAllBalances()
        {
            return new List<Balance> { _state.BaseAssetBalance, _state.QuoteAssetBalance };
        }

        public Order PlaceOrder(string clientOrderId, OrderSide side, OrderType type, decimal quantity, decimal? price = null)
        {
            if (!price.HasValue && type != OrderType.Market)
            {
                Logger.LogError("Price can be null only with market order");
                throw new ArgumentException("Price can be null only with market order");
            }
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

        private void AddCurrencyToOrder(Balance balance, decimal requiredQuantity)
        {
            if (balance.Free < requiredQuantity)
            {
                Logger.LogInformation("Not enough balance.");
                throw new ArgumentException("Not enough balance.");
            }
            balance.Free -= requiredQuantity;
            balance.Locked += requiredQuantity;
        }

        private void ReturnCurrencyFromOrder(Balance balance, decimal quantity)
        {
            balance.Free += quantity;
            balance.Locked -= quantity;
        }

        public Order CancelOrder(string orderId = null, string clientOrderId = null)
        {
            if (string.IsNullOrEmpty(orderId) && string.IsNullOrEmpty(clientOrderId))
            {
                Logger.LogError("Either orderId or origClientOrderId must be sent");
                throw new ArgumentException("Either orderId or origClientOrderId must be sent");
            }

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

        public IEnumerable<Order> CancelAllOpenOrders()
        {
            var openOrders = new List<Order>(_state.OpenOrders);
            var cancelOrders = new List<Order>();
            foreach (var order in openOrders)
            {
                cancelOrders.Add(CancelOrder(order.Id));
            }
            return cancelOrders;
        }

        public IEnumerable<Order> GetOpenOrders()
        {
            return _state.OpenOrders;
        }

        public IEnumerable<Candle> GetLastCandles(int count)
        {
            var candles = _state.HistoryCandles?.Where(c => c.Date <= _state.LastCandle.Date).TakeLast(count);
            if (candles is null)
            {
                Logger.LogError("History is not exits.");
                throw new NullReferenceException("History is not exits.");
            }
            if (candles.Count() < count)
                return null;
            return candles;
        }
    }
}
