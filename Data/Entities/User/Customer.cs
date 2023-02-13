using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.User
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string SecondPhoneNumber { get; set; }
        public int OrdersRequested { get; set; }
        public int CashCollected { get; set; }

        #region Navigation Properties
        // public virtual List<Order> Orders { get; set; } = new();
        // public string AppUserId { get; set; }
        // public AppUser AppUser { get; set; }
        #endregion
    }

}
