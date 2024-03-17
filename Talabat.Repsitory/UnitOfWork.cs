using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repsitory.Data;
using Talabat.Repsitory.Repositories;

namespace Talabat.Repsitory
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
            repoistories = new Hashtable();

        }
        Hashtable repoistories;

        public async Task<int> CompleteAsync()
        {
           return await _dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;
            if (!repoistories.ContainsKey(key))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);
                repoistories.Add(key, repository); 
            }
            return repoistories[key] as IGenericRepository<TEntity>  ;
            ///
            ///var key = typeof(TEntity).Name;
            ///if(!repoistories.ContainsKey(key))
            ///{
            ///    var repository = new GenericRepository<TEntity>(_dbContext);
            ///    repoistories.Add(key, repository);
            ///}
            ///return repoistories[key] as IGenericRepository<TEntity>;
        }
    }
}
