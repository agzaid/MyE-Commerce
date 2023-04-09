using Data.Entities.Cashier;
using Data.Entities.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Shop
{
    public interface ISkuProductService
    {
        IEnumerable<SkuProduct> GetMany(Expression<Func<SkuProduct, bool>> expression, List<string> references);
        Task<SkuProduct> GetOne(Expression<Func<SkuProduct, bool>> expression, List<string> references);
        void Insert(SkuProduct product);
        void Update(SkuProduct product);
        Task Delete(int id);
    }
}
