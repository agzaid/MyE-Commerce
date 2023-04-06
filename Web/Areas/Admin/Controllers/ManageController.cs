using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManageController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ManageController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
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
            string mimetype = "";
            int extension = 1;
            var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Report1.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("rp1", "welcome to Asian Store");
            LocalReport localReport = new LocalReport(path);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);
            return File(result.MainStream, "application/pdf");
        }

        public DataTable GetItemList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ItemId");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("ItemQty");
            dt.Columns.Add("Price");
            DataRow row;
            for (int i = 0; i < 2; i++)
            {
                row = dt.NewRow();
                row["itemId"] = i;
                row["itemName"] = "Laptop" + i;
                row["itemQty"] = 1;
                row["Price"] = 50000;

                dt.Rows.Add(row);
            }
            return dt;
        }

        #endregion
    }
}
