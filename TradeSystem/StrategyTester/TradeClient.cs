using Microsoft.Extensions.Logging;
using StrategyTester.DataProviders;
using StrategyTester.Models.Common;
using StrategyTester.Models.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyTester
{
    public class TradeClient
    {
        public TradeClient() { }
        public TradeClient(ILogger logger) {
            logger.LogInformation("Trade client created");
        }

        public TestingResult StartTest(BaseStrategy strategy,TestingSettings settings)
        {
            var paperDataProvider = new PaperDataProvider();
            strategy.DataProvider = paperDataProvider;

            paperDataProvider.OnCandleClosed += strategy.OnCandleClosed;
            paperDataProvider.OnOrderStatusChanged += strategy.OnOrderStatusChanged;

            var result= paperDataProvider.StartTrading(settings);
            return result;
        }

        public async Task<TestingResult> StartTestAsync(BaseStrategy strategy,TestingSettings settings)
        {
            return await Task.Run(() => StartTest(strategy,settings));
        }
    }
}
