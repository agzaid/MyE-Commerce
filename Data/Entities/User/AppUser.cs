using Data.Entities.Address;
using Data.Entities.Enums;
using Data.Entities.Shop;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.User
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string SecondPhoneNumber { get; set; }
        public string Image { get; set; }
        public bool IsDeleted { get; set; }

        #region Navigation Properties
        public List<UserAddress> UsersAddresses { get; set; } = new();
        //public List<ShoppingCart> ShoppingCarts { get; set; } = new();
        #endregion
    }
}
