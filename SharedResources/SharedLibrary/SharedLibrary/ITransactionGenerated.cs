using static SharedLibrary.Enumeration;

namespace SharedLibrary
{
    public interface ITransactionGenerated
    {
        public Guid UserId { get; set; }
        public double Amount  { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid AccountId { get; set;}
        public TransactionType TransactionType { get; set; }
    }
}