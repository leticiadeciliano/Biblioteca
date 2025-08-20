using Domain;

namespace Biblioteca.Domain.Interfaces
{
    public interface IGenreRepository
    {
        void Add(Genre genre);
        Genre GetById(Guid ID);
        IEnumerable<Genre> GetAll();
        void Update(Genre genre);
        void Delete(Guid ID);
    }
}