using Data.Entities.Cashier;
using Data.Entities.Enums;

namespace Web.Areas.Admin.Models.Cashier
{
    public class SkuSubItemViewModel
    {
        public int ID { get; set; }
        public string BarCodeNumber { get; set; }
        public double? Price { get; set; }
        public string ExpiryDate { get; set; }
        public SkuItemStatus Status { get; set; }

        #region Navigation properties
        public SkuMainItem SkuMainItem { get; set; }
        public int? SkuMainItemId { get; set; }
        #endregion
    }
}
