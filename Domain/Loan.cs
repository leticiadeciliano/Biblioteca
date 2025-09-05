namespace Domain
{
    public class Loan
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid Client_ID { get; set; }
        public int Inventory_ID { get; set; }

        public int Days_to_expire { get; set; } = 30;

        public DateTime Created_At { get; set; } = DateTime.Now;
        public DateTime Updated_At { get; set; } = DateTime.Now;

        public DateTime Return_At { get; set; }

        // construtor para calcular dia de devolução a partir da criação do empréstimo
        public Loan()
        {
            Return_At = Created_At.AddDays(Days_to_expire);
        }
        //DateTime.Now registra a data atual
        public bool IsLate => DateTime.Now > Return_At;
    }
}

//OBS: Campos armazenados PRECISAM estar no SQLite, campos com cálculos de entidades já existentes não precisam
//ser declarados no Banco de Dados