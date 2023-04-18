using Data.Entities.Cashier;
using Data.Entities.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Cashier
{
    public interface ISkuSubItemService
    {
        IEnumerable<SkuSubItem> GetMany(Expression<Func<SkuSubItem, bool>> expression, List<string> references);
        Task<SkuSubItem> GetOne(Expression<Func<SkuSubItem, bool>> expression, List<string> references);
        void Insert(SkuSubItem product);
        void Update(SkuSubItem product);
        Task Delete(int id);
    }
}
