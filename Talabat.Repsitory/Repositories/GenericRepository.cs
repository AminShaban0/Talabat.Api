using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repsitory.Data;

namespace Talabat.Repsitory.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Product))
            {
                return  (IReadOnlyList<T>) await _dbContext.Set<Product>().Include(P=>P.Brand).Include(P=>P.Category).ToListAsync();
            }
          return  await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
          return await SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>() , spec).ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec)
        {
            return await SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec).CountAsync();
        }

        public async Task<T?> GetWithSpecAsync( ISpecification<T> spec)
        {
            return await SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec).FirstOrDefaultAsync();
        }

        public async Task AddAsync(T item)
        {
           await _dbContext.Set<T>().AddAsync(item);
        }

        public void Update(T item)
        {
            _dbContext.Update(item);
        }
    }
}
