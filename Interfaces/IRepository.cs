using System.Linq.Expressions;
namespace LeitourApi.Interfaces;
public interface IRepository<T> where T : class{
        Task<T?> GetById(int id);
        Task<List<T>?> GetAll();
        Task<T?> GetByCondition(Expression<Func<T, bool>> predicate);
        Task<List<T>?> GetAllByCondition(Expression<Func<T, bool>> predicate);
        Task<T?> GetWithSelect(Expression<Func<T, bool>> predicate);
        Task<List<T>?> GetAllWithSelect(Expression<Func<T, bool>> predicate);
        Task<T?> GetFromProcedure(string procedure, string param);
        Task<List<T>> GetAllFromProcedure(string procedure, string param);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> IsDeactivated(int id);
        int Count();
}