using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DebtorProcessing.Services;

using ReactiveUI;

namespace DebtorProcessing.ViewModel
{
    public class TabsViewModel : ReactiveObject
    {
        public SessionService Session { get; private set; }

        public TabsViewModel(SessionService session)
        {
            Session = session;
        }
    }
}
