using Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Cashier
{
    public class SkuSubItem : BaseEntity
    {
        public string BarCodeNumber { get; set; }
        public double Price { get; set; }
        public DateTime ExiperyDate { get; set; }
        public SkuItemStatus Status { get; set; }

        #region Navigation properties
        public SkuMainItem SkuMainItem { get; set; }
        public int? SkuMainItemId { get; set; }
        #endregion

    }
}
