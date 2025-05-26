using CommunityToolkit.Mvvm.Input;
using PersonalTracker.Mobile.Models;

namespace PersonalTracker.Mobile.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}