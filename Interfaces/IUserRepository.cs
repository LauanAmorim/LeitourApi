using LeitourApi.Models;
using LeitourApi.Repository;

namespace LeitourApi.Interfaces
{
    public interface IUserRepository //IGenericRepository<User>
    {
        Task<List<User>> GetAll();
        Task<User> GetById(int id);

        // Implements methods here
    }
}