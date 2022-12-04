using Data.Entities.Enums;
using Data.Entities.Shop;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Admin.Models.Shop
{
    public class CreateProductViewModel
    {
        public CreateProductViewModel()
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
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int DisplayOrder { get; set; }
        public int Quantity { get; set; }
        public RecordStatus Status { get; set; }
        public string Image { get; set; }
        public string ThumbnailImage { get; set; }
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }

        public IFormFile ThumbnailFormFile { get; set; }
        public List<IFormFile> GalleryFormFiles { get; set; } = new();
        public List<string> RawGalleyImages { get; set; } = new();
        public List<string> GalleyImages { get; set; } = new();
        public List<string> OldGalleyImages { get; set; } = new();
        public List<string> NewGalleyImages { get; set; } = new();
        public List<string> DeleteGalleyImages { get; set; } = new();

        public List<SelectListItem> ListOfStatus { get; set; } = new();
        public List<SelectListItem> ListOfCategories { get; set; } = new();

        public Category Category { get; set; }
        public int CategoryId { get; set; }

    }
}
