using System.Linq.Expressions;
using LeitourApi.Data;
using LeitourApi.Interfaces;
using LeitourApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
public class Repository<T> : IRepository<T> where T : class
{
    private LeitourContext _context;
    readonly DbSet<T> dbSet;
    readonly DbSet<User> dbSetUser;
    public Repository(LeitourContext context)
    {
        _context = context;
        dbSet = _context.Set<T>();
        dbSetUser = _context.Set<User>();
    }

    public virtual async Task<T?> GetById(int id) => await dbSet.FindAsync(id);

    public virtual async Task<List<T>?> GetAll() => await dbSet.Skip(0).Take(Constants.LIMIT_VALUE).ToListAsync();
    public virtual async Task<List<T>?> GetAll(int offset) => await dbSet.Skip(offset).Take(Constants.LIMIT_VALUE).ToListAsync();
    public async Task<T?> GetByCondition(Expression<Func<T, bool>> predicate) =>
       await dbSet.Where(predicate).FirstOrDefaultAsync();

    public async Task<List<T>?> GetAllByCondition(Expression<Func<T, bool>> predicate) =>
        await dbSet.Where(predicate).ToListAsync();

    public async Task<List<T>?> GetAllByCondition(Expression<Func<T, bool>> predicate, int offset) =>
        await dbSet.Where(predicate).Skip(offset).Take(Constants.LIMIT_VALUE).ToListAsync();


    public void Add(T entity)
    {
        dbSet.Add(entity);
        _context.SaveChanges();
    }

    public void Update(T entity)
    {
        dbSet.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void Delete(T entity)
    {
        dbSet.Remove(entity);
        _context.SaveChanges();
    }

    public async Task<bool> IsDeactivated(int id)
    {
        User? user = await dbSetUser.FindAsync(id);
        return user == null || user.Access == "Desativado";
    }
    public int Count() => dbSet.Count();

    public async Task<T?> GetFromProcedure(string procedure, string param) => await dbSet.FromSql($"EXECUTE {procedure} {param}").FirstOrDefaultAsync();
    public async Task<T?> GetFromProcedure(string procedure) => await dbSet.FromSql($"EXECUTE {procedure}").FirstOrDefaultAsync();
    public async Task<List<T>> GetAllFromProcedure(string procedure,int offset) => await dbSet.FromSql($"EXECUTE {procedure}").Skip(offset).Take(Constants.LIMIT_VALUE).ToListAsync();
    public async Task<List<T>> GetAllFromProcedure(string procedure, string param,int offset) => await dbSet.FromSql($"EXECUTE {procedure} {param}").Skip(offset).Take(Constants.LIMIT_VALUE).ToListAsync();

    public async Task<List<T>?> GetAllWithJoin(Expression<Func<T, object>> join) =>
        await dbSet.Include(join).ToListAsync();
    public async Task<List<T>?> GetAllWithConditionJoin(Expression<Func<T, bool>> predicate,Expression<Func<T,string>> join) =>
        await dbSet.Where(predicate).Include(join).ToListAsync();
}