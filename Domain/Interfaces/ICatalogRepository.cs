using Domain;

namespace Biblioteca.Domain.Interfaces
{
    public interface ICatalogRepository
    {
        void Add(Catalog catalog);
        Catalog GetById(Guid ID);
        IEnumerable<Catalog> GetAll();
        void Update(Catalog catalog);
        void Delete(Guid ID);
    }
}