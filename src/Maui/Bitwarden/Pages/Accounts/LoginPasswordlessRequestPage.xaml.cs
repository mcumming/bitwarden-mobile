﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bit.App.Models;
using Bit.App.Utilities;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Bit.App.Pages
{
    public partial class LoginPasswordlessRequestPage : BaseContentPage
    {
        private LoginPasswordlessRequestViewModel _vm;
        private readonly AppOptions _appOptions;

        public LoginPasswordlessRequestPage(string email, AppOptions appOptions = null)
        {
            InitializeComponent();
            _appOptions = appOptions;
            _vm = BindingContext as LoginPasswordlessRequestViewModel;
            _vm.Page = this;
            _vm.Email = email;
            _vm.StartTwoFactorAction = () => Dispatcher.Dispatch(async () => await StartTwoFactorAsync());
            _vm.LogInSuccessAction = () => Dispatcher.Dispatch(async () => await LogInSuccessAsync());
            _vm.UpdateTempPasswordAction = () => Dispatcher.Dispatch(async () => await UpdateTempPasswordAsync());
            _vm.CloseAction = () => { Navigation.PopModalAsync(); };

            _vm.CreatePasswordlessLoginCommand.Execute(null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm.StartCheckLoginRequestStatus();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _vm.StopCheckLoginRequestStatus();
        }

        private async Task StartTwoFactorAsync()
        {
            var page = new TwoFactorPage(false, _appOptions);
            await Navigation.PushModalAsync(new NavigationPage(page));
        }

        private async Task LogInSuccessAsync()
        {
            if (AppHelpers.SetAlternateMainPage(_appOptions))
            {
                return;
            }
            var previousPage = await AppHelpers.ClearPreviousPage();
            Application.Current.MainPage = new TabsPage(_appOptions, previousPage);
        }

        private async Task UpdateTempPasswordAsync()
        {
            var page = new UpdateTempPasswordPage();
            await Navigation.PushModalAsync(new NavigationPage(page));
        }
    }
}

