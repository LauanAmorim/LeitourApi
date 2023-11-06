using System.Linq.Expressions;
using LeitourApi.Models;
namespace LeitourApi.Interfaces;
public interface IRepository<T> where T : class{
        Task<T?> GetById(int id);
        Task<List<T>?> GetAll();
        Task<List<T>?> GetAll(int offset);
        Task<T?> GetByCondition(Expression<Func<T, bool>> predicate);
        Task<List<T>?> GetAllByCondition(Expression<Func<T, bool>> predicate);
        Task<List<T>?> GetAllByCondition(Expression<Func<T, bool>> predicate,int offset);
        Task<List<T>?> GetAllWithJoin(Expression<Func<T, object>> join);
        Task<List<T>?> GetAllWithConditionJoin(Expression<Func<T, bool>> predicate,Expression<Func<T,string>> join);
        Task<T?> GetFromProcedure(string procedure);
        Task<T?> GetFromProcedure(string procedure, string param);
        Task<List<T>> GetAllFromProcedure(string procedure,int offset);
        Task<List<T>> GetAllFromProcedure(string procedure, string param,int offset);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> IsDeactivated(int id);
        int Count();
}