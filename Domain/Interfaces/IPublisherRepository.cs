using System;
using Domain;

namespace Biblioteca.Domain.Interfaces
{
    public interface IPublisherRepository
    {
        void Add(Publisher publisher);    //método Add (CREATE)
        Publisher GetById(int ID);       //método GetByid
        IEnumerable<Publisher> GetAll();  //método GetAll
        void Update(Publisher publisher); //método UPDATE
        void Delete(int ID);             //método DELETE
    }
}

//Interface usada para definir o que cada entidade precisa ter para ser criada, neste caso, é necessário
// ter o CRUD para dar continuidade na criação.