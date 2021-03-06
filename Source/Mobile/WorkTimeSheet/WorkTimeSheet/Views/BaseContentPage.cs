using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkTimeSheet.ViewModels;
using Xamarin.Forms;

namespace WorkTimeSheet.Views
{
    public class BaseContentPage : ContentPage
    {
        public BaseContentPage()
        {
            Appearing += BaseContentPage_Appearing;
        }

        private void BaseContentPage_Appearing(object sender, EventArgs e)
        {
            if (!(BindingContext is ViewModelBase viewModel))
                return;

            viewModel.OnNavigatedTo(null);
        }
    }
}