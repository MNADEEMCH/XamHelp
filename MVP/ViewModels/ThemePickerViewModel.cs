using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVP.Helpers;
using MVP.Resources;
using MVP.Services;
using MVP.Services.Interfaces;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MVP.ViewModels
{
    public class ThemePickerViewModel : BaseViewModel
    {
        public IList<AppThemeViewModel> AppThemes { get; set; } = new List<AppThemeViewModel> {
            new AppThemeViewModel() { Key = OSAppTheme.Unspecified, Description = Resources.Translations.theme_systemdefault },
            new AppThemeViewModel() { Key = OSAppTheme.Light, Description = Resources.Translations.theme_light  },
            new AppThemeViewModel() { Key = OSAppTheme.Dark, Description = Resources.Translations.theme_dark  }
        };

        public IAsyncCommand<AppThemeViewModel> SetAppThemeCommand { get; set; }

        public ThemePickerViewModel(IAnalyticsService analyticsService)
            : base(analyticsService)
        {
            SetAppThemeCommand = new AsyncCommand<AppThemeViewModel>((x) => SetAppTheme(x));
            AppThemes.FirstOrDefault(x => x.Key == Settings.AppTheme).IsSelected = true;
        }

        /// <summary>
        /// Sets the theme for the app.
        /// </summary>
        async Task SetAppTheme(AppThemeViewModel theme)
        {
            try
            {
                Application.Current.UserAppTheme = theme.Key;
                Settings.AppTheme = theme.Key;

                foreach (var item in AppThemes)
                    item.IsSelected = false;

                AppThemes.FirstOrDefault(x => x.Key == Settings.AppTheme).IsSelected = true;
                RaisePropertyChanged(nameof(AppThemes));

                //var statusBar = DependencyService.Get<IStatusBar>();
                //statusBar?.SetStatusBarColor((OSAppTheme)theme.Key, Color.Black);

                HapticFeedback.Perform(HapticFeedbackType.Click);

                AnalyticsService.Track("App Theme Changed", nameof(theme), ((OSAppTheme)theme.Key).ToString() ?? "null");
            }
            catch (Exception ex)
            {
                await DialogService.AlertAsync(Translations.error_couldntchangetheme, Translations.error_title, Translations.ok).ConfigureAwait(false);

                AnalyticsService.Report(ex);
            }
        }
    }
}
