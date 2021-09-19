using StrategyTester.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyTester.Models.Test
{
    public class TestingSettings
    {
        public string Symbol { get; set; }
        public CandleInterval CandleInterval { get; set; }
        public int HitoryRange { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public decimal StartBalance { get; set; }
        public decimal MakerFee { get; set; }
        public decimal TakerFee { get; set; }
    }
}
