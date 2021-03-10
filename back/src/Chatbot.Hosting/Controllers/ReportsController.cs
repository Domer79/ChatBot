using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Common;
using Chatbot.Hosting.Authentication;
using Chatbot.Model.Enums;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Chatbot.Hosting.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [CustomSecurity(SecurityPolicy.ReportsPage)]
    public class ReportsController: ChatControllerBase
    {
        private readonly IMessageDialogService _dialogService;

        public ReportsController(IMessageDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReport(DateTime start, DateTime end)
        {
            var dialogs = await _dialogService.GetDialogsByPeriod(start, end);
            var utcOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
            using var xlPackage = new ExcelPackage();
            var workSheetName = $"{start.Add(utcOffset).ToShortDateString()}-" +
                       $"{end.Add(utcOffset).ToShortDateString()}";
            var worksheet = xlPackage.Workbook.Worksheets.Add(workSheetName);
            worksheet.Cells[1, 1].Value = "№ Диалога";
            worksheet.Cells[1, 2].Value = "Статус";
            worksheet.Cells[1, 3].Value = "Дата открытия";
            worksheet.Cells[1, 4].Value = "Дата закрытия";
            worksheet.Cells[1, 5].Value = "Оператор";
            worksheet.Cells[1, 6].Value = "Клиент";
            using (ExcelRange r = worksheet.Cells["A1:F1"])
            {
                r.Style.Font.Bold = true;
                r.Style.Font.Size = 12;
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            }

            var row = 2;
            foreach (var dialog in dialogs)
            {
                worksheet.Column(1).Width = 10;
                worksheet.Column(2).Width = 11;
                worksheet.Column(3).Width = 14;
                worksheet.Column(4).Width = 14;
                worksheet.Column(5).Width = 18;
                worksheet.Column(6).Width = 18;
                
                worksheet.Cells[row, 1].Value = dialog.Number;
                worksheet.Cells[row, 2].Value = dialog.DialogStatus.GetDescription();
                worksheet.Cells[row, 3].Value =
                    dialog.DateCreated.Date.Add(utcOffset).ToString("d", CultureInfo.CurrentCulture);
                worksheet.Cells[row, 4].Value =
                    dialog.DateCompleted?.Add(utcOffset).ToString("d", CultureInfo.CurrentCulture) ?? "";
                worksheet.Cells[row, 5].Value = dialog.Operator?.Fio ?? "Отсутствует";
                worksheet.Cells[row, 6].Value = dialog.Client?.Fio ?? "Отсутствует";
                row++;
            }

            worksheet.Cells[$"A1:F{dialogs.Length+1}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[$"A1:F{dialogs.Length+1}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[$"A1:F{dialogs.Length+1}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[$"A1:F{dialogs.Length+1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            return File(xlPackage.GetAsByteArray(), "application/octet-stream", $"Report_{workSheetName}.xlsx");
        }
    }
}