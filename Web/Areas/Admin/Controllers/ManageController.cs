using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Services.Shop;
using System.Data;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManageController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductService productService;

        public ManageController(IWebHostEnvironment webHostEnvironment, IProductService productService)
        {
            _webHostEnvironment = webHostEnvironment;
            this.productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Report 

        //public IActionResult EployeeSalaryInfo()
        //{

        //}
        public IActionResult PrintReceipt()
        {
            var dt= new DataTable();
            dt = GetProductsList();

            string mimetype = "";
            int extension = 1;
            var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Report1.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("rp1", "welcome to AG Store");
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("dsProducts", dt);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);
            return File(result.MainStream, "application/pdf");
        }

        public DataTable GetProductsList()
        {
            var products = productService.GetMany(s => true, null).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("ProductId");
            dt.Columns.Add("Name");
            dt.Columns.Add("Price");
            dt.Columns.Add("Quantity");
            DataRow row;
            for (int i = 0 ; i < products.Count; i++)
            {
                row = dt.NewRow();
                row["ProductId"] = products[i].ID;
                row["Name"] = "Mr. " + products[i].ProductName;
                row["Price"] = products[i].Price;
                row["Quantity"] = products[i].Quantity;

                dt.Rows.Add(row);
            }
            return dt;
        }

        #endregion
    }
}
