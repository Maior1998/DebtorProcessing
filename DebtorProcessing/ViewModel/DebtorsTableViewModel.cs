using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtorsDbModel;
using DebtorsDbModel.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorProcessing.ViewModel
{
    public class DebtorsTableViewModel : ReactiveObject
    {
        public DebtorsTableViewModel()
        {
            UpdateDebtors();
        }

        public void UpdateDebtors()
        {
            DbModel model = new DbModel();
            Debtors = model.Debtors.ToArray();
        }

        [Reactive] public Debtor[] Debtors { get; set; } = new Debtor[0];


    }
}
