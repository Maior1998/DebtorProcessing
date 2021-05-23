using DebtorProcessing.Services;
using DebtorProcessing.View;
using ReactiveUI;

namespace DebtorProcessing
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