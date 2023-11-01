using System.Linq.Expressions;
using LeitourApi.Data;
using LeitourApi.Interfaces;
using LeitourApi.Models;
using Microsoft.EntityFrameworkCore;

public class UserRepository : Repository<User>, IUserRepository
{
    private LeitourContext _context;
    readonly DbSet<User> dbSet;
    public const string VIEW_USER = "vw_usuario";
    
     public UserRepository(LeitourContext context) : base(context)
    {
        _context = context;
        dbSet = _context.Set<User>();
    }
     
    public override async Task<User?> GetById(int id) => 
        await dbSet.FindAsync(id);

    public override async Task<List<User>?> GetAll(int offset) => 
        await dbSet.ToListAsync();

    public virtual async Task<User?> GetUser(int id) => 
        await dbSet.FromSql($"select * from tbl_usuario where pk_usuario_id = {id}").FirstOrDefaultAsync();

    public virtual async Task<User?> GetByEmail(string email) => 
        await dbSet.FromSql($"select * from tbl_usuario where usuario_email = {email}").FirstOrDefaultAsync();
    

    public virtual async Task<List<User>?> GetFollowers(int id) => 
        await dbSet.FromSql($"call sp_select_seguidor_seguidores({id});").ToListAsync();
    
    public virtual async Task<List<User>?> GetFollowing(string email) => 
        await dbSet.FromSql($"call sp_select_seguidor_seguintes({email});").ToListAsync();

}