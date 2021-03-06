using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkTimeSheet.Interfaces;

namespace WorkTimeSheet.ViewModels
{
    public class LogoutViewModel : ViewModelBase
    {
        private readonly IUserSettings _userSettings;

        public LogoutViewModel(INavigationService navigationService, IUserSettings userSettings) : base(navigationService)
        {
            _userSettings = userSettings;
            Title = "Logout";
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            _userSettings.AuthorizedUser = null;
            NavigationService.NavigateAsync("/NavigationPage/MainPage");
        }
    }
}
