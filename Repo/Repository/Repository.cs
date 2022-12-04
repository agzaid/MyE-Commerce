using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repo.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDBContext context;
        private readonly DbSet<T> entities;

        public Repository(ApplicationDBContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public int Count(Expression<Func<T, bool>> expression)
        {
            return entities.Count(expression);
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            SaveChanges();
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> references)
        {
            if (references != null && references.Any())
            {
                IQueryable<T> query = entities.Include(references.FirstOrDefault());

                if (references.Count > 1)
                    foreach (var item in references.Skip(1))
                    {
                        query = query.Include(item);
                    }

                return await query.FirstOrDefaultAsync(expression);
            }
            else
                return await entities.FirstOrDefaultAsync(expression);
        }

        public T Get(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> expression, List<string> references)
        {
            if (references != null && references.Any())
            {
                var query = entities.Include(references.FirstOrDefault());

                if (references.Count > 1)
                    foreach (var item in references.Skip(1))
                    {
                        query = query.Include(item);
                    }

                return query.Where(expression).ToList();
            }
            else
                return entities.Where(expression).ToList();
        }

        public IEnumerable<T> GetAllPaginated(Expression<Func<T, bool>> expression, int page, int pageSize, List<string> references, bool ascending = true)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAllQuerable(Expression<Func<T, bool>> expression, List<string> references)
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            SaveChanges();
        }
        public async Task InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await entities.AddAsync(entity);
            await SaveChangesAsync();
        }
        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            SaveChanges();
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            await SaveChangesAsync();
        }
        public void SaveChanges()
        {
            context.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }


        public Task DeleteAsync(T entity)
        {
            entities.Remove(entity);
            return SaveChangesAsync();
        }
    }
}
