using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Navigation.TabbedPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WorkTimeSheet.Interfaces;

namespace WorkTimeSheet.ViewModels
{
    public class LogoutViewModel : ViewModelBase
    {
        private readonly IUserSettings _userSettings;
        private ICommand _navigateToDashboardCommand;
        private ICommand _logoutCommand;

        public LogoutViewModel(INavigationService navigationService, IUserSettings userSettings) : base(navigationService)
        {
            _userSettings = userSettings;
            Title = "Logout";
        }


        public ICommand NavigateToDashboardCommand => 
            _navigateToDashboardCommand ?? (_navigateToDashboardCommand = new DelegateCommand(ExecuteNavigateToDashboardCommand));

        public ICommand LogoutCommand => 
            _logoutCommand ?? (_logoutCommand = new DelegateCommand(ExecuteLogoutCommand));

        private void ExecuteLogoutCommand()
        {
            _userSettings.AuthorizedUser = null;
            NavigationService.NavigateAsync("/NavigationPage/MainPage");
        }

        private void ExecuteNavigateToDashboardCommand()
        {
            NavigationService.SelectTabAsync("Dashboard");
        }
    }
}
