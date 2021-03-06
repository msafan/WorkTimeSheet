using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkTimeSheet.ViewModels
{
    public class TabbedPageCollectionViewModel : ViewModelBase
    {
        public TabbedPageCollectionViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
