using Microsoft.Win32;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using PhotoStudioApp.Views;
using PhotoStudioApp.Database.DBContext;
using System.ComponentModel;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace PhotoStudioApp.Helper
{
    public static class CreateFile
    {
        public static void CreatePdfReceipt(Customer customer, Services service, AdditionalService additionalService, Booking booking, Hall hall)
        {
            string fullName = $"{customer.SecondName} {customer.Name} {customer.LastName}";
            double totalCost = booking.CostServices;
            DateTime date = booking.DateBooking;

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                FileName = $"Чек {booking.DateBooking.Day}_{booking.DateBooking.Month}_{booking.DateBooking.Year}.pdf",
                Filter = "PDF файлы (*.pdf)|*.pdf"
            };

            if (saveDialog.ShowDialog() == true)
            {
                additionalService ??= new AdditionalService
                {
                    ServiceName = "---",
                    Cost = 0,
                    BonusCost = 0
                };

                hall ??= new Hall
                {
                    Description = "---"
                };

                PdfDocument document = new PdfDocument();
                document.Info.Title = "Квитанция оплаты";

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Шрифты
                XFont titleFont = new XFont("Verdana", 16);
                XFont labelFont = new XFont("Verdana", 12);
                XFont valueFont = new XFont("Verdana", 12);
                XFont totalFont = new XFont("Verdana", 14);

                int y = 40;
                int lineHeight = 25;

                void DrawLine() => gfx.DrawLine(XPens.Black, 40, y-10, page.Width - 40, y-10);

                // Заголовок
                gfx.DrawString("Фотостудия «Улыбка»", titleFont, XBrushes.Black, new XRect(0, y, page.Width, 30), XStringFormats.TopCenter);
                y += 50;

                // Информация о заказе
                gfx.DrawString("Номер брони:", labelFont, XBrushes.Black, new XPoint(50, y));
                gfx.DrawString($"№{booking.ID}", valueFont, XBrushes.Black, new XPoint(200, y));
                y += lineHeight;

                gfx.DrawString("Клиент:", labelFont, XBrushes.Black, new XPoint(50, y));
                gfx.DrawString(fullName, valueFont, XBrushes.Black, new XPoint(200, y));
                y += lineHeight;

                gfx.DrawString("Дата бронирования:", labelFont, XBrushes.Black, new XPoint(50, y));
                gfx.DrawString($"{date:dd.MM.yyyy}", valueFont, XBrushes.Black, new XPoint(200, y));
                y += lineHeight * 2;

                // Основная услуга
                DrawLine(); y += 10;
                gfx.DrawString("Основная услуга", titleFont, XBrushes.Black, new XPoint(50, y));
                y += lineHeight;

                gfx.DrawString("Название:", labelFont, XBrushes.Black, new XPoint(50, y));
                gfx.DrawString(service.ServiceName, valueFont, XBrushes.Black, new XPoint(200, y));
                y += lineHeight;

                gfx.DrawString("Стоимость:", labelFont, XBrushes.Black, new XPoint(50, y));
                gfx.DrawString($"{service.CostService} руб.", valueFont, XBrushes.Black, new XPoint(200, y));
                y += lineHeight * 2;

                // Дополнительная услуга
                DrawLine(); y += 10;
                gfx.DrawString("Дополнительная услуга", titleFont, XBrushes.Black, new XPoint(50, y));
                y += lineHeight;

                gfx.DrawString("Название:", labelFont, XBrushes.Black, new XPoint(50, y));
                gfx.DrawString(additionalService.ServiceName, valueFont, XBrushes.Black, new XPoint(200, y));
                y += lineHeight;

                gfx.DrawString("Стоимость:", labelFont, XBrushes.Black, new XPoint(50, y));
                gfx.DrawString($"{additionalService.Cost} руб.", valueFont, XBrushes.Black, new XPoint(200, y));
                y += lineHeight * 2;

                // Зал
                DrawLine(); y += 10;
                gfx.DrawString("Зал", titleFont, XBrushes.Black, new XPoint(50, y));
                y += lineHeight;

                gfx.DrawString("Описание:", labelFont, XBrushes.Black, new XPoint(50, y));
                gfx.DrawString(hall.Description, valueFont, XBrushes.Black, new XPoint(200, y));
                y += lineHeight * 2;

                // Итого
                DrawLine(); y += 10;
                gfx.DrawString("Итого к оплате:", labelFont, XBrushes.Black, new XPoint(50, y));
                gfx.DrawString($"{totalCost} руб.", totalFont, XBrushes.Black, new XPoint(200, y));
                y += lineHeight * 2;

                gfx.DrawString("Спасибо за бронирование!", titleFont, XBrushes.Black, new XRect(0, y, page.Width, 30), XStringFormats.TopCenter);

                // Сохранение и запуск
                document.Save(saveDialog.FileName);
                Process.Start(new ProcessStartInfo(saveDialog.FileName) { UseShellExecute = true });
            }
        }

        public static void CreateExcelTable(List<Booking> bookingList)
        {

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                FileName = $"Отчет от {DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}.xlsx",
                Filter = "Excel файлы (*.xlsx)|*.xlsx"
            };

            ExcelPackage.License.SetNonCommercialOrganization("Tanya");
            if (saveDialog.ShowDialog() == true)
            {
                using (ExcelPackage package = new ExcelPackage())
                using (var context = new MyDBContext())
                {
                    
                    var worksheet = package.Workbook.Worksheets.Add("Бронирования");

                    // Заголовки
                    worksheet.Cells[1, 1].Value = "ID";
                    worksheet.Cells[1, 2].Value = "Клиент";
                    worksheet.Cells[1, 3].Value = "Основная услуга";
                    worksheet.Cells[1, 4].Value = "Доп. услуга";
                    worksheet.Cells[1, 5].Value = "Зал";
                    worksheet.Cells[1, 6].Value = "Дата";
                    worksheet.Cells[1, 7].Value = "Сумма";

                    int row = 2;
                    foreach (var booking in bookingList)
                    {
                        var customer = context.Customers.Find(booking.CustomerID);
                        var service = context.Services.Find(booking.ServiceID);
                        var addService = booking.AdditionalServicesID.HasValue
                            ? context.AdditionalServices.Find(booking.AdditionalServicesID.Value)
                            : null;

                        var hall = context.Halls.Find(booking.HallID);

                        worksheet.Cells[row, 1].Value = booking.ID;
                        worksheet.Cells[row, 2].Value = $"{customer.SecondName} {customer.Name} {customer.LastName}";
                        worksheet.Cells[row, 3].Value = service?.ServiceName ?? "---";
                        worksheet.Cells[row, 4].Value = addService?.ServiceName ?? "---";
                        worksheet.Cells[row, 5].Value = hall.Description;
                        worksheet.Cells[row, 6].Value = booking.DateBooking.ToString("dd.MM.yyyy");
                        worksheet.Cells[row, 7].Value = booking.CostServices;

                        row++;
                    }

                    // Стили
                    using (var range = worksheet.Cells[1, 1, 1, 7])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    }

                    worksheet.Cells.AutoFitColumns();
                    try
                    {
                        File.WriteAllBytes(saveDialog.FileName, package.GetAsByteArray());
                    }
                    catch(Exception ex)
                    {
                        Message.Warning("Закройте пожалуйста файл с отчетом");
                        return;
                    }

                    Process.Start(new ProcessStartInfo(saveDialog.FileName) { UseShellExecute = true });
                }
            }
        }
    }
}
