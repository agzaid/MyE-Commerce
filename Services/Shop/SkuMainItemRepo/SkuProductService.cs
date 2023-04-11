using Data.Entities.Cashier;
using Data.Entities.Shop;
using Repo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Shop
{
    public class SkuMainItemService : ISkuMainItemService
    {
        private readonly IRepository<SkuMainItem> repository;

        public SkuMainItemService(IRepository<SkuMainItem> repository)
        {
            this.repository = repository;
        }
        public async Task Delete(int id)
        {
            SkuMainItem model = await GetOne(s => s.ID == id, null);

            await repository.DeleteAsync(model);
            repository.SaveChanges();
        }

        public IEnumerable<SkuMainItem> GetMany(Expression<Func<SkuMainItem, bool>> expression, List<string> references)
        {
            return repository.GetAll(expression, references);
        }

        public async Task<SkuMainItem> GetOne(Expression<Func<SkuMainItem, bool>> expression, List<string> references)
        {
            return await repository.Get(expression, references);
        }

        public void Insert(SkuMainItem product)
        {
            if (product != null)
            {
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                repository.Insert(product);
            }
            repository.SaveChanges();
        }

        public void Update(SkuMainItem product)
        {
            if (product != null)
            {
                repository.Update(product);
            }
            repository.SaveChanges();
        }
    }
}
