using Domain.Entities;
using Domain.Interfaces;
using Persistence.DbContext;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private Hashtable _repositories; 
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

                        if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<,>);

                                var repositoryInstance = Activator.CreateInstance(
                    repositoryType.MakeGenericType(typeof(TEntity), typeof(TKey)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity, TKey>)_repositories[type];
        }

        public async Task<int> CompleteAsync()
        {
                        return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
                        _context.Dispose();
        }
    }
}