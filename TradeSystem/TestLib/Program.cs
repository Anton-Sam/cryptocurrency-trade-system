using System;
using System.Linq;

namespace TestLib
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var strategy = new TestStrategy();
            var results=strategy.StartTest("ETHBTC", StrategyTester.Enums.CandleInterval.OneHour, 1000, 1);
            Console.WriteLine(results.BalanceChanges.First().Balance);
            Console.WriteLine(results.BalanceChanges.Last().Balance);

        }
    }
}
