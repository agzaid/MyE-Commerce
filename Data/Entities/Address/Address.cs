using Data.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Address
{
    public class Address : BaseEntity
    {
        public int UnitNumber { get; set; }
        public int StreetNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public int PostalCode { get; set; }

        #region Navigation property
        public List<UserAddress> UsersAddresses { get; set; } = new();
        public int CountryId { get; set; }
        public Country Country { get; set; }
        #endregion
    }
}
