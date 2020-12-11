using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkTimeSheet.ViewModels
{
    public class LogsViewModel : ViewModelBase
    {
        public LogsViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
