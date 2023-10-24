using System.Linq.Expressions;
using LeitourApi.Data;
using LeitourApi.Interfaces;
using LeitourApi.Models;
using Microsoft.EntityFrameworkCore;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    private LeitourContext _context;
    readonly DbSet<Comment> dbSet;

    public const string VIEW_COMMENT = "vw_comentario";

    public CommentRepository(LeitourContext context) : base(context)
    {
        _context = context;
        dbSet = _context.Set<Comment>();
    }
    
     
    public override async Task<Comment?> GetById(int id) => 
        await dbSet.FindAsync(id);//().FromSql($"SELECT * {POST_VIEW_GET_POST} where pk_publicacao_id = {id}").FirstOrDefaultAsync();

    public override async Task<List<Comment>?> GetAll(int offset) => 
        await dbSet.ToListAsync();//.FromSql($"SELECT * from {POST_VIEW_GET_POST}").Skip(offset).Take(Constants.LIMIT_VALUE).ToListAsync();
}