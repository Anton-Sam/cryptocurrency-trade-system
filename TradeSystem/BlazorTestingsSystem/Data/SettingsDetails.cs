using StrategyTester.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTestingsSystem.Data
{
    public enum CandleInterval
    {
        [Display(Name = "OneMinute")]
        OneMinute,
        [Display(Name = "ThreeMinute")]
        ThreeMinutes,
        [Display(Name = "FiveMinute")]
        FiveMinutes,
        [Display(Name = "FifteenMinutes")]
        FifteenMinutes,
        [Display(Name = "ThirtyMinutes")]
        ThirtyMinutes,
        [Display(Name = "OneHour")]
        OneHour,
        [Display(Name = "TwoHour")]
        TwoHour,
        [Display(Name = "FourHour")]
        FourHour,
        [Display(Name = "SixHour")]
        SixHour,
        [Display(Name = "EightHour")]
        EightHour,
        [Display(Name = "TwelveHour")]
        TwelveHour,
        [Display(Name = "OneDay")]
        OneDay,
        [Display(Name = "ThreeDay")]
        ThreeDay,
        [Display(Name = "OneWeek")]
        OneWeek,
        [Display(Name = "OneMonth")]
        OneMonth
    }
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

