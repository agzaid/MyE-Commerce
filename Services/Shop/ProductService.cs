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
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> repository;

        public ProductService(IRepository<Product> repository)
        {
            this.repository = repository;
        }
        public async Task Delete(int id)
        {
            Product model = await GetOne(s => s.ID == id, null);

            await repository.DeleteAsync(model);
            repository.SaveChanges();
        }

        public IEnumerable<Product> GetMany(Expression<Func<Product, bool>> expression, List<string> references)
        {
            return repository.GetAll(expression, references);
        }

        public async Task<Product> GetOne(Expression<Func<Product, bool>> expression, List<string> references)
        {
            return await repository.Get(expression, references);
        }

        public void Insert(Product product)
        {
            if (product != null)
            {
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                repository.Insert(product);
            }
            repository.SaveChanges();
        }

        public void Update(Product product)
        {
            if (product != null)
            {
                repository.Update(product);
            }
            repository.SaveChanges();
        }
    }
}
