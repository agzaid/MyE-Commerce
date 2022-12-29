using Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Shop
{
    public class Product : BaseEntity
    {
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int DisplayOrder { get; set; }
        public int Quantity { get; set; }
        public RecordStatus Status { get; set; }
        public string Image { get; set; }
        public string ThumbnailImage { get; set; }
        public string ShortDescription { get; set; }

        #region Navigation Properties
        public Category Category { get; set; }
        public int? CategoryId { get; set; }
        public ShoppingCartItem ShoppingCartItem { get; set; }

        //public OrderDetails OrderDetails { get; set; }
        //public int? OrderDetailsId { get; set; }
        #endregion
    }
}
