using BlazorTestingsSystem.DataServices;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorTestingsSystem.Pages.Settings
{
    public partial class Settings
    {

        private Annotation annotation = new Annotation() { };
        private void HandleValidSubmit()
        {
            Console.WriteLine("OnValidSubmit");

        }
        public class Annotation
        {
            [Required]
            public string TextValue { get; set; }
            [Required]
            public string MaskValue { get; set; }
        }


        [Inject]
        public SettingsDataService SettingsDataService { get; set; }

        [Required]
        public string Value { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

        }
        public void Dispose()
        {

        }
    }

    
}
