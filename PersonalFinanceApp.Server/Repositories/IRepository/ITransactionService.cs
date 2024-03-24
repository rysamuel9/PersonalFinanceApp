
using PersonalFinanceApp.Server.DTO;
using PersonalFinanceApp.Server.Models;

namespace PersonalFinanceApp.Server.Repositories.IRepository
{
    public interface ITransactionService
    {
        Task<Transaction> CreateTransactionAsync(Guid userId, decimal amount, string type, string category, string description, DateTime date);
        Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(Guid userId);
        Task<IEnumerable<Transaction>> GetFilteredTransactionsAsync(Guid userId, DateTime? startDate, DateTime? endDate, string type, string category);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync(Guid userId, int pageNumber, int pageSize);
        Task<TotalIncomeExpenseDTO> GetTotalIncomeExpenseAsync(int year, int? month);
    }
}
