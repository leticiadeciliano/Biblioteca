using System;
using System.Collections.Generic;
using Domain;
using Storage;

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
            return _loanRepository.GetAll();
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
                Console.WriteLine("Empréstimo não encontrado.");
                return null;
            }

            return loan;
        }

        public void Create(Guid clientID, Guid inventoryID, int? days_to_expire = null)
        {
            var loan = new Loan
            {
                ID = Guid.NewGuid(),
                ClientID = clientID,
                InventoryID = inventoryID,
                Days_to_expire = days_to_expire ?? 30,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            loan.ReturnAt = loan.CreatedAt.AddDays(loan.Days_to_expire);

            _loanRepository.Add(loan);
            Console.WriteLine("Empréstimo criado com sucesso!");
        }

        public void Update(Guid ID, Guid clientID, Guid inventoryID, int days_to_expire, DateTime? returnAt = null)
        {
            var existingLoan = _loanRepository.GetById(ID);
            if (existingLoan == null)
            {
                Console.WriteLine("Empréstimo não encontrado.");
                return;
            }

            existingLoan.ClientID = clientID;
            existingLoan.InventoryID = inventoryID;
            existingLoan.Days
        }
    }
}