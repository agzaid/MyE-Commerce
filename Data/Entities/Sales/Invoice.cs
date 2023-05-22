using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Sales
{
    public class Invoice : BaseEntity
    {
        public int InvoiceNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string PhoneNo { get; set; }
        //public string PaymentMethod { get; set; } = "Cash";
        public double? Total { get; set; }
        public int? TotalQuantity { get; set; }
        public double? SubTotal { get; set; }
        public double? Tax { get; set; }
        public byte[] BarcodeByte { get; set; }

        public List<InvoiceItems> InvoiceItems { get; set; } = new List<InvoiceItems>();

    }
}
