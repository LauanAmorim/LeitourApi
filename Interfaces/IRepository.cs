using System.Linq.Expressions;
namespace LeitourApi.Interfaces;
public interface IRepository<T> where T : class{
        Task<T?> GetById(int id);
        Task<List<T>?> GetAll();
        Task<T?> GetByCondition(Expression<Func<T, bool>> predicate);
        Task<List<T>?> GetAllByCondition(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> IsDeactivated(int id);
        int Count();
}