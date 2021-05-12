using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DebtorProcessing.Services;
using DebtorProcessing.View;
using ReactiveUI;

namespace DebtorProcessing
{
    public class MainViewModel : ReactiveObject
    {
        public PageService PageService { get; set; }

        public MainViewModel(PageService pageService)
        {
            PageService = pageService;
            pageService.NavigateWithoutHistoryCommand.Execute(new LoginView());
        }
    }
}
