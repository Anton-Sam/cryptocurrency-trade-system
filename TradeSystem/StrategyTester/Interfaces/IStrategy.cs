namespace StrategyTester.Models.Interfaces
{
    public interface IStrategy
    {
        IDataProvider DataProvider { get; set; }
        void OnOrderStatusChanged(Order order);
        void OnCandleClosed(Candle candle);
    }
}
