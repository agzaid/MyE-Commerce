using Data.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Cashier
{
    public interface IInvoiceService
    {
        IEnumerable<Invoice> GetMany(Expression<Func<Invoice, bool>> expression, List<string> references);
        Task<Invoice> GetOne(Expression<Func<Invoice, bool>> expression, List<string> references);
        void Insert(Invoice invoice);
        void Update(Invoice invoice);
        Task Delete(int id);
    }
}
