using Data.Entities.Shop;
using Data.Entities.User;
using Repo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Shop.CustomerRepo
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> repository;

        public CustomerService(IRepository<Customer> repository)
        {
            this.repository = repository;
        }
        public async Task Delete(int id)
        {
            Customer model = await GetOne(s => s.ID == id, null);

            await repository.DeleteAsync(model);
            repository.SaveChanges();
        }

        public IEnumerable<Customer> GetMany(Expression<Func<Customer, bool>> expression, List<string> references)
        {
            return repository.GetAll(expression, references);
        }

        public async Task<Customer> GetOne(Expression<Func<Customer, bool>> expression, List<string> references)
        {
            return await repository.Get(expression, references);
        }

        public void Insert(Customer customer)
        {
            if (customer != null)
            {
                customer.CreatedDate = DateTime.Now;
                customer.ModifiedDate = DateTime.Now;
                repository.Insert(customer);
            }
            repository.SaveChanges();
        }

        public void Update(Customer customer)
        {
            if (customer != null)
            {
                repository.Update(customer);
            }
            repository.SaveChanges();
        }
    }
}
