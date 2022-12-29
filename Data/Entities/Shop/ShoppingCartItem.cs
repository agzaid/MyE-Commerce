using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Shop
{
    public class ShoppingCartItem : BaseEntity
    {
        public int Qauantity { get; set; }

        
        #region Navigation Property
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart{ get; set; }

        #endregion
    }
}
