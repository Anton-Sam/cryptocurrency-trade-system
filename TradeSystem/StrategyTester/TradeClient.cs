using Microsoft.Extensions.Logging;
using StrategyTester.DataProviders;
using StrategyTester.Models.Interfaces;
using StrategyTester.Models.Test;
using System.Threading.Tasks;

namespace StrategyTester
{
    public class TradeClient
    {
        private readonly ILogger _logger;
        public TradeClient() { }
        public TradeClient(ILogger logger)
        {
            _logger = logger;
            _logger?.LogInformation("Trade client created");
        }

        public TestingResult StartTest(IStrategy strategy, TestingSettings settings)
        {
            var paperDataProvider = new BinanceSpotPaperDataProvider();
            paperDataProvider.Logger = _logger;
            strategy.DataProvider = paperDataProvider;

            paperDataProvider.OnCandleClosed += strategy.OnCandleClosed;
            paperDataProvider.OnOrderStatusChanged += strategy.OnOrderStatusChanged;

            var result = paperDataProvider.StartTrading(settings);
            return result;
        }

        public async Task<TestingResult> StartTestAsync(IStrategy strategy, TestingSettings settings)
        {
            return await Task.Run(() => StartTest(strategy, settings));
        }
    }
}
