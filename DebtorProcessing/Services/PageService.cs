using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DevExpress.Mvvm;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorProcessing.Services
{
    public class PageService : ReactiveObject
    {
        /// <summary>
        /// Событие смены страницы, на которое подписывается <see cref="MainWindow"/>, чтобы отображать новые страницы.
        /// </summary>
        public event Action<Page> OnPageChanged;

        /// <summary>
        /// Текущая отображаемая <see cref="MainWindow"/> страница. Это свойство только для чтения.
        /// </summary>
        [Reactive] public Page CurrentPage { get; private set; }

        /// <summary>
        /// Метод, осуществляющий переход на указанную страницу и записывающий это в историю.
        /// </summary>
        /// <param name="target">Страница, на которую нужно осуществить переход.</param>
        private void Navigate(Page target)
        {
            if (!(CurrentPage is null)) history.Push(CurrentPage);
            CurrentPage = target;
            OnPageChanged?.Invoke(CurrentPage);
            CanGoBack = history.Count > 0;
        }

        /// <summary>
        /// Метод, осуществляющий переход на указанную страницу без ведения истории,
        /// т.е. без возможности потом вернуться на текущую страницу.
        /// </summary>
        /// <param name="target">Страница, на которую нужно выполнить переход.</param>
        private void NavigateWithoutHistory(Page target)
        {
            CurrentPage = target;
            OnPageChanged?.Invoke(CurrentPage);
        }

        /// <summary>
        /// Меотд, осуществляющий возврат на предыдущую страницу в истории.
        /// </summary>
        private void GoBack()
        {
            Page oldpage = history.Pop();
            NavigateWithoutHistory(oldpage);
            CanGoBack = history.Count > 0;
        }

        private DelegateCommand<Page> navigateCommand;
        /// <summary>
        /// Публичная команда смены страницы, которую дергают Вьюмодели. Описание в <see cref="Navigate"/>.
        /// </summary>
        public DelegateCommand<Page> NavigateCommand => navigateCommand ??=
            new(Navigate);


        private DelegateCommand<Page> navigateWithoutHistoryCommand;
        /// <summary>
        /// Публичная команда смены страницы без записи в историю переходов, которую дергают Вьюмодели. Описание в <see cref="NavigateWithoutHistory"/>
        /// </summary>
        public DelegateCommand<Page> NavigateWithoutHistoryCommand => navigateWithoutHistoryCommand
            ??= new(NavigateWithoutHistory);
        /// <summary>
        /// Свойство, определяющее, возможен ли сейчас переход на предыдущую страницу. Это свойство только для чтения.
        /// </summary>
        [Reactive] public bool CanGoBack { get; private set; }


        private DelegateCommand backCommand;
        /// <summary>
        /// Команда перехода на предыдущую страницу, которую дергают Вьюмодели. Описание в <see cref="GoBack"/>.
        /// </summary>
        public DelegateCommand BackCommand =>
            backCommand ??= new(GoBack, () => CanGoBack);

        /// <summary>
        /// История переход по страницам. Нужна для возможности вернуться назад.
        /// </summary>
        private readonly Stack<Page> history = new();
    }
}
