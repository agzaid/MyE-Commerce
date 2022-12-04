using Data.Entities.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Shop
{
    public interface IProductService
    {
        IEnumerable<Product> GetMany(Expression<Func<Product, bool>> expression, List<string> references);
        Task<Product> GetOne(Expression<Func<Product, bool>> expression, List<string> references);
        void Insert(Product product);
        void Update(Product product);
        Task Delete(int id);
    }
}
