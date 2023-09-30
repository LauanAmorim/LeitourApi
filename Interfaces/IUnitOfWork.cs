using LeitourApi.Models;
namespace LeitourApi.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }
        IRepository<Post> PostRepository { get; }
        IRepository<Comment> CommentRepository { get; }
        IRepository<Annotation> AnnotationRepository { get; }
        IRepository<SavedBook> SavedRepository { get; }
        IRepository<Roles.Deactivated> DeactivateRepository {get;}
        void Commit();
    }
}