using System.Linq.Expressions;
using LeitourApi.Interfaces;
using LeitourApi.Models;

public interface ISavedBookRepository : IRepository<SavedBook>
{
  Task<List<SavedBook>?> GetByTitle(string title);
  Task<List<SavedBook>?> GetByTitle(string title,int offset);
}