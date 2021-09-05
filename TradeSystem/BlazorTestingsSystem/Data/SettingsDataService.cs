using Binance.Net;
using BlazorTestingsSystem.Strategies;
using StrategyTester.Enums;
using StrategyTester.Models;
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
        public Dictionary<string,BaseStrategy> StrategiesDict { get; set; }
        public IEnumerable<string> Symbols { get; set; }
        public IEnumerable<Interval> Intervals { get; set; }

        public SettingsDataService()
        {
            var client = new BinanceClient();
            
            StrategiesDict = new Dictionary<string, BaseStrategy>();
            StrategiesDict.Add("TwoEma", new TwoEmaStrategy());

            Symbols = client.Spot.System.GetExchangeInfoAsync().Result.Data.Symbols.Select(s => s.Name);

            Intervals = GetEnumDisplayNames<CandleInterval>();
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
