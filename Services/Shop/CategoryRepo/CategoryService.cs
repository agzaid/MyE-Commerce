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
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _repository;
        public CategoryService(IRepository<Category> repository)
        {
            _repository = repository;
        }
        
        public IEnumerable<Category> GetMany(Expression<Func<Category, bool>> expression, List<string> references)
        {
            return _repository.GetAll(expression, references);
        }

        public async Task<Category> GetOne(Expression<Func<Category, bool>> expression, List<string> references)
        {
            return await _repository.Get(expression, references);
        }

        public void Insert(Category model)
        {
            if (model is not null)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                _repository.Insert(model);
            }
            _repository.SaveChanges();
        }

        public void Update(Category model)
        {
            if (model is not null)
            {
                _repository.Update(model);
            }
            _repository.SaveChanges();
        }
        public async Task Delete(int id)
        {
            Category model = await GetOne(s => s.ID == id, null);

            _repository.Delete(model);
            _repository.SaveChanges();
        }
    }
}
