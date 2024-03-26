using PersonalFinanceApp.Server.Models;

namespace PersonalFinanceApp.Server.DTO
{
    public class TotalIncomeExpenseDTO
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public IEnumerable<Transaction> IncomeTransactions { get; set; }
        public IEnumerable<Transaction> ExpenseTransactions { get; set; }
    }
}
