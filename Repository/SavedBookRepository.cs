using System.Linq.Expressions;
using LeitourApi.Data;
using LeitourApi.Interfaces;
using LeitourApi.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

public class SavedBookRepository : Repository<SavedBook>, ISavedBookRepository
{
    private LeitourContext _context;
    readonly DbSet<SavedBook> dbSet;
  
    public const string VIEW_POST = "vw_publicacao";
    
     public SavedBookRepository(LeitourContext context) : base(context)
    {
        _context = context;
        dbSet = _context.Set<SavedBook>();
    }

    public virtual async Task<List<SavedBook>?> GetByTitle(string title) => await GetByTitle(title,0);
    

    public virtual async Task<List<SavedBook>?> GetByTitle(string title, int offset) =>
        await dbSet.Where(b => b.BookTitle.Contains(title)).Skip(0).Take(Constants.LIMIT_VALUE).ToListAsync();
    
}