using System.Linq.Expressions;
using LeitourApi.Interfaces;
using LeitourApi.Models;

public interface IPostRepository : IRepository<Post>
{
  new Task<Post?> GetById(int id);
  Task<List<Post>?> GetAll(int offset, int id);
  public Task Like(int userId, int postId);
}