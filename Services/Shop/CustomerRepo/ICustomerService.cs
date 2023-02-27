using Data.Entities.Shop;
using Data.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Shop.CustomerRepo
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetMany(Expression<Func<Customer, bool>> expression, List<string> references);
        Task<Customer> GetOne(Expression<Func<Customer, bool>> expression, List<string> references);
        void Insert(Customer customer);
        void Update(Customer customer);
        Task Delete(int id);
    }
}
