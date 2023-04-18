using Data.Entities.Cashier;
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
    public class SkuSubItemService : ISkuSubItemService
    {
        private readonly IRepository<SkuSubItem> repository;

        public SkuSubItemService(IRepository<SkuSubItem> repository)
        {
            this.repository = repository;
        }
        public async Task Delete(int id)
        {
            SkuSubItem model = await GetOne(s => s.ID == id, null);

            await repository.DeleteAsync(model);
            repository.SaveChanges();
        }

        public IEnumerable<SkuSubItem> GetMany(Expression<Func<SkuSubItem, bool>> expression, List<string> references)
        {
            return repository.GetAll(expression, references);
        }

        public async Task<SkuSubItem> GetOne(Expression<Func<SkuSubItem, bool>> expression, List<string> references)
        {
            return await repository.Get(expression, references);
        }

        public void Insert(SkuSubItem product)
        {
            if (product != null)
            {
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                repository.Insert(product);
            }
            repository.SaveChanges();
        }

        public void Update(SkuSubItem product)
        {
            if (product != null)
            {
                repository.Update(product);
            }
            repository.SaveChanges();
        }
    }
}
