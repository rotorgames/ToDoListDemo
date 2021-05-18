using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using ToDoListDemo.Abstractions.Tasks;
using ToDoListDemo.Models;
using ToDoListDemo.ViewModels.Base;
using Xamarin.CommunityToolkit.ObjectModel;

namespace ToDoListDemo.ViewModels
{
    public class TaskListViewModel : ViewModelBase
    {
        const int ItemsPerLoad = 30;

        readonly INavigationService _navigationService;
        readonly ITaskStorageService _taskStorageService;

        readonly ObservableRangeCollection<TaskItem> _taskCollection;

        int _loadedItems;
        bool _hasMoreItems = true;

        public IReadOnlyList<TaskItem> Tasks => _taskCollection;

        public ICommand CreateTaskCommand { get; }

        public ICommand EditTaskCommand { get; }

        public ICommand LoadMoreCommand { get; }

        public TaskListViewModel(
            INavigationService navigationService,
            ITaskStorageService taskStorageService)
        {
            _navigationService = navigationService;
            _taskStorageService = taskStorageService;

            _taskCollection = new ObservableRangeCollection<TaskItem>();

            CreateTaskCommand = new AsyncCommand(CreateTaskAsync);
            EditTaskCommand = new AsyncCommand<TaskItem>(EditTaskAsync);
            LoadMoreCommand = new AsyncCommand(LoadTasks);
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            await LoadTasks();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("newTaskId"))
                InsertTask(parameters.GetValue<Guid>("newTaskId"));
            else if (parameters.ContainsKey("updateTaskId"))
                UpdateTask(parameters.GetValue<Guid>("updateTaskId"));
            else if (parameters.ContainsKey("deleteTaskId"))
                DeleteTask(parameters.GetValue<Guid>("deleteTaskId"));
        }

        async void InsertTask(Guid id)
        {
            var task = await _taskStorageService.GetTaskAsync(id);

            _taskCollection.Insert(0, new TaskItem(task));
        }

        async void UpdateTask(Guid id)
        {
            var taskItem = _taskCollection.FirstOrDefault(t => t.Task.Id == id);

            if (taskItem == null)
                return;

            var task = await _taskStorageService.GetTaskAsync(id);

            taskItem.Update(task);
        }

        void DeleteTask(Guid id)
        {
            var item = _taskCollection.FirstOrDefault(t => t.Task.Id == id);

            if (item == null)
                return;

            _taskCollection.Remove(item);
        }

        async Task LoadTasks()
        {
            if (!_hasMoreItems)
                return;
            
            var tasks = await _taskStorageService.GetTasksAsync(_loadedItems, ItemsPerLoad);

            if(tasks.Count < ItemsPerLoad)
                _hasMoreItems = false;

            if (!tasks.Any())
                return;

            _loadedItems += tasks.Count;

            _taskCollection.AddRange(tasks.Select(t => new TaskItem(t)));
        }

        async Task CreateTaskAsync()
        {
            await _navigationService.NavigateAsync("TaskEditorView");
        }

        async Task EditTaskAsync(TaskItem taskItem)
        {
            await _navigationService.NavigateAsync("TaskEditorView", new NavigationParameters
            {
                { "taskId", taskItem.Task.Id }
            });
        }
    }
}
