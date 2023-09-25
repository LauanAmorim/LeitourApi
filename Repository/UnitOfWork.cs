using System;
using LeitourApi.Interfaces;
using LeitourApi.Models;

namespace LeitourApi.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly LeitourContext _context;
        public IUserRepository User {get; private set;}
    
        public UnitOfWork(LeitourContext context){
            _context = context;
            User = new UserRepository(context);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }
    }
}