using Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Shop
{
    public class Category : BaseEntity
    {
        //public string ParentId { get; set; }
        public string Name { get; set; }
        public string ThumbnailImage { get; set; }
        [Display(Name="Display Order")]
        public int DisplayOrder { get; set; }
        public RecordStatus Status { get; set; }

        #region navigation properties
        public List<Product> Products { get; set; }
        // public IEnumerable<Localization> Localizations { get; set; }

        #endregion
    }
}
