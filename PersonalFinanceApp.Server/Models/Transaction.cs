using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Server.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
