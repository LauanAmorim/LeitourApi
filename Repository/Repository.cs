using System.Linq.Expressions;
using LeitourApi.Data;
using LeitourApi.Interfaces;
using LeitourApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
public class Repository<T> : IRepository<T> where T : class{
        private LeitourContext _context;
        readonly DbSet<T> dbSet;
        readonly DbSet<User> dbSetUser;
        public Repository(LeitourContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
            dbSetUser = _context.Set<User>();
        }

        public async Task<T?> GetById(int id) => await dbSet.FindAsync(id);

        public async Task<List<T>?> GetAll() => await dbSet.ToListAsync();
        public async Task<T?> GetByCondition(Expression<Func<T, bool>> predicate) =>
           await dbSet.Where(predicate).FirstOrDefaultAsync();
        public async Task<List<T>?> GetAllByCondition(Expression<Func<T, bool>> predicate) =>
            await dbSet.Where(predicate).ToListAsync();

       // public Task<List<T>> GetByJoin(IQueryable<IGrouping<bool,T>> predicate) => dbSet.GroupBy(predicate);
                
        public void Add(T entity){
            dbSet.AddAsync(entity);
            _context.SaveChanges();
        }
        
        public void Update(T entity){
            dbSet.Entry(entity).State = EntityState.Modified;
             _context.SaveChanges();
        }

        public void Delete(T entity){
            dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<bool> IsDeactivated(int id){
            User? user = await dbSetUser.FindAsync(id);
            return user.Access == "Desativado";
        }
        public int Count() => dbSet.Count();

        public async Task<T?> GetFromProcedure(string procedure, string param) => await dbSet.FromSql($"EXECUTE {procedure} {param}").FirstOrDefaultAsync();
        public async Task<List<T>> GetAllFromProcedure(string procedure, string param) => await dbSet.FromSql($"EXECUTE {procedure} {param}").ToListAsync();

    public Task<T?> GetWithSelect(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<List<T>?> GetAllWithSelect(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}