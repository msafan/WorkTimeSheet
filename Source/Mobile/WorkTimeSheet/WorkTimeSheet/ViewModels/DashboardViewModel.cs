using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using WorkTimeSheet.Interfaces;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IWorkService _workService;
        private readonly IProjectService _projectService;
        private readonly IUserSettings _userSettings;
        private Project _selectedProject;
        private bool _isWorkStarted;
        private List<Project> _projects;
        private CurrentWork _currentWork;
        private TimeSpan _timeSpan;
        private Timer _timer;
        private ICommand _startWorkCommand;
        private ICommand _stopWorkCommand;
        private string _remarks;

        public DashboardViewModel(INavigationService navigationService, IUserSettings userSettings, IWorkService workService, IProjectService projectService) : base(navigationService)
        {
            Title = "Dashboard";
            _workService = workService;
            _projectService = projectService;
            _userSettings = userSettings;

            _timer = new Timer
            {
                AutoReset = true,
                Enabled = false,
                Interval = 1000
            };
            _timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (CurrentWork?.StartDateTime != null)
                TimeSpan = DateTime.UtcNow - CurrentWork.StartDateTime.Value;
        }

        public bool IsWorkStarted { get => _isWorkStarted; set => SetProperty(ref _isWorkStarted, value); }
        public Project SelectedProject { get => _selectedProject; set => SetProperty(ref _selectedProject, value); }
        public List<Project> Projects { get => _projects; set => SetProperty(ref _projects, value); }
        public CurrentWork CurrentWork { get => _currentWork; set => SetProperty(ref _currentWork, value); }
        public TimeSpan TimeSpan { get => _timeSpan; set => SetProperty(ref _timeSpan, value); }
        public string Remarks { get => _remarks; set => SetProperty(ref _remarks, value); }
        public ICommand StartWorkCommand => _startWorkCommand ?? (_startWorkCommand = new DelegateCommand(ExecuteStartWorkCommand, CanExecuteStartWorkCommand).ObservesProperty(() => SelectedProject));
        public ICommand StopWorkCommand => _stopWorkCommand ?? (_stopWorkCommand = new DelegateCommand(ExecuteStopWorkCommand, CanExecuteStopWorkCommand).ObservesProperty(() => Remarks));

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            Initialize();
        }

        private async Task Initialize()
        {
            IsWorkStarted = false;
            SelectedProject = null;

            var projects = await _projectService.GetAll(Pagination.NoPagination, new ProjectFilterModel { UserId = _userSettings.AuthorizedUser.UserId });
            Projects = projects.Items;

            CurrentWork = await _workService.GetCurrentWork();

            if (CurrentWork.ProjectId != null)
            {
                IsWorkStarted = true;
                SelectedProject = Projects.FirstOrDefault(x => x.Id == CurrentWork.ProjectId);

                _timer.Start();
                TimeSpan = DateTime.UtcNow - CurrentWork.StartDateTime.Value;
            }
            else if (projects.Items.Count == 1)
            {
                SelectedProject = Projects.FirstOrDefault();
            }
        }

        private async void ExecuteStartWorkCommand()
        {
            await _workService.StartWork(SelectedProject.Id);
            await Initialize();
        }

        private async void ExecuteStopWorkCommand()
        {
            await _workService.StopWork(Remarks);
            await Initialize();
        }

        private bool CanExecuteStopWorkCommand()
        {
            return !string.IsNullOrEmpty(Remarks);
        }

        private bool CanExecuteStartWorkCommand()
        {
            return SelectedProject != null;
        }
    }
}
