using Data.Entities.Cashier;
using Data.Entities.Sales;
using Data.Entities.Shop;
using Repo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Cashier
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IRepository<Invoice> repository;

        public InvoiceService(IRepository<Invoice> repository)
        {
            this.repository = repository;
        }
        public async Task Delete(int id)
        {
            Invoice model = await GetOne(s => s.ID == id, null);

            await repository.DeleteAsync(model);
            repository.SaveChanges();
        }

        public IEnumerable<Invoice> GetMany(Expression<Func<Invoice, bool>> expression, List<string> references)
        {
            return repository.GetAll(expression, references);
        }

        public async Task<Invoice> GetOne(Expression<Func<Invoice, bool>> expression, List<string> references)
        {
            return await repository.Get(expression, references);
        }

        public void Insert(Invoice product)
        {
            if (product != null)
            {
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                repository.Insert(product);
            }
            repository.SaveChanges();
        }

        public void Update(Invoice product)
        {
            if (product != null)
            {
                repository.Update(product);
            }
            repository.SaveChanges();
        }
    }
}
