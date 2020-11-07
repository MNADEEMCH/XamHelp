﻿using System.Threading.Tasks;
using MVP.Pages;
using MVP.Services.Interfaces;
using TinyNavigationHelper;
using Xamarin.CommunityToolkit.ObjectModel;

namespace MVP.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        public IAsyncCommand LogoutCommand { get; set; }

        public ProfileViewModel(IAnalyticsService analyticsService, IAuthService authService, IDialogService dialogService, INavigationHelper navigationHelper)
            : base(analyticsService, authService, dialogService, navigationHelper)
        {
            LogoutCommand = new AsyncCommand(() => Logout());
        }

        async Task Logout()
        {
            if (await AuthService.SignOutAsync())
            {
                await MvpApiService.ClearAllLocalData();
                NavigationHelper.SetRootView(nameof(IntroPage));
            }
        }
    }
}
