using DebtorsProcessing.Desktop.Services;
using DebtorsProcessing.Desktop.View.Pages;

using ReactiveUI;

namespace DebtorsProcessing.Desktop.ViewModel
{
    public class MainViewModel : ReactiveObject
    {
        public MainViewModel(PageService pageService)
        {
            PageService = pageService;
            pageService.NavigateWithoutHistoryCommand.Execute(new LoginView());
        }

        public PageService PageService { get; set; }
    }
}