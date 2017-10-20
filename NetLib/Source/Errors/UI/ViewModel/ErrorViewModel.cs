﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.IconPacks;
using MicroMvvm;

namespace NetLib.Errors.UI.ViewModel
{
    public class ErrorViewModel : ViewModelBase
    {
        private readonly IError error;

        public ErrorViewModel(IError error)
        {
            this.error = error;
            Message = error.Message;
            Icon = GetIcon(error);
        }

        public string Message { get; set; }
        public Control Icon { get; set; }

        private static Control GetIcon(IError error)
        {
            switch (error.Level)
            {
                case ErrorLevel.Error:
                    return new PackIconOcticons
                    {
                        Kind = PackIconOcticonsKind.Stop,
                        Background = Brushes.Red
                    };
                case ErrorLevel.Info:
                    return new PackIconOcticons
                    {
                        Kind = PackIconOcticonsKind.Info,
                        Background = Brushes.DeepSkyBlue
                    };
                case ErrorLevel.Exclamation:
                    return new PackIconOcticons
                    {
                        Kind = PackIconOcticonsKind.Alert,
                        Background = Brushes.Yellow
                    };
                default: return null;
            }
        }
    }
}