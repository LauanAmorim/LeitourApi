using System;
using LeitourApi.Interfaces;
using LeitourApi.Models;
using LeitourApi.Data;

namespace LeitourApi.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
       private LeitourContext _context;
        private Repository<User>? userRepository {get;}
        private PostRepository? postRepository {get;}
        private CommentRepository?  commentRepository {get;}
        private Repository<Annotation>? annotationRepository {get;}
        private Repository<SavedBook>? savedRepository {get;}
        public UnitOfWork(LeitourContext context) => _context = context;
        
        public void Commit() => _context.SaveChanges();
        public IRepository<User> UserRepository => userRepository ?? new Repository<User>(_context);
        public IRepository<Post> PostRepository => postRepository ?? new PostRepository(_context);
        public IRepository<Annotation> AnnotationRepository => annotationRepository ?? new Repository<Annotation>(_context);
        public IRepository<SavedBook> SavedRepository => savedRepository ?? new Repository<SavedBook>(_context);
        public IRepository<Comment> CommentRepository => commentRepository ?? new Repository<Comment>(_context);

       private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    _context.Dispose();
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
}
}