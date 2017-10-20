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
using System.Windows.Shapes;
using NetLib.Errors.UI.ViewModel;

namespace NetLib.Errors.UI.View
{
    /// <summary>
    /// Interaction logic for ErrorsView.xaml
    /// </summary>
    public partial class ErrorsView
    {
        public ErrorsView(ErrorsViewModel errorsVM)
        {
            InitializeComponent();
            DataContext = errorsVM;
        }
    }
}