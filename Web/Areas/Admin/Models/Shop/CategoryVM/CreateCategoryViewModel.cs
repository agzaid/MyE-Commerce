using Data.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Admin.Models.Shop.CategoryVM
{
    public class CreateCategoryViewModel
    {
        public CreateCategoryViewModel()
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
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
        public int DisplayOrder { get; set; }
        public RecordStatus Status { get; set; }
        public string ThumbnailImage { get; set; }
        

        public IFormFile ThumbnailFormFile { get; set; }

        public List<SelectListItem> ListOfStatus { get; set; } = new();

    }
}
