using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.ViewModels
{
    public class MasterDetailViewModel : ViewModelBase
    {
        private List<MenuItem> _menuItems;
        private ICommand _navigateCommand;

        public MasterDetailViewModel(INavigationService navigationService) : base(navigationService)
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
        }

        public List<MenuItem> MenuItems { get => _menuItems; set => SetProperty(ref _menuItems, value); }

        public ICommand NavigateCommand => _navigateCommand ?? (_navigateCommand = new DelegateCommand<MenuItem>(ExecuteNavigateCommand));

        private void ExecuteNavigateCommand(MenuItem menuItem)
        {
            NavigationService.NavigateAsync(menuItem.NavigationUri);
        }
    }
}
