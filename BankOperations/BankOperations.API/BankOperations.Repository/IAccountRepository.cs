using SharedLibrary;

namespace BankOperations.Repository
{
    interface IRepository<T> where T : class
    {
        void Deposit(TransactionGenerated transactionGenerated);
        void Withdrawl(TransactionGenerated transactionGenerated);
        void Transfer(TransactionGenerated transactionGenerated);
    }
}
