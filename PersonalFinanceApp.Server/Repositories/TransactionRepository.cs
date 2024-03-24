using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Server.DTO;
using PersonalFinanceApp.Server.Infrastructure;
using PersonalFinanceApp.Server.Models;
using PersonalFinanceApp.Server.Repositories.IRepository;

namespace PersonalFinanceApp.Server.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TotalIncomeExpenseDTO> GetTotalIncomeExpenseAsync(int year, int? month)
        {
            var query = _context.Transactions.Where(t => t.Date.Year == year);

            if (month.HasValue)
            {
                query = query.Where(t => t.Date.Month == month);
            }

            var totalIncome = await query.Where(t => t.Type == "income").SumAsync(t => t.Amount);
            var totalExpense = await query.Where(t => t.Type == "expense").SumAsync(t => t.Amount);

            return new TotalIncomeExpenseDTO
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense
            };
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync(Guid userId, int pageNumber, int pageSize)
        {
            var query = _context.Transactions.Where(t => t.UserId == userId)
                                             .OrderByDescending(t => t.Date)
                                             .Skip((pageNumber - 1) * pageSize)
                                             .Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetFilteredTransactionsAsync(Guid userId, DateTime? startDate, DateTime? endDate, string type, string category)
        {
            var query = _context.Transactions.Where(t => t.UserId == userId);

            if (startDate != null)
                query = query.Where(t => t.Date >= startDate);

            if (endDate != null)
                query = query.Where(t => t.Date <= endDate);

            if (!string.IsNullOrEmpty(type))
            {
                type = type.ToUpper();
                if (type != "ALL")
                    query = query.Where(t => t.Type.ToUpper() == type);
            }

            if (!string.IsNullOrEmpty(category))
            {
                category = category.ToUpper();
                if (category != "ALL")
                    query = query.Where(t => t.Category.ToUpper() == category);
            }

            return await query.ToListAsync();
        }


        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(Guid userId)
        {
            return await _context.Transactions.Where(t => t.UserId == userId).ToListAsync();
        }
    }
}
