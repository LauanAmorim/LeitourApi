namespace LeitourApi.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(int id);
        Task<List<TEntity>> GetByAll(int id);
        Task<List<TEntity>> GetAll();

        string Debug(string value);

        // Implements methods here
    }
}