using Data.Entities.Enums;
using Data.Entities.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Cashier
{
    public class SkuProduct : BaseEntity
    {
        public string Name { get; set; }
        public string BarCodeNumber { get; set; }
        public double Price { get; set; }
        public string ThumbnailImage { get; set; }
        public string ShortDescription { get; set; }
        public SkuItemStatus Status { get; set; }

        #region Navigation Properties
        //public Category Category { get; set; }
        //public int? CategoryId { get; set; }

        #endregion

    }
}