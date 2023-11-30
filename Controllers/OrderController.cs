using GraduaitionProjectITI.Reprository.OrderReprository;
using GraduaitionProjectITI.Reprository.SubOrderReprository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace GraduaitionProjectITI.Controllers
{
    [Authorize(Roles = "Admin")]

    public class OrderController : Controller
    {
        private readonly IOrderReprository reprositry;
        private readonly ISubOrderReprository reprositrySubOrder;
        public OrderController(IOrderReprository reprositry, ISubOrderReprository reprositrySubOrder)
        {
            this.reprositry = reprositry;
            this.reprositrySubOrder = reprositrySubOrder;
        }
        public IActionResult Index()
        {
            return View(this.reprositry.GetAll());
        }
        public IActionResult Details(int Id)
        {
            return View(this.reprositrySubOrder.GetSubOrderForOrder(Id));

        }


        public IActionResult GenerateReport()
        {
            var Products = reprositry.GetAll();
            ExcelPackage.LicenseContext = LicenseContext.Commercial; // For commercial licenses
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // For non-commercial licenses

            using (var package = new ExcelPackage())
            {

                var worksheet = package.Workbook.Worksheets.Add("Products");


                var headerStyle = worksheet.Cells[1, 1, 1, 7].Style;
                headerStyle.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerStyle.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                headerStyle.Font.Color.SetColor(System.Drawing.Color.White);

                worksheet.Cells[1, 1].Value = "Order ID";
                worksheet.Cells[1, 2].Value = "Reservation Date";
                worksheet.Cells[1, 3].Value = "Delivery Date";
                worksheet.Cells[1, 4].Value = "Cost";
                worksheet.Cells[1, 5].Value = "Destination";
                worksheet.Cells[1, 6].Value = "IsConfirmed";
                worksheet.Cells[1, 7].Value = "User";


                worksheet.Cells[worksheet.Dimension.Address].AutoFilter = true;


                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 20;
                worksheet.Column(4).Width = 15;
                worksheet.Column(5).Width = 12;
                worksheet.Column(6).Width = 20;
                worksheet.Column(7).Width = 20;


                int row = 2;
                foreach (var product in Products)
                {
                    worksheet.Cells[row, 1].Value = product.Id.ToString();
                    worksheet.Cells[row, 2].Value = product.ResevationDate.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 3].Value = product.DeliveryDate.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 4].Value = product.Cost;
                    worksheet.Cells[row, 5].Value = product.destination;
                    worksheet.Cells[row, 6].Value = product.IsConfirmed;
                    worksheet.Cells[row, 7].Value = product.User;
                    row++;
                }


                using (var cells = worksheet.Cells[2, 1, row - 1, 2])
                {
                    cells.Style.Numberformat.Format = "dd/MM/yyyy";
                }

                var excelBytes = package.GetAsByteArray();
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Orders Report.xlsx");
            }
        }
    }
}
