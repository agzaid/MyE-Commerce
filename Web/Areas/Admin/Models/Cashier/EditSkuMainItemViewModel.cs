using Data.Entities.Cashier;
using Data.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Web.Areas.Admin.Models.Cashier
{
    public class EditSkuMainItemViewModel
    {
        public EditSkuMainItemViewModel()
        {
            ListOfStatus = Enum.GetNames(typeof(RecordStatus))
              .Select(v => new SelectListItem
              {
                  Text = v,
                  Value = v
              }).ToList();
            ListOfStatus.Insert(0, new SelectListItem
            {
                Value = String.Empty,
                Text = "--------------"
            });
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Name...")]
        public string Name { get; set; }
        [Display(Name="Purchase Price")]
        public double? PurchasePrice { get; set; }
        public int? Quantity { get; set; }
        public string ThumbnailImage { get; set; }
        public IFormFile ThumbnailFormFile { get; set; }
        public string ShortDescription { get; set; }
        public RecordStatus Status { get; set; }
        public int? CategoryId { get; set; }
        public List<SelectListItem> ListOfStatus { get; set; } = new();
        public List<SelectListItem> ListOfCategories { get; set; } = new();
        public List<SkuSubItemViewModel> ListSkuSubItems { get; set; } = new();
    }
}
