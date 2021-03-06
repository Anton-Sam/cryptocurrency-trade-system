# StrategyTester
## Description
StrategyTester is an easy-to-use library that allows you to create trading strategies and backtest them using Binance Historical Data.
## How to use it
To create a trading strategy you need to implement interface IStrategy.
````C#
public class TestStrategy : IStrategy
    {
        public IDataProvider DataProvider { get; set; }

        public void OnCandleClosed(Candle candle)
        {
            //Actions on candle close
        }

        public void OnOrderStatusChanged(Order order)
        {
            //Actions on order status change
        }
    }
````
IDataProvider provides you an opportunity to work with orders, balances and market data.
To start backtesting use TradeClient.
````C#
TradeClient client = new TradeClient();
TestingSettings settings=new TestingSettings
{
    Symbol = "BTCUSDT",
    CandleInterrval = CandleInterval.OneHour,
    HitoryRange = 100,
    StartBalance = 100000
});
IStrategy strategy=new TestStrategy();
var testResult=client.StartTest(strategy,settings);
````
## Technologies
* .NET Core
* Binance API
* Blazor

**Thesis project from IT-Academy course (md-nd1-54-21)**


