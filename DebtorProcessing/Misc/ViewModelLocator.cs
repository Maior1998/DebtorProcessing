using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtorProcessing.ViewModel;
using ReactiveUI;

namespace DebtorProcessing.Misc
{
    public class ViewModelLocator : ReactiveObject
    {
        public MainViewModel MainViewModel => App.Resolve<MainViewModel>();
        public LoginViewModel LoginViewModel => App.Resolve<LoginViewModel>();
        public DebtorsTableViewModel DebtorsTableViewModel => App.Resolve<DebtorsTableViewModel>();



        
    }
}
