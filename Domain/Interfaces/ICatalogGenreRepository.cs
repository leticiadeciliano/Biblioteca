using Domain;

namespace Biblioteca.Domain.Interfaces
{
    public interface ICatalogGenreRepository
    {
        CatalogGenre GetById(Guid ID);
        IEnumerable<CatalogGenre> GetAll();
        void Update(CatalogGenre cataloggenre);
        void Delete(Guid ID);
    }
}