using JetBrains.Annotations;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace NetLib.Excel
{
    public static class ExcelExt
    {
        public static void SetColor([NotNull] this ExcelRange cell, Color color)
        {
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(color);
        }

        public static void SetStyleCenterAlignment([NotNull] this ExcelStyle style)
        {
            style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }

        public static void SetBorderByGost(this ExcelStyle style, ExcelBorderStyle vertic = ExcelBorderStyle.Medium,
            ExcelBorderStyle hor = ExcelBorderStyle.Thin)
        {
            try
            {
                style.Border.Bottom.Style = hor;
                style.Border.Top.Style = hor;
                style.Border.Left.Style = vertic;
                style.Border.Right.Style = vertic;
                style.Border.BorderAround(vertic);
            }
            catch
            {
                //
            }
        }

        public static void SetCellCenter([NotNull] this ExcelRange cell)
        {
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }
    }
}
