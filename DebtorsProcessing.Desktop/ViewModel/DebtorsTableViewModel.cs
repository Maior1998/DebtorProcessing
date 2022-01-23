using System;
using System.IO;
using System.Linq;
using System.Windows;

using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;
using DebtorsProcessing.Desktop.Model;
using DebtorsProcessing.Desktop.Services;
using DebtorsProcessing.Desktop.View.Pages;

using DevExpress.Mvvm;

using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

using OfficeOpenXml;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorsProcessing.Desktop.ViewModel
{
    public class DebtorsTableViewModel : ReactiveObject
    {
        private DelegateCommand addDebtor;

        private DelegateCommand deleteDebtor;

        private DelegateCommand editDebtor;

        private DelegateCommand exportCurrentDebtorsCollection;
        private readonly PageService pageService;

        private DelegateCommand search;
        private readonly SessionService session;

        public DebtorsTableViewModel(SessionService session, PageService pageService)
        {
            this.session = session;
            this.pageService = pageService;
            UpdateDebtors();
        }

        public DelegateCommand AddDebtor => addDebtor ??= new(() =>
        {
            DebtorsContext context = new();
            Debtor addingDebtor = DatabaseHelperFunctions.GetExampleDebtor();
            User currentLoggedInUser = context.Users.Single(x => x.Id == session.UserId);
            currentLoggedInUser.Debtors.Add(addingDebtor);

            context.Debtors.Add(addingDebtor);
            context.SaveChanges();
            UpdateDebtors();
            SelectedDebtor = Debtors.Single(x => x.Id == addingDebtor.Id);
            EditDebtor.Execute(null);
        }, () => session.CanEditDebtorsTable);

        public DelegateCommand EditDebtor => editDebtor ??=
            new(() => { pageService.NavigateCommand.Execute(new DebtorEditView(SelectedDebtor.Id)); },
                () => SelectedDebtor != null);

        public DelegateCommand DeleteDebtor => deleteDebtor ??= new(() =>
        {
            if (MessageBox.Show(
                $"Вы уверены, что хотите удалить должника {SelectedDebtor.FullName} ({SelectedDebtor.ContractNumber})?",
                "Удаление должника", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DebtorsContext db = new();
                Debtor deletingDebtor = db.Debtors.Single(x => x.Id == SelectedDebtor.Id);
                db.Debtors.Remove(deletingDebtor);
                db.SaveChanges();
                UpdateDebtors();
            }
        }, () => SelectedDebtor != null && session.CanEditDebtorsTable);

        public DelegateCommand ExportCurrentDebtorsCollection =>
            exportCurrentDebtorsCollection ??= new(() =>
            {
                SaveFileDialog saveFileDialog = new()
                {
                    AddExtension = true,
                    CheckPathExists = true,
                    FileName = $"Экспорт должников от {DateTime.Now:dd MM yyyy HH mm ss}",
                    DefaultExt = "*.xlsx"
                };
                if (!saveFileDialog.ShowDialog().Value) return;
                string saveFileName = saveFileDialog.FileName;
                ExcelPackage excel = new();
                ExcelWorksheet sheet = excel.Workbook.Worksheets.Add("Выгрузка");

                sheet.Cells[1, 1].Value = "Номер КД";
                sheet.Cells[1, 2].Value = "ФИО";
                sheet.Cells[1, 3].Value = "Начальный долг";

                for (int i = 0; i < Debtors.Length; i++)
                {
                    sheet.Cells[i + 2, 1].Value = Debtors[i].ContractNumber;
                    sheet.Cells[i + 2, 2].Value = Debtors[i].FullName;
                    sheet.Cells[i + 2, 3].Value = Debtors[i].StartDebt;

                }

                excel.SaveAs(new FileInfo(saveFileName));
            }, () => Debtors != null && Debtors.Any());

        [Reactive] public Debtor[] Debtors { get; set; } = new Debtor[0];
        [Reactive] public Debtor SelectedDebtor { get; set; }
        [Reactive] public string SearchText { get; set; }
        public DelegateCommand Search => search ??= new(UpdateDebtors);

        private IQueryable<Debtor> SetUserFilter(IQueryable<Debtor> source)
        {
            if (!session.CanViewNotOwnedDebtors)
                return source.Where(x => x.Responsible.Id == session.UserId);
            return source;
        }

        private IQueryable<Debtor> SetSearchFilter(IQueryable<Debtor> source)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return source;
            return source.Where(x => x.ContractNumber.ToLower().Contains(SearchText.ToLower()));
        }

        public void UpdateDebtors()
        {
            DebtorsContext model = new();
            Debtors = SetSearchFilter(SetUserFilter(model.Debtors))
                .Include(x => x.Responsible)
                .Include(x => x.Payments)
                .ToArray();
        }
    }
}