using PersonalFinanceApp.Server.DTO;
using PersonalFinanceApp.Server.Models;
using PersonalFinanceApp.Server.Repositories.IRepository;

namespace PersonalFinanceApp.Server.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<TotalIncomeExpenseDTO> GetTotalIncomeExpenseAsync(int year, int? month)
        {
            return await _transactionRepository.GetTotalIncomeExpenseAsync(year, month);
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync(Guid userId, int pageNumber, int pageSize)
        {
            return await _transactionRepository.GetAllTransactionsAsync(userId, pageNumber, pageSize);
        }

        public Task<IEnumerable<Transaction>> GetFilteredTransactionsAsync(Guid userId, DateTime? startDate, DateTime? endDate, string type, string category)
        {
            return _transactionRepository.GetFilteredTransactionsAsync(userId, startDate, endDate, type, category);
        }

        public async Task<Transaction> CreateTransactionAsync(Guid userId, decimal amount, string type, string category, string description, DateTime date)
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Amount = amount,
                Type = type,
                Category = category,
                Description = description,
                Date = date,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return await _transactionRepository.CreateTransactionAsync(transaction);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(Guid userId)
        {
            return await _transactionRepository.GetTransactionsByUserIdAsync(userId);
        }
    }
}
