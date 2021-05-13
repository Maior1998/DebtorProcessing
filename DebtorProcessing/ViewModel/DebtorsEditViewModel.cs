using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DebtorProcessing.Services;
using DebtorProcessing.View;

using DebtorsDbModel;
using DebtorsDbModel.Model;

using DevExpress.Mvvm;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorProcessing.ViewModel
{
    public class DebtorsEditViewModel : ReactiveObject
    {

        public PageService PageService { get; set; }

        public DebtorsEditViewModel(PageService pageService)
        {
            PageService = pageService;
        }
        public void SetDebtor(Guid id)
        {
            trackingContext = new();
            Debtor = trackingContext.Debtors.Single(x => x.Id == id);
        }

        private Context trackingContext;

        private DelegateCommand backCommand;
        public DelegateCommand BackCommand => backCommand ??= new(GoBack);

        [Reactive] public Debtor Debtor { get; set; }

        private DelegateCommand save;

        private void GoBack()
        {
            PageService.NavigateWithoutHistoryCommand.Execute(new DebtorsTableView());
        }

        public DelegateCommand Save => save ??= new(() =>
        {
            trackingContext.SaveChanges();
            GoBack();
        });
    }
}
