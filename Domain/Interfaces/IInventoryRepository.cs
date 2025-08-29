using Domain;
namespace Biblioteca.Domain.Interfaces
{
    public interface IInventoryRepository
    {
        void Add(Inventory inventory);
        IEnumerable<Inventory> GetAll();
        void Update(Inventory inventory);
        void Delete(int ID);
    }
}