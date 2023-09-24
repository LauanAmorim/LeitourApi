namespace LeitourApi.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(int id);
        Task<List<TEntity>> GetByAll(int id);
        Task<List<TEntity>> GetAll();

        // Implements methods here
    }
}