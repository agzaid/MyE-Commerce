using Data.Entities.Enums;
using Data.Entities.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Cashier
{
    public class SkuMainItem : BaseEntity
    {
        public string Name { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public string ThumbnailImage { get; set; }
        public string ShortDescription { get; set; }
        public RecordStatus Status { get; set; }

        #region Navigation Properties
        public List<SkuSubItem> skuSubItems { get; set; } = new();
        public Category Category { get; set; }
        public int? CategoryId { get; set; }

        #endregion

    }
}