using CryptoTrader.Enums;

namespace CryptoTrader.Models
{
    class FuturePosition
    {
        public string Symbol { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal Quantity { get; set; }
        //public PositionSide PositionSide { get; set; }
    }
}
