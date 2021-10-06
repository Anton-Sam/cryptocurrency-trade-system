using BlazorResultVisualization.DataServices;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorResultVisualization.Pages.Settings
{
    public partial class Settings
    {
        [Inject]
        public SettingsDataService SettingsDataService { get; set; }
        public string SearchValue { get; set; }
        private bool ShowSpinner = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

        }
        public void Dispose()
        {

        }
    }

    
}
