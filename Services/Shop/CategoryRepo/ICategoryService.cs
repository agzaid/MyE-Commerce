using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.Shop;

namespace Services.Shop.CategoryRepo
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetMany(Expression<Func<Category, bool>> expression, List<string> references);
        Task<Category> GetOne(Expression<Func<Category, bool>> expression, List<string> references);
        void Insert(Category product);
        void Update(Category product);
        Task Delete(int id);
    }
}
