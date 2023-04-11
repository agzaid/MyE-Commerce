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
    public interface ISkuMainItemService
    {
        IEnumerable<SkuMainItem> GetMany(Expression<Func<SkuMainItem, bool>> expression, List<string> references);
        Task<SkuMainItem> GetOne(Expression<Func<SkuMainItem, bool>> expression, List<string> references);
        void Insert(SkuMainItem product);
        void Update(SkuMainItem product);
        Task Delete(int id);
    }
}
