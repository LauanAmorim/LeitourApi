using System.Linq.Expressions;
using LeitourApi.Interfaces;
using LeitourApi.Models;

public interface IUserRepository : IRepository<User>
{
  new Task<User?> GetById(int id);
  new Task<List<User>?> GetAll(int id);
  Task<User?> GetUser(int id);
  Task<User?> GetByEmail(string email);
  Task<List<User>?> GetByUsername(int offset,string name);
  Task Follow(int id,string email);
  Task Unfollow(int id,string email);
  Task<List<User>?> GetFollowers(int id);
  Task<List<User>?> GetFollowing(string email);
}