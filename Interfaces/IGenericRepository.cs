using System.Linq.Expressions;

namespace LeitourApi.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<List<T>> GetAll();

        //Task<List<T>> GetAllById(int id);
        Task<T> FindByCondition(Expression<Func<T, bool>> predicate);
        Task<List<T>> FindByConditionList(Expression<Func<T, bool>> predicate);

        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);

        string Debug(string value);
    }
}