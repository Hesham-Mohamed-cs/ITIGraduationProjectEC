using DinkToPdf.Contracts;
using GraduaitionProjectITI.Models.Context;
using GraduaitionProjectITI.Reprository.BrandRepository;
using GraduaitionProjectITI.Reprository.CategoryReprositry;
using GraduaitionProjectITI.Reprository.OrderReprository;
using GraduaitionProjectITI.Reprository.ProductReprository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;
namespace GraduaitionProjectITI.Controllers
{
    public class ReportPDFController : Controller
    {
        private readonly ECcontext context;
        private readonly IConverter _pdfConverter;
        private readonly IProductReprository productReprository;
        private readonly ICategoryReprositry categoryReprositry;
        private readonly IBrandRepository brandRepository;
        private readonly IOrderReprository orderReprository;
        public ReportPDFController(IConverter pdfConverter, ECcontext context, IProductReprository productReprository,
            ICategoryReprositry categoryReprositry, IBrandRepository brandRepository
            , IOrderReprository orderReprository)
        {
            this.context = context;
            _pdfConverter = pdfConverter;
            this.productReprository = productReprository;
            this.categoryReprositry = categoryReprositry;
            this.brandRepository = brandRepository;
            this.orderReprository = orderReprository;
        }
        [HttpGet]
        public async Task<IActionResult> GeneratePDF(string InvoiceNo)
        {
            ViewBag.productCount = productReprository.GetAll().Count();
            ViewBag.CategoryCount = categoryReprositry.GetAll().Count();
            ViewBag.BrandCount = brandRepository.GetAll().Count();
            ViewBag.orderCount = orderReprository.GetAll().Count();

            ViewBag.monthCost = context.orders.Where(c=>c.ResevationDate.Month== DateTime.Now.Month).Sum(c=>c.Cost);
            ViewBag.yearCost = context.orders.Where(c=>c.ResevationDate.Year== DateTime.Now.Year).Sum(c=>c.Cost);
            ViewBag.monthOrders = context.orders.Where(c=>c.ResevationDate.Month== DateTime.Now.Month).Count();
            ViewBag.yearOrders = context.orders.Where(c=>c.ResevationDate.Year == DateTime.Now.Year).Count();
            //return View(orderreport);
            var htmlContent = await this.RenderViewToStringAsync("GeneratePDF", null);
            var document = new PdfDocument();
            //string HtmlContent = "<h1>fgdhdhd</h1>";
            PdfGenerator.AddPdfPages(document, htmlContent, PageSize.A4);
            byte[] response;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            string FileName = "Sales Report" + InvoiceNo + ".pdf";
            return File(response, "application/pdf", FileName);
        }
        public async Task<string> RenderViewToStringAsync(string viewName, object model)
        {

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                var viewEngineResult = HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

                var viewContext = new ViewContext(
                    ControllerContext,
                    viewEngineResult.FindView(ControllerContext, viewName, true).View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewContext.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }

        }
    }
}
