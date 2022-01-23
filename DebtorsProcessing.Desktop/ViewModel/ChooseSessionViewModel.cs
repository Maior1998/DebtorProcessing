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
using System.Threading.Tasks;
using DebtorsProcessing.Api.Dtos.Responses;
using DebtorsProcessing.Desktop.Model;

namespace DebtorsProcessing.Desktop.ViewModel
{
    public class ChooseSessionViewModel : ReactiveObject
    {
        private readonly SessionService session;
        private readonly PageService pageService;
        public ChooseSessionViewModel(SessionService session, PageService pageService)
        {
            try
            {
                this.session = session;
                this.pageService = pageService;
                task = loadRolesAndSessions();

            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
        }

        private async Task loadRolesAndSessions()
        {
            IEnumerable<ChooseUserSessionDto> sessions = await ServiceTalker.GetSessions();
            Sessions = sessions.Select(x => new SelectSessionItem() { Date = x.StartSessionTime, Id = x.Id, RolesString = string.Join(", ", x.RolesInSession.Select(y => y.Name)) }).ToArray();
            IEnumerable<UserRoleDto> roles = await ServiceTalker.GetRoles();
            AvailiableRoles = roles.Select(x => new SelectRoleItem() { Role = new() { Name = x.Name, Id = x.Id } }).ToArray();
        }

        private static Task task;
        private AsyncCommand<SelectSessionItem> chooseSession;
        public AsyncCommand<SelectSessionItem> ChooseSession => chooseSession ??= new(async (userSession) =>
        {
            await ServiceTalker.SelectSession(userSession.Id);
            await System.Windows.Application.Current.Dispatcher.InvokeAsync(() => pageService.NavigateWithoutHistoryCommand.Execute(new TabsView()));
        });

        private AsyncCommand createSession;
        public AsyncCommand CreateSession => createSession ??= new(async () =>
        {
            Guid[] selectedRoles = AvailiableRoles.Where(x => x.IsChecked).Select(x => x.Role.Id).ToArray();
            var createdSession = await ServiceTalker.CreateSesion(selectedRoles);
            var selectedSession = await ServiceTalker.SelectSession(createdSession);
            Console.WriteLine();
            await System.Windows.Application.Current.Dispatcher.InvokeAsync(() => pageService.NavigateWithoutHistoryCommand.Execute(new TabsView()));
        }, () => AvailiableRoles != null && AvailiableRoles.Any(x => x.IsChecked));


        [Reactive] public SelectSessionItem[] Sessions { get; set; }

        [Reactive] public SelectRoleItem[] AvailiableRoles { get; set; }

        public record SelectSessionItem
        {
            public DateTime Date { get; set; }
            public string RolesString { get; set; }
            public Guid Id { get; set; }
            public string String => $"От {Date.ToShortDateString()}. Роли: {RolesString}";
        }

        public class SelectRoleItem : ReactiveObject
        {
            public UserRole Role { get; set; }
            [Reactive] public bool IsChecked { get; set; }
        }

    }
}
