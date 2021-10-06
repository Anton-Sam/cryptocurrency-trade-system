using BlazorResultVisualization.DataServices;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorResultVisualization.Pages.Dashboard
{
    public partial class ResultCards
    {
        [CascadingParameter]
        private TradingDataService TradingDataService { get; set; }

        [Inject]
        public SettingsDataService SettingsDataService { get; set; }

        private string TotalIncome = "$0";
        private string TotalExpense = "$0";
        private string TotalBalance = "$0";
        private string TotalTransactions = "0";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            UpdateCards();
        }

        /// <summary>
        /// Calculation for card values
        /// </summary>
        public void RefreshCards()
        {
            return;
        }
        private void UpdateCards()
        {
            return;
        }
    }
}
