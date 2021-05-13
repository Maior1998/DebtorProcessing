﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DebtorProcessing.ViewModel;

namespace DebtorProcessing.View
{
    /// <summary>
    /// Interaction logic for DebtorEditView.xaml
    /// </summary>
    public partial class DebtorEditView : Page
    {
        public DebtorEditView(Guid debtorId)
        {
            InitializeComponent();
            ((DebtorsEditViewModel)DataContext).SetDebtor(debtorId);
        }
    }
}
