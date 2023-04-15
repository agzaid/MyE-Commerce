using Data.Entities.Cashier;
using Data.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Repo.Migrations;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Admin.Models.Cashier
{
    public class CreateSkuMainItemViewModel
    {
        public CreateSkuMainItemViewModel()
        {
            ListOfStatus = Enum.GetNames(typeof(SkuItemStatus))
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
        public string Name { get; set; }
        [Display(Name = "Short Description")]
        public string BarCodeNumber { get; set; }
        public double Price { get; set; }
        public int Quantity{ get; set; }
        public string ThumbnailImage { get; set; }
        public IFormFile ThumbnailFormFile { get; set; }
        public string ShortDescription { get; set; }
        public SkuItemStatus Status { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem> ListOfStatus { get; set; } = new();
        public List<SelectListItem> ListOfCategories { get; set; } = new();
        public List<SkuSubItem> ListSkuSubItems { get; set; } = new();



    }
}
