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
        public decimal StartBalance { get; set; }
    }
}
