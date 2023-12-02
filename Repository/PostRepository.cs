using System.Linq.Expressions;
using LeitourApi.Data;
using LeitourApi.Interfaces;
using LeitourApi.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

public class PostRepository : Repository<Post>, IPostRepository
{
    private LeitourContext _context;
    readonly DbSet<Post> dbSet;
  
    public const string VIEW_POST = "vw_publicacao";
    
     public PostRepository(LeitourContext context) : base(context)
    {
        _context = context;
        dbSet = _context.Set<Post>();
    }
     
    public override async Task<Post?> GetById(int id) => 
        await dbSet.FindAsync(id);

    public async Task<List<Post>?> GetAll(int offset, int id)=>
        await dbSet.FromSqlInterpolated($"call sp_select_publicacao({id},{Constants.LIMIT_VALUE},{offset})").ToListAsync();

    public async Task Like(int userId, int postId) => await _context.Database.ExecuteSqlInterpolatedAsync($"call sp_like({userId},{postId},@vSucesso)");
    
}