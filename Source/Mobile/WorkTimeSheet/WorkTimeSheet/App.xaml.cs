using Prism;
using Prism.Ioc;
using WorkTimeSheet.Interfaces;
using WorkTimeSheet.Services;
using WorkTimeSheet.ViewModels;
using WorkTimeSheet.Views;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace WorkTimeSheet
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<Dashboard, DashboardViewModel>();
            containerRegistry.RegisterForNavigation<WorkStatus, WorkStatusViewModel>();
            containerRegistry.RegisterForNavigation<WorkLogs, WorkLogsViewModel>();

            containerRegistry.Register<IWebApiLayer, WebApiLayer>();
            containerRegistry.Register<IAuthenticationService, AuthenticationService>();
        }
    }
}
