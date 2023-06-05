
using static BankOperations.Contracts.Enumeration;

namespace BankOperations.Contracts
{
    public interface ITransactionGenerated
    {
        public Guid CustomerId { get; set; }
        public double Amount  { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AccountNumber { get; set;}
        public TransactionType TransactionType { get; set; }
        public string TransferToAccountNumber { get; set; }
    }
}