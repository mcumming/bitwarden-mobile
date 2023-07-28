﻿using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Bit.App.Pages
{
    public partial class AutofillServicesPage : BaseContentPage
    {
        private readonly AutofillServicesPageViewModel _vm;
        private readonly SettingsPage _settingsPage;
        private DateTime? _timerStarted = null;
        private TimeSpan _timerMaxLength = TimeSpan.FromMinutes(5);

        public AutofillServicesPage(SettingsPage settingsPage)
        {
            InitializeComponent();
            _vm = BindingContext as AutofillServicesPageViewModel;
            _vm.Page = this;
            _settingsPage = settingsPage;
        }

        protected async override void OnAppearing()
        {
            await _vm.InitAsync();
            _vm.UpdateEnabled();
            _timerStarted = DateTime.UtcNow;
            // TODO Xamarin.Forms.Device.StartTimer is no longer supported. Use Microsoft.Maui.Dispatching.DispatcherExtensions.StartTimer instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 500), () =>
            {
                if (_timerStarted == null || (DateTime.UtcNow - _timerStarted) > _timerMaxLength)
                {
                    return false;
                }
                _vm.UpdateEnabled();
                return true;
            });
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            _timerStarted = null;
            _settingsPage.BuildList();
            base.OnDisappearing();
        }

        private void ToggleAutofillService(object sender, EventArgs e)
        {
            if (DoOnce())
            {
                _vm.ToggleAutofillService();
            }
        }

        private void ToggleInlineAutofill(object sender, EventArgs e)
        {
            _vm.ToggleInlineAutofill();
        }

        private void ToggleDrawOver(object sender, EventArgs e)
        {
            if (DoOnce())
            {
                _vm.ToggleDrawOver();
            }
        }
    }
}
