using System.Linq.Expressions;
using LeitourApi.Interfaces;
using LeitourApi.Models;

public interface IUserRepository : IRepository<User>
{
  new Task<User?> GetById(int id);
  new Task<List<User>?> GetAll(int id);
  Task<User?> GetUser(int id);
  Task<User?> GetByEmail(string email);
}