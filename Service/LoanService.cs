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
            return (List<Loan>)_loanRepository.GetAll();
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

        public void Create(Guid clientID,Guid inventoryID, int days_to_expire)
        {
            var loan = new Loan
            {
                ID = Guid.NewGuid(),
                ClientID = Guid.NewGuid(),
                InventoryID = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Days_to_expire = days_to_expire
            };

            loan.ReturnAt = loan.CreatedAt.AddDays(loan.Days_to_expire);

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
            } //clientID e inventoryID não podem ser alterados pelo usuário, portanto, não deve estar em Update

            existingloan.Days_to_expire = days_to_expire;

            existingloan.UpdatedAt = DateTime.Now;

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

        internal void Update(int newDays)
        {
            throw new NotImplementedException();
        }
    }
}
