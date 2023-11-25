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
        await dbSet.FindAsync(id);//().FromSql($"SELECT * {POST_VIEW_GET_POST} where pk_publicacao_id = {id}").FirstOrDefaultAsync();

    public override async Task<List<Post>?> GetAll(int offset) => 
        await dbSet.ToListAsync();//.FromSql($"SELECT * from {POST_VIEW_GET_POST}").Skip(offset).Take(Constants.LIMIT_VALUE).ToListAsync();

    public async Task<int> Like(int userId, int postId){
       /* string query ="call sp_like(@vId,@vPublicacao)";
        MySqlCommand myCommand = new(query);
        myCommand.
        myCommand.Parameters.Add("@vId", (System.Data.DbType)userId);
        myCommand.Parameters.Add("@vPublicacao ",postId );
        await myCommand.ExecuteNonQueryAsync();*/
       await dbSet.FromSql($"call sp_like({userId},{postId})'").SingleAsync();
       return 0;
       }
}