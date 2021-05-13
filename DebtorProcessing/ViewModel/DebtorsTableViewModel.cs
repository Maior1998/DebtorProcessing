using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DebtorProcessing.Services;

using DebtorsDbModel;
using DebtorsDbModel.Model;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorProcessing.ViewModel
{
    public class DebtorsTableViewModel : ReactiveObject
    {
        public SessionService session;
        public DebtorsTableViewModel(SessionService session)
        {
            this.session = session;
            UpdateDebtors();
        }

        public void UpdateDebtors()
        {
            DbModel model = new();
            Debtors = model.Debtors.ToArray();
        }

        [Reactive] public Debtor[] Debtors { get; set; } = new Debtor[0];


    }
}
