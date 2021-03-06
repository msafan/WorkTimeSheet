using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkTimeSheet.Interfaces;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.ViewModels
{
    public class LogsViewModel : ViewModelBase
    {
        private readonly IProjectService _projectService;
        private readonly IUserSettings _userSettings;
        private readonly IWorkLogService _workLogService;
        private List<Project> _projects;
        private ICommand _projectChangedCommand;
        private Project _selectedProject;
        private List<WorkLog> _workLogs;
        private long _totalTime;

        public LogsViewModel(INavigationService navigationService, IUserSettings userSettings, IProjectService projectService, IWorkLogService workLogService) : base(navigationService)
        {
            Title = "Work Logs";
            _projectService = projectService;
            _userSettings = userSettings;
            _workLogService = workLogService;
        }

        public List<Project> Projects { get => _projects; set => SetProperty(ref _projects, value); }
        public List<WorkLog> WorkLogs { get => _workLogs; set => SetProperty(ref _workLogs, value); }
        public Project SelectedProject { get => _selectedProject; set => SetProperty(ref _selectedProject, value); }
        public long TotalTime { get => _totalTime; set => SetProperty(ref _totalTime, value); }
        public ICommand ProjectChangedCommand => _projectChangedCommand ?? (_projectChangedCommand = new DelegateCommand(ExecuteProjectChangedCommand));

        public override void Initialize(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            Initialize();
        }

        private async void Initialize()
        {
            SelectedProject = null;
            WorkLogs = new List<WorkLog>();

            var projects = await _projectService.GetAll(Pagination.NoPagination, new ProjectFilterModel { UserId = _userSettings.AuthorizedUser.UserId });
            Projects = projects.Items;

            if (Projects.Count == 1)
            {
                SelectedProject = Projects.FirstOrDefault();
                await FetchWorkLogs();
            }
        }

        private async void ExecuteProjectChangedCommand()
        {
            await FetchWorkLogs();
        }

        private async Task FetchWorkLogs()
        {
            if (SelectedProject == null || _userSettings == null || _userSettings.AuthorizedUser == null)
                return;

            var workLogs = await _workLogService.GetAll(Pagination.NoPagination, new WorkLogFilterModel
            {

                ProjectIds = new[] { SelectedProject.Id },
                UserIds = new[] { _userSettings.AuthorizedUser.UserId },
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month), 23, 59, 59)
            });
            WorkLogs = workLogs.PaginatedResults.Items;
            TotalTime = workLogs.TotalTime;
        }
    }
}
