using Data.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;

namespace Web.Areas.Admin.Models.Cashier
{
    public class CreateSkuMainItemViewModel
    {
        public CreateSkuMainItemViewModel()
        {
            ListOfStatus = Enum.GetNames(typeof(SkuItemStatus))
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
        [Required]
        public string Name { get; set; }
        [Required]
        public string BarCodeNumber { get; set; }
        [Required]
        public double Price { get; set; }
        public string ThumbnailImage { get; set; }
        public IFormFile ThumbnailFormFile { get; set; }
        public string ShortDescription { get; set; }
        public SkuItemStatus Status { get; set; }
        public List<SelectListItem> ListOfStatus { get; set; } = new();


    }
}
