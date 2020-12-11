using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WorkTimeSheet.Interfaces;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.ViewModels
{
    public class MasterDetailViewModel : ViewModelBase
    {
        private List<MenuItem> _menuItems;
        private ICommand _navigateCommand;
        private string _userName;

        public MasterDetailViewModel(INavigationService navigationService, IUserSettings userSettings) : base(navigationService)
        {
            MenuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Title="Dashboard",
                    Icon="Images/dashboard.png",
                    NavigationUri="NavigationPage/Dashboard"
                },
                new MenuItem
                {
                    Title="Logs",
                    Icon="Images/logs.png",
                    NavigationUri="NavigationPage/Logs"
                },
                new MenuItem
                {
                    Title="Logout",
                    Icon="Images/logout.png",
                    NavigationUri="NavigationPage/Logout"
                }
            };

            UserName = userSettings.AuthorizedUser.Name;
        }

        public List<MenuItem> MenuItems { get => _menuItems; set => SetProperty(ref _menuItems, value); }
        public string UserName { get => _userName; set => SetProperty(ref _userName, value); }

        public ICommand NavigateCommand => _navigateCommand ?? (_navigateCommand = new DelegateCommand<MenuItem>(ExecuteNavigateCommand));

        private void ExecuteNavigateCommand(MenuItem menuItem)
        {
            NavigationService.NavigateAsync(menuItem.NavigationUri);
        }
    }
}
