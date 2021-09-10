using Binance.Net;
using BlazorTestingsSystem.Enums;
using BlazorTestingsSystem.Strategies;
using StrategyTester.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorTestingsSystem.Data
{
    public class SettingsDataService
    {
        public Dictionary<string, BaseStrategy> StrategiesDict { get; set; }
        public IEnumerable<string> Symbols { get; set; }
        public IEnumerable<Interval> Intervals { get; set; }
        public SettingsDetails SettingsDetails { get; set; }

        public static async Task<SettingsDataService> CreateAsync()
        {
            var service = new SettingsDataService();
            return await service.InitializeAsync();
        }
        private async Task<SettingsDataService> InitializeAsync()
        {
            using (var client = new BinanceClient())
            {
                StrategiesDict = new Dictionary<string, BaseStrategy>();
                StrategiesDict.Add("TwoEma", new TwoEmaStrategy());

                Intervals = GetEnumDisplayNames<CandleInterval>();

                //SettingsDetails = new SettingsDetails();

                Symbols = (await client.Spot.System.GetExchangeInfoAsync()).Data.Symbols.Select(s => s.Name);
            }
            return this;
        }

        private List<Interval> GetEnumDisplayNames<T>()
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
    }
}
