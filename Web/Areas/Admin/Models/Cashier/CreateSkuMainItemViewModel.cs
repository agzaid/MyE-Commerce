using Data.Entities.Cashier;
using Data.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Admin.Models.Cashier
{
    public class CreateSkuMainItemViewModel
    {
        public CreateSkuMainItemViewModel()
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
        [Required(ErrorMessage ="Please Enter Name...")]
        public string Name { get; set; }
        [Display(Name = "Barcode serial")]
        public string BarCodeNumber { get; set; }
        public double? Price { get; set; }
        public int Quantity{ get; set; }
        public string ThumbnailImage { get; set; }
        public IFormFile ThumbnailFormFile { get; set; }
        public string ShortDescription { get; set; }
        public RecordStatus Status { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem> ListOfStatus { get; set; } = new();
        public List<SelectListItem> ListOfCategories { get; set; } = new();
        public List<SkuSubItemViewModel> ListSkuSubItems { get; set; } = new();



    }
}
