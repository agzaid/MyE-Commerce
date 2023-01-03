using Data.Entities.Shop;
using Repo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Shop.CategoryRepo
{
    public class CartService : ICartService
    {
        private readonly IRepository<ShoppingCart> _repository;
        public CartService(IRepository<ShoppingCart> repository)
        {
            _repository = repository;
        }
        
        public IEnumerable<ShoppingCart> GetMany(Expression<Func<ShoppingCart, bool>> expression, List<string> references)
        {
            return _repository.GetAll(expression, references);
        }

        public async Task<ShoppingCart> GetOne(Expression<Func<ShoppingCart, bool>> expression, List<string> references)
        {
            return await _repository.Get(expression, references);
        }

        public void Insert(ShoppingCart model)
        {
            if (model is not null)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                _repository.Insert(model);
            }
            _repository.SaveChanges();
        }

        public void Update(ShoppingCart model)
        {
            if (model is not null)
            {
                _repository.Update(model);
            }
            _repository.SaveChanges();
        }
        public async Task Delete(int id)
        {
            ShoppingCart model = await GetOne(s => s.ID == id, null);

            _repository.Delete(model);
            _repository.SaveChanges();
        }
    }
}
