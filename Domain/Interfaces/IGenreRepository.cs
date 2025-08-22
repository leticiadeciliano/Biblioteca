using Domain;

namespace Biblioteca.Domain.Interfaces
{
    public interface IGenreRepository
    {
        void Add(Genre genre);
        Genre GetById(int ID);
        IEnumerable<Genre> GetAll();
        void Update(Genre genre);
        void Delete(int ID);
    }
}