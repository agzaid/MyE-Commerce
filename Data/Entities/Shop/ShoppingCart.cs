using Data.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Shop
{
    public class ShoppingCart : BaseEntity
    {

        #region Navigation Property
        public AppUser AppUser { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = new();

        #endregion
    }
}
