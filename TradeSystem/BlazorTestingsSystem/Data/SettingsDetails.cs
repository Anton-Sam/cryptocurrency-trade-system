using BlazorTestingsSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace BlazorTestingsSystem.Data
{
    public class SettingsDetails
    {
        [Required]
        public string Symbol { get; set; }

        [Required]
        public CandleInterval? CandleInterval { get; set; }

        [Required]
        public int? HistoryRange { get; set; }

        [Required]
        public double? StartBalance { get; set; }

        [Required]
        public string StrategyName { get; set; }
    }

    public class Interval
    {
        public string Name { get; set; }
        public CandleInterval? CandleInterval { get; set; }
    }
}

