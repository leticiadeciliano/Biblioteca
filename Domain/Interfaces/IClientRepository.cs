using Domain;

namespace Biblioteca.Domain.Interfaces
{
    public interface IClientRepository
    {
        void Add(Client client);
        Client GetById(Guid ID);
        IEnumerable<Client> GetAll();
        void Update(Client client);
        void Delete(Guid ID);
    }
}
