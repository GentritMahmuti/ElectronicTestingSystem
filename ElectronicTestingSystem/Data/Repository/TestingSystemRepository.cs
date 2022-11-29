using ElectronicTestingSystem.Data;
using ElectronicTestingSystem.Repository.IRepository;
using System.Linq.Expressions;

namespace ElectronicTestingSystem.Repository
{
    public class TestingSystemRepository<Tentity> : ITestingSystemRepository<Tentity> where Tentity : class
    {
        private readonly ElectronicTestingSystemDbContext _dbContext;

        public TestingSystemRepository(ElectronicTestingSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Tentity> GetByCondition(Expression<Func<Tentity, bool>> expression)
        {
            return _dbContext.Set<Tentity>().Where(expression);
        }

        public IQueryable<Tentity> GetAll()
        {
            var result = _dbContext.Set<Tentity>();

            return result;
        }

        public IQueryable<Tentity> GetById(Expression<Func<Tentity, bool>> expression)
        {
            return _dbContext.Set<Tentity>().Where(expression);
        }

        public void Create(Tentity entity)
        {
            _dbContext.Set<Tentity>().Add(entity);
        }

        public void Delete(Tentity entity)
        {
            _dbContext.Set<Tentity>().Remove(entity);
        }
        public void Update(Tentity entity)
        {
            _dbContext.Set<Tentity>().Update(entity);
        }
    }
}
