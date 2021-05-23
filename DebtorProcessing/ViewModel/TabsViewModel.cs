using DebtorProcessing.Services;
using ReactiveUI;

namespace DebtorProcessing.ViewModel
{
    public class TabsViewModel : ReactiveObject
    {
        public TabsViewModel(SessionService session)
        {
            Session = session;
        }

        public SessionService Session { get; }
    }
}