using Data.Entities.Cashier;
using Data.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;

namespace Web.Areas.Admin.Models.Cashier
{
    public class EditSkuSubItemViewModel
    {
        public EditSkuSubItemViewModel()
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
        public int ID { get; set; }
        public string Name { get; set; }
        [Required]
        public string BarCodeNumber { get; set; }
        [Required]
        public double? Price { get; set; }
        [Required]
        public string ExpiryDate { get; set; }
        public string ThumbnailImage { get; set; }
        public SkuItemStatus Status { get; set; }
        public List<SelectListItem> ListOfStatus { get; set; } = new();

        #region Navigation properties
        public SkuMainItem SkuMainItem { get; set; }
        public int? SkuMainItemId { get; set; }
        #endregion
    }
}
