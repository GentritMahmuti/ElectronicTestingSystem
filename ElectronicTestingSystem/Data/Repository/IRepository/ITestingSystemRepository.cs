using System.Linq.Expressions;

namespace ElectronicTestingSystem.Repository.IRepository
{
    public interface ITestingSystemRepository<Tentity> where Tentity : class
    {
        IQueryable<Tentity> GetByCondition(Expression<Func<Tentity, bool>> expression);

        IQueryable<Tentity> GetAll();

        IQueryable<Tentity> GetById(Expression<Func<Tentity, bool>> expression);

        void Create(Tentity entity);
        
        void Delete(Tentity entity);
  
        void Update(Tentity entity);
        
    }
}
