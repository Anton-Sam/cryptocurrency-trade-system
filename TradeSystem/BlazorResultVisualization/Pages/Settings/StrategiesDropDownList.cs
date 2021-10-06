using BlazorResultVisualization.DataServices;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DropDowns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorResultVisualization.Pages.Settings
{
    public partial class StrategiesDropDownList
    {
        [Inject]
        public SettingsDataService SettingsDataService { get; set; }

        private string _selectedStrategyName;
        
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private void OnValueChange(ChangeEventArgs<string, string> args)
        {
            SettingsDataService.Strategy = SettingsDataService.StrategiesDict[_selectedStrategyName];
        }
    }
}
