using BlazorTestingsSystem.DataServices;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DropDowns;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorTestingsSystem.Pages.Settings
{
    public partial class IntervalDropDownList
    {
        [Inject]
        public SettingsDataService SettingsDataService { get; set; }

        private Interval _interval = new Interval();
        private IEnumerable<Interval> _intervals = GetEnumDisplayNames<CandleInterval>();


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            SettingsDataService.Interval = (StrategyTester.Enums.CandleInterval)Enum.Parse(typeof(StrategyTester.Enums.CandleInterval), _interval.CandleInterval.ToString());
            Console.WriteLine(SettingsDataService.Interval.ToString());
        }

        public class Interval
        {
            public string Name { get; set; }
            public CandleInterval? CandleInterval { get; set; } = IntervalDropDownList.CandleInterval.OneMinute;
        }

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

        public static List<Interval> GetEnumDisplayNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<T>()
                       .Select(x => new Interval
                       {
                           CandleInterval = x as CandleInterval?,
                           Name = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        private void OnValueChange(ChangeEventArgs<CandleInterval?, Interval> args)
        {
            SettingsDataService.Interval = (StrategyTester.Enums.CandleInterval)Enum.Parse(typeof(StrategyTester.Enums.CandleInterval), _interval.CandleInterval.ToString());
            Console.WriteLine(SettingsDataService.Interval.ToString());
        }
    }
}
