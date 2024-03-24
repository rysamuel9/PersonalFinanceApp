namespace PersonalFinanceApp.Server.DTO
{
    public class TransactionCreateDTO
    {
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
