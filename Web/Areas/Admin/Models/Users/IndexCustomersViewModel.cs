using Data.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Web.Areas.Admin.Models.Users
{
    public class IndexCustomersViewModel
    {
        public IndexCustomersViewModel()
        {
            ListOfStatus = Enum.GetNames(typeof(RecordStatus))
                .Select(v => new SelectListItem
                {
                    Text = v,
                    Value = v
                }).ToList();
            ListOfStatus.Insert(0, new SelectListItem
            {
                Value = String.Empty,
                Text = "--------------"
            });
        }
        public int ID { get; set; }
        [Display(Name = "Customer Name")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public string City { get; set; }

        [Display(Name = "First Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "First Phone Number")]
        public string SecondPhoneNumber { get; set; }
        public int OrdersRequested { get; set; }
        public int CashCollected { get; set; }
        public RecordStatus Status { get; set; }
        public List<SelectListItem> ListOfStatus { get; set; } = new();

    }
}
