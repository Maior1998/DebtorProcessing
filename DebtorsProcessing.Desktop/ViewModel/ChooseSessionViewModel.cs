using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;
using DebtorsProcessing.Desktop.Services;
using DebtorsProcessing.Desktop.View.Pages;

using DevExpress.Mvvm;

using DynamicData;
using DynamicData.Binding;

using Microsoft.EntityFrameworkCore;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DebtorsProcessing.Desktop.ViewModel
{
    public class ChooseSessionViewModel : ReactiveObject
    {
        private SessionService session;
        private PageService pageService;
        public ChooseSessionViewModel(SessionService session, PageService pageService)
        {
            this.session = session;
            this.pageService = pageService;
            DebtorsContext context = new();
            List<UserSession> sessions = context.Sessions
                .Include(x => x.Roles)
                .ThenInclude(x => x.Objects)
                .Where(x => x.User.Id == session.UserId && x.EndDate == null)
                .OrderBy(x => x.StartDate)
                .ToList();
            AvailiableRoles = context.Users.Include(x => x.UserRoles).Single(x => x.Id == session.UserId).UserRoles.Select(x => new SelectRoleItem() { Role = x }).ToArray();
            Sessions = new(sessions.Select(x => new SelectSessionItem() { Session = x }));
            Sessions.ToObservableChangeSet().WhenAnyPropertyChanged(nameof(SelectSessionItem.IsChecked)).Subscribe(_ => OnSelectionChanged());
            Sessions.First().IsChecked = true;
        }

        private void OnSelectionChanged()
        {
            IsNewSessionOptionSelected = SelectedSession.Id == Guid.Empty;
            IsExistingSessionOptionSelected = !IsNewSessionOptionSelected;
        }

        [Reactive] public bool IsNewSessionOptionSelected { get; set; }
        [Reactive] public bool IsExistingSessionOptionSelected { get; set; }

        private AsyncCommand chooseSession;
        public AsyncCommand ChooseSession => chooseSession ??= new(async () =>
        {
            session.UserSession = SelectedSession;
            await System.Windows.Application.Current.Dispatcher.InvokeAsync(() => pageService.NavigateWithoutHistoryCommand.Execute(new TabsView()));
        });

        private AsyncCommand createSession;
        public AsyncCommand CreateSession => createSession ??= new(async () =>
        {
            Guid[] selectedRoles = AvailiableRoles.Select(x => x.Role.Id).ToArray();
            DebtorsContext db = new();
            UserRole[] roles = await db.UserRoles.Where(x => selectedRoles.Contains(x.Id)).ToArrayAsync();
            User user = await db.Users.SingleAsync(x => x.Id == session.UserId);
            UserSession newSession = new()
            {
                Id = Guid.NewGuid(),
                StartDate = DateTime.Now,
                Roles = roles,
                User = user
            };
            db.Sessions.Add(newSession);
            await db.SaveChangesAsync();
            session.UserSession = newSession;
            await System.Windows.Application.Current.Dispatcher.InvokeAsync(() => pageService.NavigateWithoutHistoryCommand.Execute(new TabsView()));
        }, () => AvailiableRoles != null && AvailiableRoles.Any(x => x.IsChecked));


        private UserSession SelectedSession => Sessions.First(x => x.IsChecked).Session;
        public ObservableCollection<SelectSessionItem> Sessions { get; set; } = new();

        public SelectRoleItem[] AvailiableRoles { get; set; } = new SelectRoleItem[0];

        [Reactive] public SecurityObject[] AcessedObjects { get; set; } = new SecurityObject[0];

        public class SelectRoleItem : ReactiveObject
        {
            public UserRole Role { get; set; }
            [Reactive] public bool IsChecked { get; set; }
        }

        public class SelectSessionItem : ReactiveObject
        {
            public UserSession Session { get; set; }
            [Reactive] public bool IsChecked { get; set; }
        }
    }
}
