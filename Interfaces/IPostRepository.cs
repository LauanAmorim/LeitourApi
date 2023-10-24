using System.Linq.Expressions;
using LeitourApi.Interfaces;
using LeitourApi.Models;

public interface IPostRepository : IRepository<Post>
{
  new Task<Post?> GetById(int id);
  new Task<List<Post>?> GetAll(int id);
}