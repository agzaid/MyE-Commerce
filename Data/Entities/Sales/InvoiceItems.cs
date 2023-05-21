using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Sales
{
    public class InvoiceItems : BaseEntity
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        #region Navigation properties
        public int InvoiceID { get; set; }
        public Invoice Invoice { get; set; }
        #endregion
    }
}
