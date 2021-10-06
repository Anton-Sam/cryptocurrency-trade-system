# Strategy Tester
## Description
Easy to use library for .NET that allows create trade strategies and test them using Binance data.
## Usage
To create a strategy you need implement interface IStrategy.
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
IDataProvider allows you work with orders, balances and market data.
To start testing use TradeClient.
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

**Diploma project for IT-Academy course (md-nd1-54-21)**


