using Binance.Net;
using System;

namespace TestLib
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new BinanceClient();
            client.General.GetAccountInfoAsync().Result.Data.Balances
        }
    }
}
