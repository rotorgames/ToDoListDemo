using Prism.Mvvm;
using Prism.Navigation;

namespace ToDoListDemo.ViewModels.Base
{
    public abstract class ViewModelBase : BindableBase, INavigationAware, IInitialize
    {
        public virtual void Initialize(INavigationParameters parameters) { }

        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }

        public virtual void OnNavigatedTo(INavigationParameters parameters) { }
    }
}
