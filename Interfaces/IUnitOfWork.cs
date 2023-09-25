namespace LeitourApi.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository User {get;}

        int Complete();
    }
}