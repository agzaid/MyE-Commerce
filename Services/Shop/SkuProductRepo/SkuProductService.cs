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
    public class SkuProductService : ISkuProductService
    {
        private readonly IRepository<SkuProduct> repository;

        public SkuProductService(IRepository<SkuProduct> repository)
        {
            this.repository = repository;
        }
        public async Task Delete(int id)
        {
            SkuProduct model = await GetOne(s => s.ID == id, null);

            await repository.DeleteAsync(model);
            repository.SaveChanges();
        }

        public IEnumerable<SkuProduct> GetMany(Expression<Func<SkuProduct, bool>> expression, List<string> references)
        {
            return repository.GetAll(expression, references);
        }

        public async Task<SkuProduct> GetOne(Expression<Func<SkuProduct, bool>> expression, List<string> references)
        {
            return await repository.Get(expression, references);
        }

        public void Insert(SkuProduct product)
        {
            if (product != null)
            {
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                repository.Insert(product);
            }
            repository.SaveChanges();
        }

        public void Update(SkuProduct product)
        {
            if (product != null)
            {
                repository.Update(product);
            }
            repository.SaveChanges();
        }
    }
}
