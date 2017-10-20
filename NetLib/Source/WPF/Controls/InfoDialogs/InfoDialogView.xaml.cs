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

namespace NetLib.WPF.Controls.InfoDialogs
{
    /// <summary>
    /// Interaction logic for InfoDialogView.xaml
    /// </summary>
    internal partial class InfoDialogView
    {
        public InfoDialogView(InfoDialogViewModel infoDlgVM)
        {
            InitializeComponent();
            DataContext = infoDlgVM;
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}