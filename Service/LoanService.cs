using Storage;
using Domain;

namespace Service
{
    public class LoanService
    {
        private readonly LoanRepository _loanRepository;

        public LoanService()
        {
            _loanRepository = new LoanRepository();
        }

        public List<Loan> GetAll()
        {
            return _loanRepository.GetAll().ToList();
        }

        public Loan? GetById(Guid ID)
        {
            if (ID == Guid.Empty)
            {
                Console.WriteLine("ID inválido.");
                return null;
            }

            var loan = _loanRepository.GetById(ID);

            if (loan == null)
            {
                Console.WriteLine("Catálogo não encontrado.");
                return null;
            }

            return loan;
        }

        public void CreateLoan(Guid client_ID, int inventory_ID, int days_to_expire)
        {
            var loan = new Loan
            {
                ID = Guid.NewGuid(),
                Client_ID = client_ID,
                Inventory_ID = inventory_ID,
                Days_to_expire = days_to_expire,
                Created_At = DateTime.Now,
                Updated_At = DateTime.Now,
                Return_At = DateTime.Now.AddDays(days_to_expire)
            };

            _loanRepository.Add(loan);
            Console.WriteLine("Empréstimo criado com sucesso!");
        }


        public void Update(Guid ID, int days_to_expire)
        {
            var existingloan = _loanRepository.GetById(ID);
            if (existingloan == null)
            {
                Console.WriteLine("Empréstimo não encontrado.");
                return;
            } //client_ID e inventory_ID não podem ser alterados pelo usuário, portanto, não deve estar em Update

            existingloan.Days_to_expire = days_to_expire;

            existingloan.Updated_At = DateTime.Now;

            _loanRepository.Update(existingloan);
            Console.WriteLine("Empréstimo atualizado com sucesso!");
        }

        public void Delete(Guid ID)
        {
            var existingLoan = _loanRepository.GetById(ID);
            if (existingLoan == null)
            {
                Console.WriteLine("Empréstimo não encontrado.");
                return;
            }

            _loanRepository.Delete(ID);
            Console.WriteLine("Empréstimo removido com sucesso!");
        }
    }
}
