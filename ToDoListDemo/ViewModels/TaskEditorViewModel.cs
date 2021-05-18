using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using ToDoListDemo.Abstractions.Tasks;
using ToDoListDemo.LocalizableResources;
using ToDoListDemo.Services.Tasks;
using ToDoListDemo.ViewModels.Base;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using ATaskStatus = ToDoListDemo.Abstractions.Tasks.TaskStatus;

namespace ToDoListDemo.ViewModels
{
    public class TaskEditorViewModel : ViewModelBase
    {
        readonly INavigationService _navigationService;
        readonly IPageDialogService _pageDialogService;
        readonly ITaskStorageService _taskStorageService;

        ITaskModel _task;

        string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        ATaskStatus _status;
        public ATaskStatus Status
        {
            get { return _status; }
            private set { SetProperty(ref _status, value); }
        }

        bool _isNew;
        public bool IsNew
        {
            get { return _isNew; }
            private set { SetProperty(ref _isNew, value); }
        }

        public ICommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand SetStatusCommand { get; }

        public TaskEditorViewModel(
            INavigationService navigationService,
            IPageDialogService pageDialogService,
            ITaskStorageService taskStorageService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _taskStorageService = taskStorageService;

            SaveCommand = new AsyncCommand(OnSave);
            DeleteCommand = new AsyncCommand(OnDelete);

            SetStatusCommand = new Command<ATaskStatus>(SetTaskStatus);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.ContainsKey("taskId"))
                LoadTask(parameters.GetValue<Guid>("taskId"));
            else
                IsNew = true;
        }

        async void LoadTask(Guid taskId)
        {
            _task = await _taskStorageService.GetTaskAsync(taskId);

            if (_task == null)
                throw new InvalidOperationException($"Task with {taskId} id could not be found");

            Name = _task.Name;
            Description = _task.Description;
            Status = _task.Status;
        }

        async Task OnSave()
        {
            if(string.IsNullOrEmpty(Name))
            {
                await _pageDialogService.DisplayAlertAsync(AppResources.Error, AppResources.TaskNameIsRequired, AppResources.Ok);

                return;
            }

            var task = IsNew ? new TaskModel() : _task;

            task.Name = Name;
            task.Description = Description;
            task.Status = Status;

            if (IsNew)
                task.CreatingDate = DateTime.Now;

            await _taskStorageService.SetTaskAsync(task);

            var np = new NavigationParameters();

            if (IsNew)
                np.Add("newTaskId", task.Id);
            else
                np.Add("updateTaskId", task.Id);

            await _navigationService.GoBackAsync(np);
        }

        async Task OnDelete()
        {
            if (_task == null)
                return;

            var result = await _pageDialogService.DisplayAlertAsync(
                AppResources.Deleting,
                AppResources.ConfirmDelete,
                AppResources.Ok,
                AppResources.Cancel);

            if (!result)
                return;

            await _taskStorageService.DeleteTaskAsync(_task.Id);

            await _navigationService.GoBackAsync(new NavigationParameters
            {
                { "deleteTaskId", _task.Id }
            });
        }

        void SetTaskStatus(ATaskStatus status)
        {
            Status = status;
        }
    }
}
