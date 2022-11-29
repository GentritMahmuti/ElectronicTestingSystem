using ElectronicTestingSystem.Repository.IRepository;

namespace ElectronicTestingSystem.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        public ITestingSystemRepository<TEntity> Repository<TEntity>() where TEntity : class;
        bool Complete();
    }
}
