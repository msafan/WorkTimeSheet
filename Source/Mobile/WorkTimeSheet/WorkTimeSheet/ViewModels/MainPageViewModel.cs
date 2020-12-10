using Prism.Commands;
using Prism.Navigation;
using System;
using System.Windows.Input;
using WorkTimeSheet.Interfaces;

namespace WorkTimeSheet.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserSettings _userSettings;
        private string _email;
        private string _password;
        private ICommand _loginCommand;

        public MainPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IUserSettings userSettings)
            : base(navigationService)
        {
            Title = "Main Page";
            _authenticationService = authenticationService;
            _userSettings = userSettings;
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LoginCommand => _loginCommand ??
            (_loginCommand = new DelegateCommand(ExecuteLoginCommand, CanExecuteLoginCommand)
            .ObservesProperty(() => Email)
            .ObservesProperty(() => Password));

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (_userSettings.AuthorizedUser != null)
            {
                try
                {
                    var authorizedUser = await _authenticationService.RefreshAuthentication(_userSettings.AuthorizedUser);
                    _userSettings.AuthorizedUser = authorizedUser;
                    await NavigationService.NavigateAsync("/NavigationPage/Dashboard");
                }
                catch (Exception ex)
                {

                }
            }
        }

        private bool CanExecuteLoginCommand()
        {
            return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
        }

        private async void ExecuteLoginCommand()
        {
            try
            {
                var authorizedUser = await _authenticationService.Authenticate(Email, Password);
                _userSettings.AuthorizedUser = authorizedUser;
                await NavigationService.NavigateAsync("/NavigationPage/Dashboard");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
