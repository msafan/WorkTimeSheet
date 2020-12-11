using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkTimeSheet.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Dashboard";
        }
    }
}
