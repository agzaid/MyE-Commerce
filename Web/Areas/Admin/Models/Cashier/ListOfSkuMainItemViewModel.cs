using Data.Entities.Enums;
using Data.Entities.Shop;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Web.Areas.Admin.Models.Cashier
{
    public class ListOfSkuMainItemViewModel
    {
        public ListOfSkuMainItemViewModel()
        {
            ListOfStatus = Enum.GetNames(typeof(SkuItemStatus))
                .Select(v => new SelectListItem
                {
                    Text = v,
                    Value = v
                }).ToList();
            ListOfStatus.Insert(0, new SelectListItem
            {
                Value = string.Empty,
                Text = "--------------"
            });
        }
        public int ID { get; set; }
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "BarCode Serial Number")]
        //[Unique(ErrorMessage = "This item already exists !!")]
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public string ThumbnailImage { get; set; }
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }
        public SkuItemStatus Status { get; set; }
        public IFormFile ThumbnailFormFile { get; set; }
        public List<SelectListItem> ListOfStatus { get; set; } = new();
        //public List<IFormFile> GalleryFormFiles { get; set; } = new();

        //public List<string> RawGalleyImages { get; set; } = new();
        //public List<string> GalleyImages { get; set; } = new();
        //public List<string> OldGalleyImages { get; set; } = new();
        //public List<string> NewGalleyImages { get; set; } = new();
        //public List<string> DeleteGalleyImages { get; set; } = new();


        //public Category Category { get; set; }
        //public int CategoryId { get; set; }

    }
}
