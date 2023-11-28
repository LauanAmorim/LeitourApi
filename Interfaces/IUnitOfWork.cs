using LeitourApi.Models;
namespace LeitourApi.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IPostRepository PostRepository { get; }
        IRepository<Comment> CommentRepository { get; }
        IRepository<Annotation> AnnotationRepository { get; }
        ISavedBookRepository SavedRepository { get; }
        void Commit();
    }
}