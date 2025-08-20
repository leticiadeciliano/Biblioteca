using Domain;

namespace Biblioteca.Domain.Interfaces
{
    public interface ILoanRepository
    {
        void Add(Loan loan);
        Loan GetById(Guid ID);
        IEnumerable<Loan> GetAll();
        void Update(Loan loan);
        void Delete(Guid ID);
    }
}