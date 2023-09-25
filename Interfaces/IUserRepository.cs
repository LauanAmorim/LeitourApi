using LeitourApi.Models;
using LeitourApi.Repository;

namespace LeitourApi.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<List<User>> GetAllUsers();

        // Implements methods here
    }
}