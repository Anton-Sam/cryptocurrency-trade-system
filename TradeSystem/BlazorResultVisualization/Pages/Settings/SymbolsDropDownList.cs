using Binance.Net;
using BlazorResultVisualization.DataServices;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DropDowns;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorResultVisualization.Pages.Settings
{
    public partial class SymbolsDropDownList
    {
        [Inject]
        public SettingsDataService SettingsDataService { get; set; }

        private BinanceClient _client;
        private string _selectedSymbol;
        private IEnumerable<string> _symbols;

        protected override async Task OnInitializedAsync()
        {
            //_client = new BinanceClient();
            //_symbols = (await _client.Spot.System.GetExchangeInfoAsync()).Data.Symbols.Select(s => s.Name);
            _symbols = new List<string> { "btcusdt", "ltcusdt" };
            await base.OnInitializedAsync();
        }

        //private void OnValueChange(ChangeEventArgs<string, string> args)
        //{
        //    SettingsDataService.Symbol = _selectedSymbol;
        //}
    }
}
