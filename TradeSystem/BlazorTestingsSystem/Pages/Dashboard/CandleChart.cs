using BlazorTestingsSystem.DataServices;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTestingsSystem.Pages.Dashboard
{
    public partial class CandleChart
    {
        [CascadingParameter]
        public TradingDataService TradingDataService { get; set; }

        
        //public SettingsDataService SettingsDataService { get; set; }
        [Parameter]
        public bool Render { get; set; } = true;
        private List<CandleData> ChartPoints { get; set; } = new List<CandleData>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var candle = new CandleData
            {
                Date = new DateTime(2020, 01, 01, 0, 0, 0),
                Close = 10,
                Open = 5,
                High = 11,
                Low = 4,
                Volume = 30
            };
            for (int i = 0; i < 100; i++)
            {
                ChartPoints.Add(new CandleData
                {
                    Date = candle.Date.AddMinutes(i),
                    Close = candle.Close + i,
                    Open = candle.Open + i,
                    High = candle.High + i,
                    Low = candle.Low + i,
                    Volume = candle.Volume
                }); ;

            }


            UpdatePieChart();
        }

        /// <summary>
        /// Calculate the total expense and update the pie chart
        /// </summary>
        public void RefreshPieChart()
        {
            UpdatePieChart();
        }

        private void UpdatePieChart()
        {
            //var a = SettingsDataService.HistoryRange;

            return;
        }



        public void Dispose()
        {

        }

        public class CandleData
        {
            public DateTime Date { get; set; }
            public double High { get; set; }
            public double Low { get; set; }
            public double Open { get; set; }
            public double Close { get; set; }
            public double Volume { get; set; }
        }
    }
}
