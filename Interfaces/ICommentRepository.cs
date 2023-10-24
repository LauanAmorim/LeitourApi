using System.Linq.Expressions;
using LeitourApi.Interfaces;
using LeitourApi.Models;

public interface ICommentRepository : IRepository<Comment>
{
  new Task<Comment?> GetById(int id);
  new Task<List<Comment>?> GetAll(int id);
}