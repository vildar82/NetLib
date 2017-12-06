﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.XmlAccess;

namespace NetLib.Excel
{
    public abstract class ExportExcelBase
    {
        protected abstract string DefFileName { get; set; }
        protected abstract List<string> SheetNames { get; set; }

        public void Export()
        {
            var tempFile = IO.Path.GetTempFile(".xlsx");
            try
            {
                // Запись файла
                using (var xlPackage = new ExcelPackage(new FileInfo(tempFile)))
                {
                    CreateExcelPackage(xlPackage);
                    xlPackage.Save();
                }

                // Диалог выбора места сохранения файла
                var dlg = new SaveFileDialog
                {
                    InitialDirectory = Path.GetDirectoryName(DefFileName),
                    FileName = DefFileName,
                    DefaultExt = ".xlsx",
                    AddExtension = true,
                    OverwritePrompt = true
                };
                if (dlg.ShowDialog() != true)
                {
                    throw new OperationCanceledException();
                }
                File.Copy(tempFile, dlg.FileName, true);
                Process.Start(dlg.FileName);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        protected virtual void CreateExcelPackage(ExcelPackage xlPackage)
        {
            foreach (var name in SheetNames)
            {
                var ws = xlPackage.Workbook.Worksheets.Add(name);
                FillSheet(name, ws);
            }
        }

        protected abstract void FillSheet(string name, ExcelWorksheet wb);

        public static ExcelNamedStyleXml AddStyle(ExcelWorkbook wb, string name,
            ExcelBorderStyle borderStyle = ExcelBorderStyle.Medium, 
            System.Drawing.Color? bckColor = null, int size = 11, bool bold = false,
            bool borderByGost = true, bool center = true)
        {
            var style = wb.Styles.CreateNamedStyle(name);
            style.Style.Font.Name = "Arial";
            style.Style.Font.Size = size;
            style.Style.WrapText = true;
            style.Style.Font.Bold = bold;
            if (center)
            {
                style.Style.SetStyleCenterAlignment();
            }
            if (borderByGost)
            {
                style.Style.SetBorderByGost(vertic:borderStyle);
            }
            else
            {
                style.Style.SetBorderByGost(vertic: borderStyle, hor:borderStyle);
            }
            if (bckColor != null)
            {
                style.Style.Fill.PatternType = ExcelFillStyle.Solid;
                style.Style.Fill.BackgroundColor.SetColor(bckColor.Value);
            }
            return style;
        }
    }
}