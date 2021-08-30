using Binance.Net;
using BlazorResultVisualization.DataServices;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorResultVisualization.Pages.Settings
{
    public partial class SymbolsDropDownList
    {
        [Inject]
        public SettingsDataService SettingsDataService { get; set; }

        private BinanceClient _client=new BinanceClient();
        private string _selectedSymbol;
        //private IEnumerable<string> _symbols=_client.Spot.;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
        }
    }
}
