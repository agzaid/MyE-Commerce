using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.Shop;

namespace Services.Shop.CategoryRepo
{
    public interface ICartService
    {
        IEnumerable<ShoppingCart> GetMany(Expression<Func<ShoppingCart, bool>> expression, List<string> references);
        Task<ShoppingCart> GetOne(Expression<Func<ShoppingCart, bool>> expression, List<string> references);
        void Insert(ShoppingCart product);
        void Update(ShoppingCart product);
        Task Delete(int id);
    }
}
