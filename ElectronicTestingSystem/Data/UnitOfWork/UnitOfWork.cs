using ElectronicTestingSystem.Repository;
using ElectronicTestingSystem.Repository.IRepository;
using System.Collections;

namespace ElectronicTestingSystem.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ElectronicTestingSystemDbContext _context;

        private Hashtable _repositories;

        public UnitOfWork(ElectronicTestingSystemDbContext context)
        {
            _context = context;
        }
        public bool Complete()
        {
            var numberOfAffectedRows = _context.SaveChanges();
            return numberOfAffectedRows > 0;
        }

        public ITestingSystemRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(TestingSystemRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (ITestingSystemRepository<TEntity>)_repositories[type];
        }
    }
}
