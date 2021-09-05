//using BlazorTestingsSystem.DataServices;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Forms;
//using Syncfusion.Blazor.Inputs;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;

//namespace BlazorTestingsSystem.Pages.Settings
//{
//    public enum CandleInterval
//    {
//        [Display(Name = "OneMinute")]
//        OneMinute,
//        [Display(Name = "ThreeMinute")]
//        ThreeMinutes,
//        [Display(Name = "FiveMinute")]
//        FiveMinutes,
//        [Display(Name = "FifteenMinutes")]
//        FifteenMinutes,
//        [Display(Name = "ThirtyMinutes")]
//        ThirtyMinutes,
//        [Display(Name = "OneHour")]
//        OneHour,
//        [Display(Name = "TwoHour")]
//        TwoHour,
//        [Display(Name = "FourHour")]
//        FourHour,
//        [Display(Name = "SixHour")]
//        SixHour,
//        [Display(Name = "EightHour")]
//        EightHour,
//        [Display(Name = "TwelveHour")]
//        TwelveHour,
//        [Display(Name = "OneDay")]
//        OneDay,
//        [Display(Name = "ThreeDay")]
//        ThreeDay,
//        [Display(Name = "OneWeek")]
//        OneWeek,
//        [Display(Name = "OneMonth")]
//        OneMonth
//    }
//    public partial class Settings
//    {
//        [Inject]
//        public SettingsDataService SettingsDataService { get; set; }

//        private void click()
//        {
//            cssClass = "cssclasschag";
//        }

//        private CandleInterval? _interval;
//        private SettingsModel model;
//        private EditContext editContext;
//        private string cssClass { get; set; }
//        protected override void OnInitialized()
//        {
//            model = new SettingsModel();
//            editContext = new EditContext(model);
//        }
//        public class SettingsModel
//        {
//            [Required]
//            public string TestProperty { get; set; }
//            [Required]
//            public CandleInterval? Interval { get; set; }
//        }
//        public void TestPropertyBlurEvent(FocusOutEventArgs args)
//        {
//            if (!editContext.Validate())
//            {
//                cssClass = "e-error";
//            }
//            else
//            {
//                cssClass = "e-success";
//            }
//            StateHasChanged();
//        }
//        protected override async Task OnInitializedAsync()
//        {
//            await base.OnInitializedAsync();

//        }
//        public void Dispose()
//        {

//        }
//    }

    
//}
