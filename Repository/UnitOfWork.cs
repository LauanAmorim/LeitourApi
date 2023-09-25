using System;
using LeitourApi.Interfaces;
using LeitourApi.Models;

namespace LeitourApi.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly LeitourContext context;
        public IUserRepository User {get;}

        public UnitOfWork(LeitourContext context, IUserRepository userRepository){
            this.context = context;
            User = userRepository;
        }

        public int Complete() => context.SaveChanges();
        

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    context.Dispose();
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}