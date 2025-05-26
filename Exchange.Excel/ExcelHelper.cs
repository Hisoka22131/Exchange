using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Exchange.Excel;

public static class ExcelHelper
{
    public static byte[] CreateExcelFile<T>(IEnumerable<T> entities)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        using var package = new ExcelPackage();

        var worksheet = package.Workbook.Worksheets.Add("Data");

        // Получаем свойства сущности
        var properties = typeof(T).GetProperties();

        // Заполняем заголовки колонок
        for (int col = 0; col < properties.Length; col++)
        {
            worksheet.Cells[1, col + 1].Value = properties[col].Name;
            worksheet.Cells[1, col + 1].Style.Font.Bold = true;
            worksheet.Cells[1, col + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[1, col + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[1, col + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }

        int row = 2;
        foreach (var entity in entities)
        {
            for (int col = 0; col < properties.Length; col++)
            {
                var value = properties[col].GetValue(entity);
                worksheet.Cells[row, col + 1].Value = value;
                worksheet.Cells[row, col + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            row++;
        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        return package.GetAsByteArray();
    }
}