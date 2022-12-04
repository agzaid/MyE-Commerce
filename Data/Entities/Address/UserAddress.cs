using Data.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Address
{
    public class UserAddress
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }
        public bool IsDefault { get; set; }
    }
}
