using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using DebtorProcessing.Model;
using DebtorProcessing.Services;
using DebtorProcessing.View;

using DebtorsDbModel;
using DebtorsDbModel.Model;

using DevExpress.Mvvm;

using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

using OfficeOpenXml;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorProcessing.ViewModel
{
    public class DebtorsTableViewModel : ReactiveObject
    {
        private SessionService session;
        private PageService pageService;
        public DebtorsTableViewModel(SessionService session, PageService pageService)
        {
            this.session = session;
            this.pageService = pageService;
            UpdateDebtors();
        }

        public void UpdateDebtors()
        {
            Context model = new();
            if (string.IsNullOrWhiteSpace(SearchText))
                Debtors = model.Debtors
                    .Include(x => x.Responsible)
                    .Include(x => x.Payments)
                    .ToArray();
            else
                Debtors = model.Debtors
                    .Include(x => x.Responsible)
                    .Include(x => x.Payments)
                    .Where(x => x.ContractNumber.ToLower().Contains(SearchText.ToLower())).ToArray();
        }

        private DelegateCommand addDebtor;
        public DelegateCommand AddDebtor => addDebtor ??= new(() =>
        {
            Context context = new();
            Debtor addingDebtor = DatabaseHelperFunctions.GetExampleDebtor();
            User currentLoggedInUser = context.Users.Single(x => x.Id == session.CurrentLoggedInUser.Id);
            currentLoggedInUser.Debtors.Add(addingDebtor);

            context.Debtors.Add(addingDebtor);
            context.SaveChanges();
            UpdateDebtors();
            SelectedDebtor = Debtors.Single(x => x.Id == addingDebtor.Id);
            EditDebtor.Execute(null);
        });

        private DelegateCommand editDebtor;
        public DelegateCommand EditDebtor => editDebtor ??= new(() =>
        {
            pageService.NavigateCommand.Execute(new DebtorEditView(SelectedDebtor.Id));
        }, () => SelectedDebtor != null);

        private DelegateCommand deleteDebtor;
        public DelegateCommand DeleteDebtor => deleteDebtor ??= new(() =>
        {
            if (MessageBox.Show(
                $"Вы уверены, что хотите удалить должника {SelectedDebtor.FullName} ({SelectedDebtor.ContractNumber})?",
                "Удаление должника", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Context db = new();
                Debtor deletingDebtor = db.Debtors.Single(x => x.Id == SelectedDebtor.Id);
                db.Debtors.Remove(deletingDebtor);
                db.SaveChanges();
                UpdateDebtors();
            }
        }, () => SelectedDebtor != null);

        private DelegateCommand exportCurrentDebtorsCollection;

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
                sheet.Cells[1, 3].Value = "Адрес регистрации";
                sheet.Cells[1, 4].Value = "Начальный долг";
                sheet.Cells[1, 5].Value = "Текущий долг";

                for (int i = 0; i < Debtors.Length; i++)
                {
                    sheet.Cells[i + 2, 1].Value = Debtors[i].ContractNumber;
                    sheet.Cells[i + 2, 2].Value = Debtors[i].FullName;
                    sheet.Cells[i + 2, 3].Value = Debtors[i].RegistrationAddress;
                    sheet.Cells[i + 2, 4].Value = Debtors[i].StartDebt;
                    sheet.Cells[i + 2, 5].Value = Debtors[i].CurrentDebt;
                }

                excel.SaveAs(new FileInfo(saveFileName));
            }, () => Debtors != null && Debtors.Any());

        [Reactive] public Debtor[] Debtors { get; set; } = new Debtor[0];
        [Reactive] public Debtor SelectedDebtor { get; set; }
        [Reactive] public string SearchText { get; set; }

        private DelegateCommand search;
        public DelegateCommand Search => search ??= new(UpdateDebtors);
    }
}
