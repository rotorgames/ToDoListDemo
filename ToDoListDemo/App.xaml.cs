using System;
using System.IO;
using Prism.Ioc;
using Prism.Unity;
using ToDoListDemo.Abstractions.Tasks;
using ToDoListDemo.Services.Tasks;
using ToDoListDemo.ViewModels;
using ToDoListDemo.Views;
using Xamarin.Forms;

namespace ToDoListDemo
{
    public partial class App : PrismApplication
    {
        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("/NavigationPage/TaskListView");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "main.db3");

            // Services
            containerRegistry.RegisterInstance<ITaskStorageService>(new TaskStorageService(databasePath));

            // Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<TaskListView, TaskListViewModel>();
            containerRegistry.RegisterForNavigation<TaskEditorView, TaskEditorViewModel>();
        }
    }
}
