using ToDoListDemo.Abstractions.Tasks;
using ToDoListDemo.Extensions;
using Xamarin.Forms;

namespace ToDoListDemo.Controls
{
    public partial class TaskStatusView : ContentView
    {
        public static readonly BindableProperty StatusProperty = BindableProperty.Create(nameof(Status), typeof(TaskStatus), typeof(TaskStatusView), TaskStatus.Open, propertyChanged: OnStatusPropertyChanged);

        public TaskStatus Status
        {
            get { return (TaskStatus)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public static readonly BindableProperty IsActiveProperty = BindableProperty.Create(nameof(IsActive), typeof(bool), typeof(TaskStatusView), true);

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public TaskStatusView()
        {
            InitializeComponent();

            UpdateStatus();
        }

        static void OnStatusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((TaskStatusView)bindable).UpdateStatus();
        }

        void UpdateStatus()
        {
            StatusNameLabel.Text = Status.GetLocalizableName().ToLower();
        }
    }
}
